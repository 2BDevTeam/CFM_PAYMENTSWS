using Hangfire;
using Microsoft.EntityFrameworkCore;
using LENMEDWS.Domains.Models;
using LENMEDWS.DTOs;
using LENMEDWS.Persistence.Contexts;
using System.Diagnostics;

namespace LENMEDWS.Helper
{
    public class LogHelper
    {
    //    private readonly List<ResponseCodesDTO> logabbleCodes = new List<ResponseCodesDTO>
        
    //{
    //    new ResponseCodesDTO ("0000","Success",1),
    //    new ResponseCodesDTO("0001", "Incorrect HTTP method"),
    //    new ResponseCodesDTO("0002", "Invalid JSON"),
    //    new ResponseCodesDTO("0003", "Incorrect API Key"),
    //    new ResponseCodesDTO("0004", "Api Key not provided"),
    //    new ResponseCodesDTO("0005", "Invalid Reference"),
    //    new ResponseCodesDTO("0006", "Duplicated payment"),
    //    new ResponseCodesDTO("0007", "Internal Error"),
    //    new ResponseCodesDTO("0008", "Invalid Amount Used"),
    //    new ResponseCodesDTO("0009", "Request Id not provided"),
    //    new ResponseCodesDTO("I-500", "Internal Error During Call Remote Api")
    //};



        public LogHelper()
        {
        }


        public void generateLog(ResponseDTO response, string requestId, string operation)
        {
            try
            {
                BackgroundJob.Enqueue(
           () => generateLogJB(response, requestId, operation));
            }
            catch (Exception ex)
            {
                Debug.Print(" SAVE LOG FAILED " + ex.ToString());

            }


        }

        public void GenerateApiLog(ResponseDTO response, string requestId, string operation, string responseText)
        {
            try
            {
                BackgroundJob.Enqueue(
                              () => GenerateApiLogJB(response, requestId, operation, responseText));
            }
            catch (Exception ex)
            {
                Debug.Print(" SAVE LOG FAILED " + ex.ToString());

            }
        }

        public void GenerateApiLogJB(ResponseDTO response, string requestId, string operation, string responseText)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            var configuration = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile($"appsettings.json");



            var config = configuration.Build();
            var connString = config.GetConnectionString("DBconnect");
            optionsBuilder.UseSqlServer(connString);



            using (AppDbContext context = new AppDbContext(optionsBuilder.Options))
            {

                ApiLogs apiLogs = new ApiLogs { Code = response?.response?.cod, RequestId = requestId, ResponseDesc = response?.response?.codDesc, Data = DateTime.Now, Content = response?.Content?.ToString(), Operation = operation, ResponseText = responseText };

                context.ApiLogs.Add(apiLogs);
                context.SaveChanges();

                Debug.Print($"RESPONSE LOG GENERATED");

            }

        }
    
        public decimal generateResponseID()
        {
            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            // Convert the timestamp to a 16-digit string
            decimal sixteenDigitNumber = decimal.Parse( timestamp.ToString("D16"));
            return sixteenDigitNumber;
        }

        public void generateLogJB(ResponseDTO response, string requestId, string operation)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            var configuration = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile($"appsettings.json");



            var config = configuration.Build();
            var connString = config.GetConnectionString("DBconnect");
            optionsBuilder.UseSqlServer(connString);



            using (AppDbContext context = new AppDbContext(optionsBuilder.Options))
            {
               // var codeLogable = logabbleCodes.Where(loggable => loggable?.cod == response?.response?.cod).Count();

              //  if (codeLogable > 0)
               // {

                    Log Log = new Log { Code = response?.response?.cod, RequestId = requestId, ResponseDesc = response?.response.codDesc, Data = DateTime.Now, Content = response?.Content?.ToString(), Operation = operation };

                    context.Log.Add(Log);
                    context.SaveChanges();

             //   }


                //your logic
            }
        }


        public void generateResponseLog(ResponseDTO response, string requestId, string operation, string responseText)
        {

            try
            {
                BackgroundJob.Enqueue(
           () => generateResponseLogJB(response, requestId, operation,responseText));
            }
            catch (Exception ex)
            {
                Debug.Print($" ERROR TRYING TO GENERATE LOG RESPONSE Response  MESSAGE: {ex?.Message?.ToString()} INNER {ex?.InnerException?.ToString()}  ");

            }
        }
        public void generateResponseLogJB(ResponseDTO response, string requestId, string operation,string responseText)
        {
                var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
                var configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile($"appsettings.json");



                var config = configuration.Build();
                var connString = config.GetConnectionString("DBconnect");
                optionsBuilder.UseSqlServer(connString);



                using (AppDbContext context = new AppDbContext(optionsBuilder.Options))
                {
                    
                    Log Log = new Log { Code = response?.response?.cod, RequestId = requestId, ResponseDesc = response?.response?.codDesc, Data = DateTime.Now, Content = response?.Content?.ToString(), Operation = operation,ResponseText=responseText };

                    context.Log.Add(Log);
                    context.SaveChanges();

                    Debug.Print($"RESPONSE LOG GENERATED");
                   
                }

            }
          
        }



    }

