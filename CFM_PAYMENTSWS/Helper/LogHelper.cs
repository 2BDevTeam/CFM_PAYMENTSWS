//using Hangfire;
using CFM_PAYMENTSWS.Domains.Models;
using CFM_PAYMENTSWS.DTOs;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using CFM_PAYMENTSWS.Domains.Models;
using CFM_PAYMENTSWS.DTOs;
using CFM_PAYMENTSWS.Persistence;
using System.Diagnostics;
using CFM_PAYMENTSWS.Persistence.Contexts;

namespace CFM_PAYMENTSWS.Helper
{
    public class LogHelper
    {
        private readonly List<ResponseCodesDTO> logabbleCodes = new List<ResponseCodesDTO>
    {
        new ResponseCodesDTO ("0000","Success"),
        new ResponseCodesDTO("0001", "Incorrect HTTP method"),
        new ResponseCodesDTO("0002", "Invalid JSON"),
        new ResponseCodesDTO("0003", "Incorrect API Key"),
        new ResponseCodesDTO("0004", "Api Key not provided"),
        new ResponseCodesDTO("0005", "Invalid Reference"),
        new ResponseCodesDTO("0006", "Duplicated payment"),
        new ResponseCodesDTO("0007", "Internal Error"),
        new ResponseCodesDTO("0008", "Invalid Amount Used"),
        new ResponseCodesDTO("0009", "Request Id not provided"),
        new ResponseCodesDTO("I-500", "Internal Error During Call Remote Api")
    };



        public LogHelper()
        {
        }


        public decimal generateResponseID()
        {
            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            // Convert the timestamp to a 16-digit string
            decimal sixteenDigitNumber = decimal.Parse(timestamp.ToString("D16"));
            return sixteenDigitNumber;
        }
        public void generateLog(ResponseDTO response, string requestId, string operation, object contentlog)
        {
            try
            {
                BackgroundJob.Enqueue(
           () => generateLogJB(response, requestId, operation, contentlog));
            }
            catch (Exception ex)
            {
                Debug.Print(" SAVE LOG FAILED " + ex.ToString());

            }


        }


        public void generateLogJB_PHC(ResponseDTO response, string requestId, string operation)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PHCDbContext>();
            var configuration = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile($"appsettings.json");



            var config = configuration.Build();
            var connString = config.GetConnectionString("ConnStrE14");
            optionsBuilder.UseSqlServer(connString);


            using (PHCDbContext context = new PHCDbContext(optionsBuilder.Options))
            {
               
                Log log = new Log { Code = response?.response?.cod, RequestId = requestId, ResponseDesc = response?.Data?.ToString(), Data = DateTime.Now, Content = response?.Content?.ToString(), Operation = operation };

                //context.Log.Add(log);
                //context.SaveChanges();
             
            }
        }


        public void generateLogJB(ResponseDTO response, string requestId, string operation, object contentlog)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            var configuration = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile($"appsettings.json");



            var config = configuration.Build();
            var connString = config.GetConnectionString("ConnStr");
            optionsBuilder.UseSqlServer(connString);



            using (AppDbContext context = new AppDbContext(optionsBuilder.Options))
            {


                Log log = new Log { Code = response?.response?.cod, RequestId = requestId, ResponseDesc = response?.Data?.ToString(), Data = DateTime.Now, Content = contentlog.ToString(), Operation = operation };

                context.Log.Add(log);
                context.SaveChanges();

            }
        }


    }

}