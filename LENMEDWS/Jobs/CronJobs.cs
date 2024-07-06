using Hangfire;
using LENMEDWS.Services;
using LENMEDWS.Persistence.Contexts;
using LENMEDWS.Domains.Interfaces;
using Microsoft.EntityFrameworkCore;
using LENMEDWS.Persistence.Repositories;

namespace LENMEDWS.Jobs
{
    public class CronJobs
    {


        private PHCService PHCService = new PHCService();


        public void JobHandler()
        {

            /*
            RecurringJob.AddOrUpdate(
               "SincronizarFt",
               () => PHCService.SincronizarFt(),
              Cron.Minutely());
            */


        }

    }
}
