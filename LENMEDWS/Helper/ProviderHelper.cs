using Microsoft.EntityFrameworkCore;
using LENMEDWS.Domains.Models;
using LENMEDWS.Persistence.Contexts;

namespace LENMEDWS.Helper
{
    public class ProviderHelper
    {
        public List<UProvider> getProviderData(decimal providerCode)
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

                return context.UProvider.Where(provider => provider.codigo == providerCode).ToList();
            }
        }
        public List<UProvider> getProviderByGroup(decimal providerCode,string grupo)
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

                return context.UProvider.Where(provider => provider.codigo == providerCode &&provider.grupo==grupo).ToList();
            }
        }

        public string getProviderByKey(List<UProvider> providerData, string key)
        {
            return providerData.Where(providerData => providerData.chave == key).FirstOrDefault()?.valor;
           
        }
    }
}
