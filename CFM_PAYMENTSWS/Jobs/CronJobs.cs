using Hangfire;
using CFM_PAYMENTSWS.Services;
using CFM_PAYMENTSWS.Persistence.Contexts;
using CFM_PAYMENTSWS.Domains.Interfaces;
using Microsoft.EntityFrameworkCore;
using CFM_PAYMENTSWS.Persistence.Repositories;

namespace CFM_PAYMENTSWS.Jobs
{
    public class CronJobs
    {


        private PaymentService paymentService = new PaymentService();


        public void JobHandler()
        {

            RecurringJob.AddOrUpdate(
               "processarPagamentos",
               () => paymentService.ProcessarPagamentos(),
              Cron.Minutely()
              );

            RecurringJob.AddOrUpdate(
               "processarEmails",
               () => paymentService.ProcessarEmails(),
              //"*/30 * * * * *"
              Cron.Yearly()
              );


        }

    }
}
