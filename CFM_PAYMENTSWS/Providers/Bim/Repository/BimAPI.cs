using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using CFM_PAYMENTSWS.Domains.Models;
using CFM_PAYMENTSWS.Helper;
using CFM_PAYMENTSWS.Providers.BCI.DTOs;
using System.Diagnostics;
using System.Net;
using System.Text.Json.Nodes;
using Newtonsoft.Json.Linq;
using CFM_PAYMENTSWS.Providers.Bim.DTOs;
using CFM_PAYMENTSWS.DTOs;
using CFM_PAYMENTSWS.Extensions;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.IdentityModel.Logging;

namespace CFM_PAYMENTSWS.Providers.Bim.Repository
{


    public class BimAPI
    {
        private readonly HttpHelper httpHelper = new HttpHelper();
        private readonly APIHelper apiHelper = new APIHelper();




        public async Task<string> Authenticate()
        {
            Debug.Print("BimAPI.Auth");

            API apiData;
            var result = "";
            var requestId = KeysExtension.generateRequestId();
            var data = new object();

            try
            {

                apiData = apiHelper.getApiEntity("Bim", "AUTH");

                if (apiData?.status == null || apiData?.status == "0")
                {
                    ResponseDTO response = new ResponseDTO(new ResponseCodesDTO("I-500", apiData.message), null, null);
                    //logHelper.generateLogJB(response, requestId, "BimAPI.authenticate");

                    return "UNAUTHORIZED";
                }

                var endpoint = apiData.endpoints.Where(endpoint => endpoint.operationCode == "login").FirstOrDefault();

                Debug.Print("   " + endpoint.ToString());

                var httpWebRequest = httpHelper.GetHttpWebRequestByEntityAndRoute("Bim", "login");

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {

                    data = new
                    {
                        scope = endpoint.credentials.username.ToString().Trim(),
                        password = endpoint.credentials.password.ToString().Trim(),

                    };

                    string json = await Task.Run(() => JsonConvert.SerializeObject(data));

                    Debug.Print("json AUTHHH data " + json);

                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                {
                    StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream());
                    result = streamReader.ReadToEnd();
                    var statuscode = httpResponse.StatusCode.ToString();
                    using (streamReader = new StreamReader(httpResponse.GetResponseStream())) ;

                    BimAuthResponse bimAuthResponse = JsonConvert.DeserializeObject<BimAuthResponse>(result);
                    Debug.WriteLine("Resultado Auth Token " + bimAuthResponse.token);

                    return bimAuthResponse.token;

                }
            }
            catch (WebException ex)
            {
                HttpWebResponse response;


                if (ex?.Response?.GetResponseStream() == null)
                {
                    ResponseDTO responseex = new ResponseDTO(new ResponseCodesDTO("I-500", "GetResponseStreamSemResposta"), null, JsonConvert.SerializeObject(data));

                    //logHelper.generateLogJB(responseex, requestId, "BCXAPI.authenticate");

                    return "UNAUTHORIZED";
                }

                StreamReader reader = new StreamReader(ex.Response.GetResponseStream());

                string rawresp = reader.ReadToEnd();

                ResponseDTO responseex2 = new ResponseDTO(new ResponseCodesDTO("I-500", rawresp.ToString()), null, null);

                //logHelper.generateLogJB(responseex2, requestId, "BCXAPI.authenticate");

                return "UNAUTHORIZED";
            }

        }


        public async Task<BimResponseDTO> loadPayments(Paymentv1_5 payment)
        {

            try
            {
                string authResult = await Authenticate();

                var httpWebRequest = httpHelper.GetHttpWebRequestByEntityAndRoute("Bim", "loadpayments");

                string result = "";

                httpWebRequest.Headers.Add("Authorization", $"Bearer {authResult}");
                httpWebRequest.Headers.Add("Scope", "CFM");

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = payment.ToString();

                    Debug.Print($"loadPayments {json} ");

                    JObject jsonObject = JObject.Parse(json);
                    RemoveNullProperties(jsonObject);

                    string cleanedJson = jsonObject.ToString(Formatting.None);
                    Debug.Print(cleanedJson);
                    Debug.Print($"loadPayments cleanedJson {cleanedJson} ");

                    streamWriter.Write(cleanedJson);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                StreamReader streamReader = new System.IO.StreamReader(httpResponse.GetResponseStream());

                result = streamReader.ReadToEnd();

                BimResponseDTO response = JsonConvert.DeserializeObject<BimResponseDTO>(result);

                Debug.Print("Dto deserealizado" + JsonConvert.SerializeObject(response));
                if (response == null)
                {
                    throw new Exception();
                }


                return response;
            }

            catch (WebException ex)
            {
                int statusCode = 500;
                Debug.Print($"WEB EXCPPP {ex.ToString()} ");
                Debug.Print("CODIGO DE RESPOSTA" + ex.Status.ToString());

                if (ex.Response is HttpWebResponse httpResponse)
                {
                    statusCode = (int)httpResponse.StatusCode;
                    Debug.Print("A solicitação web retornou o código de erro: " + statusCode);
                }

                return new BimResponseDTO(payment.BatchId, ex.ToString(), statusCode.ToString(), ex.Response?.ToString() ?? "No response");

            }
            catch (Exception ex)
            {
                return new BimResponseDTO(payment.BatchId, ex.ToString(), "0007", "Internal Error");
                // return getGeneralExceptionResponse(ex, consumidores, "ForUTechRepository.inserirConsumidores");
            }
        }


