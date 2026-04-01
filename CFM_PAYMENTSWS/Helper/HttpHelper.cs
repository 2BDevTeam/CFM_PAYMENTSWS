using Newtonsoft.Json;
using CFM_PAYMENTSWS.DTOs;
using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Http.Extensions;

namespace CFM_PAYMENTSWS.Helper
{
    public class HttpHelper
    {   
        private readonly APIHelper apiHelper = new APIHelper();
        private readonly ProviderHelper providerHelper=new ProviderHelper();
        public ResponseDTO ParseToDefaultResponse(string responseText)
        {
            try
            {
                return JsonConvert.DeserializeObject<ResponseDTO>(responseText);
            }
            catch (Exception ex)
            {
                Debug.Print($" ERROR TRYING TO PARSE TO DEFAULT Response {responseText} MESSAGE: {ex?.Message?.ToString()} INNER {ex?.InnerException?.ToString()}  ");
                return null;
            }

        }
        public HttpWebRequest getHttpWebRequestByProvider(decimal providerCode, string grupo,string parameters)
        {
            Debug.Print("Provier1" );
            var providerData = providerHelper.getProviderByGroup(providerCode, grupo);
            Debug.Print("Provier: "+ JsonConvert.SerializeObject(providerData));

            var Address = providerHelper.getProviderByKey(providerData,"address");
            var Path = providerHelper.getProviderByKey(providerData, "path");
            var ContentType = providerHelper.getProviderByKey(providerData, "contentType");
            var Method = providerHelper.getProviderByKey(providerData, "method");
            var Host = providerHelper.getProviderByKey(providerData, "host");
            var UseDefaultCredentials = false;
            var Authorization = providerHelper.getProviderByKey(providerData, "authorization");
            var userAgent = providerHelper.getProviderByKey(providerData, "User-Agent");
            var Origin = "PHC";

            var url = "https://" + Address + Path + parameters;

            Debug.Print($"METHOD Authorization {grupo} {providerCode}  {Authorization}");
            Debug.Print("URL: " + url?.ToString());
            Debug.Print("Host: " + Host?.ToString());


            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = ContentType;
            httpWebRequest.Accept = ContentType;
            httpWebRequest.Method = Method;
            //httpWebRequest.Host = Host;
           // httpWebRequest.UseDefaultCredentials = UseDefaultCredentials;
            httpWebRequest.Headers.Add("Authorization","Bearer "+ Authorization);
            Debug.Print("Authorization: " + Authorization);

            // httpWebRequest.Headers.Add("Origin", Origin);
            httpWebRequest.Headers.Add("User-Agent", userAgent);

            // Imprimir o conteúdo completo do cabeçalho
            Debug.Print("Saindo do helper: "+ httpWebRequest.ToString());
            return httpWebRequest;
        }


        public HttpWebRequest GetHttpWebRequestByEntityAndRoute(string entity, string route)
        {
            API apiData;
            apiData = apiHelper.getApiEntity(entity, route);

            if (apiData?.status == null || apiData?.status == "0")
            {
                throw new Exception("API_DATA_NOT_FOUND");
            }
            var endpoint = apiData.endpoints.Where(endpoint => endpoint.operationCode == route).FirstOrDefault();

            //Debug.Print($"GetHttpWebRequestByEntityAndRoute  {endpoint.ToString()}");

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(endpoint.url?.Trim());
            httpWebRequest.ContentType = endpoint.contentType;
            httpWebRequest.Method = endpoint.method;

            return httpWebRequest;
        }


    }
}
