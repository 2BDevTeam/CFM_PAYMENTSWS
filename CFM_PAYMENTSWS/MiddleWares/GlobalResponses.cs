using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using CFM_PAYMENTSWS.DTOs;
using CFM_PAYMENTSWS.Helper;
using System.Diagnostics;
using System.Text.Json;

namespace CFM_PAYMENTSWS.MiddleWares
{
    public class GlobalResponses : IAsyncResultFilter
    {

        private readonly LogHelper logHelper = new LogHelper();
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            try
            {


                var result = context.Result as ObjectResult;

                Debug.Print("Response Result: "+result);

                var response = result?.Value as ResponseDTO;

                if (response != null)
                {
                    var requestId = context.HttpContext.Request.Headers["requestId"];

                    var operation = context.HttpContext.Request.Path;
                    Debug.Print(" THE RESULTTTT" + response.response);

                    //   logHelper.generateLog(response, requestId, operation);

                   // await context.HttpContext.Response.WriteAsync(json);
                  //  await next();

                }

            }
            catch (Exception ex)
            {

                await next();
            }



            await next();
        }
    }
}
