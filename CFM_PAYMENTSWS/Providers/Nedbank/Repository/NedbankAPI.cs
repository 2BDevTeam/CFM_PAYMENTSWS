using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using CFM_PAYMENTSWS.Domains.Models;
using CFM_PAYMENTSWS.Helper;
using CFM_PAYMENTSWS.Providers.Nedbank.DTOs;
using System.Diagnostics;
using System.Net;
using System.Text.Json.Nodes;
using Newtonsoft.Json.Linq;

namespace CFM_PAYMENTSWS.Providers.Nedbank.Repository
{

 
    public class NedbankAPI
    {
        private readonly HttpHelper httpHelper = new HttpHelper();
        public NedbankResponseDTO loadPayments(Payment payment)
        {
            
            try
            {
                string result = "";
             
                HttpWebRequest httpWebRequest = httpHelper.getHttpWebRequestByProvider(105, "loadPayments","");
                
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = JsonConvert.SerializeObject(payment);

                    Debug.Print($"loadPayments {json} ");

                    JObject jsonObject = JObject.Parse(json);
                    RemoveNullProperties(jsonObject);

                    string cleanedJson = jsonObject.ToString(Formatting.None);
                    Debug.Print(cleanedJson);

                    streamWriter.Write(cleanedJson);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
               
                StreamReader streamReader = new System.IO.StreamReader(httpResponse.GetResponseStream());
                
                result = streamReader.ReadToEnd();
          
                NedbankResponseDTO response = JsonConvert.DeserializeObject<NedbankResponseDTO>(result);

                Debug.Print("Dto deserealizado"+response.BatchId.ToString());
                if (response == null)
                {
                    
                    throw new Exception();
                }



                //response.content = payment;

                return response;
            }

            catch (WebException ex)
            {
                int statusCode=500;
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
                return new NedbankResponseDTO(payment.BatchId, ex.ToString(), statusCode.ToString(), ex.Response?.ToString() ?? "No response");

            }
            catch (Exception ex)
            {
                return new NedbankResponseDTO(payment.BatchId, ex.ToString(),"0007","Internal Error");
                // return getGeneralExceptionResponse(ex, consumidores, "ForUTechRepository.inserirConsumidores");
            }
        }

        public NedBankCheckPaymentReportResponseDTO validatePayments(string batchId)
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

                NedBankCheckPaymentReportResponseDTO response = JsonConvert.DeserializeObject<NedBankCheckPaymentReportResponseDTO>(result);

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

                return new NedBankCheckPaymentReportResponseDTO { BatchId = batchId, StatusCode = statusCode.ToString(), StatusDescription = ex.Response.ToString() };
                //return new NedbankResponseDTO(batchId, ex.ToString(), statusCode.ToString(), ex.Response.ToString());
            }
            catch (Exception ex)
            {
                return new NedBankCheckPaymentReportResponseDTO { BatchId = batchId, StatusCode = "0007", StatusDescription = "Internal Error" };
             //   return new NedbankResponseDTO(batchId, ex.ToString(), "0007", "Internal Error");
                // return getGeneralExceptionResponse(ex, consumidores, "ForUTechRepository.inserirConsumidores");
            }
        }

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
