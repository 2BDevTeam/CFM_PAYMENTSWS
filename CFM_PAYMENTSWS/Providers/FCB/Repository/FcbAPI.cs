using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CFM_PAYMENTSWS.Helper;
using CFM_PAYMENTSWS.Providers.FCB.DTOs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CFM_PAYMENTSWS.Providers.FCB.Repository
{
    public class FcbAPI
    {
        private readonly APIHelper apiHelper = new();
        private const string Entity = "FCB";

        public async Task<FcbResponseDTO> LoadPaymentsAsync(FcbPaymentDTO payment)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                var authResponse = await AuthenticateAsync();
                if (authResponse == null || string.IsNullOrWhiteSpace(authResponse.AccessToken))
                {
                    var authError = authResponse == null
                        ? "Auth response null"
                        : $"Attempt {authResponse.AttemptCount}/5; HttpStatus={authResponse.HttpStatusCode}; error={authResponse.Error}; error_description={authResponse.ErrorDescription}; payload={authResponse.RawPayload}";

                    throw new Exception($"UNAUTHORIZED - TOKEN_ERROR: {authError}");
                }

                var httpWebRequest = GetRequest("loadPayments");
                var endpointUrl = httpWebRequest.RequestUri.ToString();
                httpWebRequest.Accept = httpWebRequest.ContentType;
                httpWebRequest.Headers.Add("Authorization", $"Bearer {authResponse.AccessToken}");
                httpWebRequest.Headers.Add("Request-UUID", Guid.NewGuid().ToString());
                httpWebRequest.Headers.Add("Application-ID", "CFM");

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    var jsonObject = JObject.Parse(payment.ToString());
                    RemoveNullProperties(jsonObject);
                    var cleanedJson = jsonObject.ToString(Formatting.None);
                    Debug.Print($"LoadPaymentsAsync  {cleanedJson}");
                    await streamWriter.WriteAsync(cleanedJson);
                    await streamWriter.FlushAsync();
                }

                using var httpResponse = (HttpWebResponse)await httpWebRequest.GetResponseAsync();
                var httpStatusCode = (int)httpResponse.StatusCode;
                using var streamReader = new StreamReader(httpResponse.GetResponseStream());
                var result = await streamReader.ReadToEndAsync();

                var response = JsonConvert.DeserializeObject<FcbResponseDTO>(result);
                if (response != null)
                {
                    stopwatch.Stop();
                    response.HttpStatusCode = httpStatusCode;
                    response.DurationMs = (int)stopwatch.ElapsedMilliseconds;
                    response.EndpointUrl = endpointUrl;
                    return response;
                }

                stopwatch.Stop();
                var emptyResponse = new FcbResponseDTO
                {
                    StatusCode = "0007",
                    StatusDescription = "Empty response from FCB service",
                    HttpStatusCode = httpStatusCode,
                    DurationMs = (int)stopwatch.ElapsedMilliseconds,
                    EndpointUrl = endpointUrl
                };
                return emptyResponse;
            }
            catch (WebException ex)
            {
                stopwatch.Stop();
                var statusCode = HttpStatusCode.InternalServerError;
                if (ex.Response is HttpWebResponse errorResponse)
                {
                    statusCode = errorResponse.StatusCode;
                    using var reader = new StreamReader(errorResponse.GetResponseStream());
                    var errorPayload = await reader.ReadToEndAsync();
                    try
                    {
                        var errorDto = JsonConvert.DeserializeObject<FcbResponseDTO>(errorPayload);
                        if (errorDto != null)
                        {
                            errorDto.HttpStatusCode = (int)statusCode;
                            errorDto.DurationMs = (int)stopwatch.ElapsedMilliseconds;
                            errorDto.EndpointUrl = GetRequest("loadPayments").RequestUri.ToString();
                            return errorDto;
                        }
                    }
                    catch
                    {
                        var errorResponse1 = new FcbResponseDTO
                        {
                            StatusCode = ((int)statusCode).ToString(),
                            StatusDescription = string.IsNullOrWhiteSpace(errorPayload) ? ex.Message : errorPayload,
                            HttpStatusCode = (int)statusCode,
                            DurationMs = (int)stopwatch.ElapsedMilliseconds,
                            EndpointUrl = GetRequest("loadPayments").RequestUri.ToString()
                        };
                        return errorResponse1;
                    }
                }

                return new FcbResponseDTO
                {
                    StatusCode = ((int)statusCode).ToString(),
                    StatusDescription = ex.Message,
                    HttpStatusCode = (int)statusCode,
                    DurationMs = (int)stopwatch.ElapsedMilliseconds,
                    EndpointUrl = GetRequest("loadPayments").RequestUri.ToString()
                };
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                return new FcbResponseDTO
                {
                    StatusCode = "0007",
                    StatusDescription = ex.Message,
                    HttpStatusCode = 500,
                    DurationMs = (int)stopwatch.ElapsedMilliseconds,
                    EndpointUrl = GetRequest("loadPayments").RequestUri.ToString()
                };
            }
        }

        private async Task<FcbAuthResponseDTO?> AuthenticateAsync()
        {
            var apiData = apiHelper.getApiEntity(Entity, "login");
            var endpoint = apiData?.endpoints?.FirstOrDefault(e => e.operationCode == "login");
            if (endpoint == null)
            {
                throw new Exception("FCB login endpoint not configured");
            }

            const int maxAttempts = 5;
            FcbAuthResponseDTO? lastResponse = null;

            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                lastResponse = await RequestTokenOnceAsync(endpoint.url.Trim(), endpoint.method, endpoint.contentType, endpoint.credentials.username, endpoint.credentials.password);
                if (lastResponse != null)
                    lastResponse.AttemptCount = attempt;

                if (lastResponse != null && !string.IsNullOrWhiteSpace(lastResponse.AccessToken))
                {
                    return lastResponse;
                }

                var shouldRetry = attempt < maxAttempts && IsTransientTokenFailure(lastResponse);
                if (!shouldRetry)
                {
                    return lastResponse;
                }

                await Task.Delay(500 * attempt);
            }

            return lastResponse;
        }

        private async Task<FcbAuthResponseDTO?> RequestTokenOnceAsync(string url, string method, string contentType, string username, string password)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.Expect100Continue = false;
                ServicePointManager.ServerCertificateValidationCallback = (_, _, _, _) => true;

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.Method = method;
                httpWebRequest.ContentType = contentType;
                httpWebRequest.Accept = "application/json";
                httpWebRequest.Timeout = 30000;
                httpWebRequest.ReadWriteTimeout = 30000;
                httpWebRequest.KeepAlive = true;

                var credentials = $"{username}:{password}";
                var base64Credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials));
                httpWebRequest.Headers.Add("Authorization", $"Basic {base64Credentials}");

                var body = "grant_type=client_credentials";
                var data = Encoding.UTF8.GetBytes(body);
                httpWebRequest.ContentLength = data.Length;

                using (var requestStream = await httpWebRequest.GetRequestStreamAsync())
                {
                    await requestStream.WriteAsync(data, 0, data.Length);
                    await requestStream.FlushAsync();
                }

                using var httpResponse = (HttpWebResponse)await httpWebRequest.GetResponseAsync();
                using var streamReader = new StreamReader(httpResponse.GetResponseStream());
                var result = await streamReader.ReadToEndAsync();
                var success = JsonConvert.DeserializeObject<FcbAuthResponseDTO>(result) ?? new FcbAuthResponseDTO();
                success.HttpStatusCode = (int)httpResponse.StatusCode;
                success.RawPayload = result;
                return success;
            }
            catch (WebException ex)
            {
                if (ex.Response is HttpWebResponse errorResponse)
                {
                    using var reader = new StreamReader(errorResponse.GetResponseStream());
                    var payload = await reader.ReadToEndAsync();
                    var errorDto = JsonConvert.DeserializeObject<FcbAuthResponseDTO>(payload) ?? new FcbAuthResponseDTO();
                    errorDto.HttpStatusCode = (int)errorResponse.StatusCode;
                    errorDto.RawPayload = payload;
                    return errorDto;
                }

                throw;
            }
        }

        private static bool IsTransientTokenFailure(FcbAuthResponseDTO? response)
        {
            if (response == null)
                return true;

            var status = response.HttpStatusCode ?? 0;
            if (status == 499 || status == 408 || status == 429 || status >= 500)
                return true;

            return false;
        }

        private HttpWebRequest GetRequest(string operationCode)
        {
            var apiData = apiHelper.getApiEntity(Entity, operationCode);
            var endpoint = apiData?.endpoints?.FirstOrDefault(e => e.operationCode == operationCode);
            if (endpoint == null)
            {
                throw new Exception($"FCB endpoint {operationCode} not configured");
            }

            ServicePointManager.ServerCertificateValidationCallback = (_, _, _, _) => true;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(endpoint.url.Trim());
            httpWebRequest.Method = endpoint.method;
            httpWebRequest.ContentType = endpoint.contentType;
            httpWebRequest.Timeout = 30000;
            httpWebRequest.ReadWriteTimeout = 30000;
            return httpWebRequest;
        }

        private static void RemoveNullProperties(JToken token)
        {
            if (token.Type == JTokenType.Object)
            {
                var propertiesToRemove = new List<JProperty>();
                foreach (var property in token.Children<JProperty>())
                {
                    if (property.Value.Type == JTokenType.Null)
                    {
                        propertiesToRemove.Add(property);
                    }
                    else
                    {
                        RemoveNullProperties(property.Value);
                    }
                }

                foreach (var property in propertiesToRemove)
                {
                    property.Remove();
                }
            }
            else if (token.Type == JTokenType.Array)
            {
                foreach (var item in token.Children())
                {
                    RemoveNullProperties(item);
                }
            }
        }
    }
}
