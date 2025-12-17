using CFM_PAYMENTSWS.Domains.Models;
using CFM_PAYMENTSWS.Helper;
using CFM_PAYMENTSWS.Providers.Moza.DTOs;
using Hangfire.MemoryStorage.Database;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;

namespace CFM_PAYMENTSWS.Providers.Moza.Repository
{
    public class MozaAPI
    {
        private readonly APIHelper apiHelper = new();
        private const string Entity = "MOZA";

        public async Task<MozaPaymentResponseDTO> LoadPaymentsAsync(PaymentCamelCase payment)
        {
            try
            {
                var authResponse = await AuthenticateAsync();
                if (authResponse == null || string.IsNullOrWhiteSpace(authResponse.AccessToken))
                {
                    throw new Exception("UNAUTHORIZED");
                }
                Debug.Print("MOZA Auth Token: " + authResponse.AccessToken);

                var endpoint = GetEndpoint("loadPayments");
                Debug.Print("MOZA Endpoint URL: " + endpoint.url);

                ServicePointManager.ServerCertificateValidationCallback = (_, _, _, _) => true;
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(endpoint.url.Trim());
                httpWebRequest.Method = endpoint.method;
                httpWebRequest.ContentType = endpoint.contentType;
                //httpWebRequest.Accept = endpoint.contentType;
                httpWebRequest.Headers.Add("Authorization", $"Bearer {authResponse.AccessToken}");

                // AUMENTO DO TIMEOUT  
                httpWebRequest.Timeout = 3 * 60 * 1000;
                httpWebRequest.ReadWriteTimeout = 3 * 60 * 1000;

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    var jsonObject = JObject.Parse(payment.ToString());
                    RemoveNullProperties(jsonObject);
                    var cleanedJson = jsonObject.ToString(Formatting.None);
                    Debug.Print($"MOZA LoadPaymentsAsync {cleanedJson}");
                    await streamWriter.WriteAsync(cleanedJson);
                    await streamWriter.FlushAsync();
                }

                using var httpResponse = (HttpWebResponse)await httpWebRequest.GetResponseAsync();
                using var streamReader = new StreamReader(httpResponse.GetResponseStream());
                var result = await streamReader.ReadToEndAsync();
                Debug.Print("MOZA LoadPaymentsAsync Response: " + result);

                var response = JsonConvert.DeserializeObject<MozaPaymentResponseDTO>(result);
                return response ?? new MozaPaymentResponseDTO
                {
                    Data = new MozaPaymentDataDTO
                    {
                        StatusCode = "0007",
                        StatusDescription = "Empty response from MOZA service"
                    }
                };
            }
            catch (WebException ex)
            {
                var statusCode = HttpStatusCode.InternalServerError;
                var statusDescription = ex.Message;

                if (ex.Response is HttpWebResponse errorResponse)
                {
                    statusCode = errorResponse.StatusCode;
                    using var reader = new StreamReader(errorResponse.GetResponseStream());
                    var payload = await reader.ReadToEndAsync();

                    try
                    {
                        var errorDto = JsonConvert.DeserializeObject<MozaPaymentResponseDTO>(payload);
                        if (errorDto != null)
                        {
                            return errorDto;
                        }
                    }
                    catch
                    {
                        statusDescription = string.IsNullOrWhiteSpace(payload) ? statusDescription : payload;
                    }
                }

                return new MozaPaymentResponseDTO
                {
                    Data = new MozaPaymentDataDTO
                    {
                        StatusCode = ((int)statusCode).ToString(),
                        StatusDescription = statusDescription
                    }
                };
            }
            catch (Exception ex)
            {
                return new MozaPaymentResponseDTO
                {
                    Data = new MozaPaymentDataDTO
                    {
                        StatusCode = "0007",
                        StatusDescription = ex.Message
                    }
                };
            }
        }

        private async Task<MozaAuthResponseDTO?> AuthenticateAsync()
        {
            var apiData = apiHelper.getApiEntity(Entity, "login");
            var endpoint = apiData?.endpoints?.FirstOrDefault(e => e.operationCode == "login");
            if (endpoint == null)
            {
                throw new Exception("MOZA login endpoint not configured");
            }
            var data = new object();

            ServicePointManager.ServerCertificateValidationCallback = (_, _, _, _) => true;

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(endpoint.url.Trim());
            httpWebRequest.Method = endpoint.method;
            httpWebRequest.ContentType = endpoint.contentType;

            // AUMENTO DO TIMEOUT  
            httpWebRequest.Timeout = 3 * 60 * 1000;
            httpWebRequest.ReadWriteTimeout = 3 * 60 * 1000;

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {

                data = new
                {
                    username = endpoint.credentials.username.ToString().Trim(),
                    password = endpoint.credentials.password.ToString().Trim(),
                };

                string json = await Task.Run(() => JsonConvert.SerializeObject(data));

                Debug.Print("json AUTHHH data " + json);
                await streamWriter.WriteAsync(json);
                await streamWriter.FlushAsync();
            }

            using var httpResponse = (HttpWebResponse)await httpWebRequest.GetResponseAsync();
            using var streamReader = new StreamReader(httpResponse.GetResponseStream());
            var result = await streamReader.ReadToEndAsync();
            return JsonConvert.DeserializeObject<MozaAuthResponseDTO>(result);
        }

        private CFM_PAYMENTSWS.DTOs.Endpoint GetEndpoint(string operationCode)
        {
            var apiData = apiHelper.getApiEntity(Entity, operationCode);
            var endpoint = apiData?.endpoints?.FirstOrDefault(e => e.operationCode == operationCode);
            if (endpoint == null)
            {
                throw new Exception($"MOZA endpoint {operationCode} not configured");
            }

            return endpoint;
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
