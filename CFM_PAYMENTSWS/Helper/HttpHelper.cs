using Newtonsoft.Json;
using CFM_PAYMENTSWS.DTOs;
using System.Diagnostics;
using System.Net;

namespace CFM_PAYMENTSWS.Helper
{
    public class HttpHelper
    {   
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
        public HttpWebRequest getHttpWebRequestByProviderApiKey(decimal providerCode, string grupo, string parameters)
        {
            Debug.Print("Provier1");

            var providerData = providerHelper.getProviderByGroup(providerCode, grupo);
            Debug.Print("Provier: " + providerData.ToString());

            var Address = providerHelper.getProviderByKey(providerData, "address");
            var Path = providerHelper.getProviderByKey(providerData, "path");
            var ContentType = providerHelper.getProviderByKey(providerData, "contentType");
            var Method = providerHelper.getProviderByKey(providerData, "method");
            var Host = providerHelper.getProviderByKey(providerData, "host");
            var UseDefaultCredentials = false;
            var Authorization = providerHelper.getProviderByKey(providerData, "authorization");
            var userAgent = providerHelper.getProviderByKey(providerData, "User-Agent");
            var Origin = "PHC";

            var url = "https://" + Address + Path + parameters;

            Debug.Print($"METHOD Authorization {grupo}{Authorization}");
            Debug.Print("URL: " + url?.ToString());
            Debug.Print("Host: " + Host?.ToString());

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = ContentType;
            httpWebRequest.Accept = ContentType;
            httpWebRequest.Method = Method;
            //httpWebRequest.Host = Host;
            // httpWebRequest.UseDefaultCredentials = UseDefaultCredentials;
            //httpWebRequest.Headers.Add("Authorization","Bearer "+ Authorization);
            httpWebRequest.Headers.Add("User-Agent", userAgent);


            /*  CONTA OBA  */
            httpWebRequest.Headers.Add("Authorization", "Token " + "9ce660df65c753deea9462726647d5c0f586a5fe");
            
            /*  MINHA CONTA  */
            //httpWebRequest.Headers.Add("Authorization", "Token " + "7f74c66bd3c76132c8a4388a9f6d2e9aad876971");


            // Imprimir o conteúdo completo do cabeçalho
            Debug.Print("Saindo do helper: " + httpWebRequest.ToString());
            return httpWebRequest;
        }
        public HttpWebRequest getHttpWebRequestByProvider(decimal providerCode, string grupo,string parameters)
        {
            Debug.Print("Provier1" );
            var providerData = providerHelper.getProviderByGroup(providerCode, grupo);
            Debug.Print("Provier: "+ providerData.ToString());

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

            Debug.Print($"METHOD Authorization {grupo}   {Authorization}");
            Debug.Print("URL: " + url?.ToString());
            Debug.Print("Host: " + Host?.ToString());

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = ContentType;
            httpWebRequest.Accept = ContentType;
            httpWebRequest.Method = Method;
            //httpWebRequest.Host = Host;
           // httpWebRequest.UseDefaultCredentials = UseDefaultCredentials;
            httpWebRequest.Headers.Add("Authorization","Bearer "+ Authorization);

            // httpWebRequest.Headers.Add("Origin", Origin);
            httpWebRequest.Headers.Add("User-Agent", userAgent);

            // Imprimir o conteúdo completo do cabeçalho
            Debug.Print("Saindo do helper: "+ httpWebRequest.ToString());
            return httpWebRequest;
        }
        


    }
}