        public BimResponseDTO checkPayments(string batchId, string initgPtyCode)
        {

            try
            {
                string result = "";

                HttpWebRequest httpWebRequest = httpHelper.getHttpWebRequestByProvider(107, "loadPayments", $"/{batchId}?initgPtyCode={initgPtyCode}");

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                StreamReader streamReader = new System.IO.StreamReader(httpResponse.GetResponseStream());

                result = streamReader.ReadToEnd();

                BimResponseDTO response = JsonConvert.DeserializeObject<BimResponseDTO>(result);

                Debug.Print("Dto deserealizado" + JsonConvert.SerializeObject(response));
                if (response == null)
                {
                    throw new Exception();
                }


                return response;
            }

            catch (WebException ex)
            {
                int statusCode = 500;
                Debug.Print($"WEB EXCPPP {ex.ToString()} ");
                Debug.Print("CODIGO DE RESPOSTA" + ex.Status.ToString());

                if (ex.Response is HttpWebResponse httpResponse)
                {
                    statusCode = (int)httpResponse.StatusCode;
                    Debug.Print("A solicitação web retornou o código de erro: " + statusCode);
                }

                return new BimResponseDTO(batchId, ex.ToString(), statusCode.ToString(), ex.Response?.ToString() ?? "No response");

            }
            catch (Exception ex)
            {
                return new BimResponseDTO(batchId, ex.ToString(), "0007", "Internal Error");
                // return getGeneralExceptionResponse(ex, consumidores, "ForUTechRepository.inserirConsumidores");
            }
        }

        /*
        public BCICheckPaymentReportResponseDTO validatePayments(string batchId)
        {

            try
            {
                string result = "";

                HttpWebRequest httpWebRequest = httpHelper.getHttpWebRequestByProvider(105, "checkpaymentsreport", "");

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    CheckPaymentsReportDTO checkPaymentsReportDTO = new CheckPaymentsReportDTO { BatchId = batchId };
                    string json = JsonConvert.SerializeObject(checkPaymentsReportDTO);


                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                StreamReader streamReader = new System.IO.StreamReader(httpResponse.GetResponseStream());

                result = streamReader.ReadToEnd();

                BCICheckPaymentReportResponseDTO response = JsonConvert.DeserializeObject<BCICheckPaymentReportResponseDTO>(result);

                Debug.Print("Dto deserealizado" + response.BatchId.ToString());
                if (response == null)
                {

                    throw new Exception();
                }



                //response.content = payment;

                return response;
            }

            catch (WebException ex)
            {
                int statusCode = 500;
                if (ex.Response is HttpWebResponse httpResponse)
                {
                    statusCode = (int)httpResponse.StatusCode;
                    // Faça o que for necessário com o código de status HTTP
                    // Retorne ou registre o código de status, por exemplo:
                    Debug.Print("A solicitação web retornou o código de erro: " + statusCode);
                }

                //var rpp = (HttpWebResponse)ex.Response;
                Debug.Print($"WEB EXCPPP {ex.ToString()} ");
                Debug.Print("CODIGO DE RESPOSTA" + ex.Status.ToString());

                return new BCICheckPaymentReportResponseDTO { BatchId = batchId, StatusCode = statusCode.ToString(), StatusDescription = ex.Response.ToString() };
                //return new BCIResponseDTO(batchId, ex.ToString(), statusCode.ToString(), ex.Response.ToString());
            }
            catch (Exception ex)
            {
                return new BCICheckPaymentReportResponseDTO { BatchId = batchId, StatusCode = "0007", StatusDescription = "Internal Error" };
                //   return new BCIResponseDTO(batchId, ex.ToString(), "0007", "Internal Error");
                // return getGeneralExceptionResponse(ex, consumidores, "ForUTechRepository.inserirConsumidores");
            }
        }
        */

        public void RemoveNullProperties(JToken token)
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
