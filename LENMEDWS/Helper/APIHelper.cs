using LENMEDWS.DTOs;

namespace LENMEDWS.Helper
{
    public class APIHelper
    {



        public API getApiEntity(string entity,string operationCode)
        {
            var configuration = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile($"appsettings.json");

            var config = configuration.Build();
            API[] configuracoes = config.GetSection("APIS").Get<API[]>();

            var apiEntityData = configuracoes.Where(apiEntity => apiEntity.entity == entity).FirstOrDefault();

            if(apiEntityData != null)
            {

                var endpointData = apiEntityData.endpoints.Where(endpoint => endpoint.operationCode == operationCode);

                if(endpointData != null)
                {
                    apiEntityData.status = "1";
                    return apiEntityData;

                }

                return new API {status="0", message = "Não foi encontrado o endpoint com o código indicado para a respectiva entidade." };
              

            }

            return new API { status = "0", message = "Os dados da API da entidade indicada não foram encontrados" };
        }
    }
}
