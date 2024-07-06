using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Request.Body.Peeker;
using LENMEDWS.DTOs;
using LENMEDWS.Helper;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
namespace LENMEDWS.MiddleWares
{
    public class HttpLoggingMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;
        private readonly HttpHelper parseToDefaultResponse=new HttpHelper();
        private readonly LogHelper logHelper=new LogHelper();


        public HttpLoggingMiddleware(RequestDelegate next, ILogger<HttpLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                MemoryStream responseBody = null;
                Stream originalResponseBody = null;

                try
                {
                    var content =await context.Request.PeekBodyAsync();

                  //  Debug.Print($" THE REQUEST {request?.ToString()}");

                    originalResponseBody = context.Response.Body;
                    responseBody = new MemoryStream();
                    context.Response.Body = responseBody;
                    await _next.Invoke(context);

                   
                    responseBody.Seek(0, SeekOrigin.Begin);
                    var responseText = await new StreamReader(responseBody).ReadToEndAsync();

                    ResponseDTO finalResponse= new ResponseDTO(new ResponseCodesDTO("0000", "Success....", logHelper.generateResponseID()), null, content); ;
                    ResponseDTO tmpResponse = parseToDefaultResponse.ParseToDefaultResponse(responseText);

                    //Debug.Print($"RESPONSE TEXT 2 {tmpResponse?.ToString()}");
                    if(tmpResponse != null)
                    {
                        finalResponse= tmpResponse;
                        finalResponse.Content = content;
                    }
                    string controllerName = context.GetRouteData().Values["controller"]?.ToString();
                    string operationName = context.GetRouteData().Values["action"]?.ToString();


                    //generateResponseLog(ResponseDTO response, string requestId, string operation,string responseText)
                   

                    logHelper.generateResponseLog(finalResponse, finalResponse?.response?.id.ToString(), $"{controllerName}/{operationName}", responseText);
                    finalResponse.Content = null;

                    if(tmpResponse?.response!=null)
                    {
                       // Debug.Print($"TMP NOT NULL");
                        var responseJson = JsonConvert.SerializeObject(finalResponse);
                        var responseBytes = Encoding.UTF8.GetBytes(responseJson);
                        await originalResponseBody.WriteAsync(responseBytes, 0, responseBytes.Length);
                    }
                    else
                    {
                        var responseBytes = Encoding.UTF8.GetBytes(responseText);
                        await originalResponseBody.WriteAsync(responseBytes, 0, responseBytes.Length);

                    }
                    

                    
                }
                finally
                {
                    // Make sure to reset the response body to the original stream
                    if (responseBody != null)
                    {
                        context.Response.Body = originalResponseBody;
                    }

                    // Dispose the MemoryStream if it's not null
                    responseBody?.Dispose();
                }
            }
            catch (Exception ex)
            {

                Debug.Print($"{ex?.Message?.ToString()} STACK: {ex?.StackTrace?.ToString()} INNER {ex?.InnerException?.ToString()}");
                // Create a ResponseDTO for the
                // 

                string controllerName = context.GetRouteData().Values["controller"]?.ToString();
                string operationName = context.GetRouteData().Values["action"]?.ToString();
                var finalResponse = new ResponseDTO(new ResponseCodesDTO("0007", "Exp", 1222), null, null);
                
                var errorDTO = new ErrorDTO { message = ex?.Message, stack = ex?.StackTrace?.ToString(), inner = ex?.InnerException?.ToString() };
                logHelper.generateResponseLog(finalResponse, finalResponse?.response?.id.ToString(), $"{controllerName}/{operationName}", errorDTO.ToString());


                // Serialize the errorResponseDto to JSON
                var errorResponseJson = JsonConvert.SerializeObject(finalResponse);

                // Set the appropriate HTTP status code
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                // Set the response content type
                context.Response.ContentType = "application/json";

                // Write the error response to the response body
                await context.Response.WriteAsync(errorResponseJson);
            }
        }


    }
}

