using Microsoft.EntityFrameworkCore;
using CFM_PAYMENTSWS.Domains.Models;
using CFM_PAYMENTSWS.Persistence.Contexts;
using System.Diagnostics;

namespace CFM_PAYMENTSWS.Helper
{
    public class ProviderHelper
    {

        public List<UProvider> getProviderByGroup(decimal providerCode,string grupo)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            var configuration = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile($"appsettings.json");



            var config = configuration.Build();
            var connString = config.GetConnectionString("ConnStr");
            optionsBuilder.UseSqlServer(connString);



            Debug.Print("connString: " + connString);


            using (AppDbContext context = new AppDbContext(optionsBuilder.Options))
            {

                return context.UProvider.Where(provider => provider.codigo == providerCode && provider.grupo==grupo).ToList();
            }
        }

        public List<UProvider> getProviderDataPHC(decimal providerCode)
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

                return context.UProvider.Where(provider => provider.codigo == providerCode).ToList();
            }
        }

        public string getProviderByKey(List<UProvider> providerData, string key)
        {
            return providerData.Where(providerData => providerData.chave == key).FirstOrDefault()?.valor;
           
        }
    }
}
