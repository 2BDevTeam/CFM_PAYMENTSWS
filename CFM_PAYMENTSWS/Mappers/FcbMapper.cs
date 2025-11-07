using AutoMapper;
using CFM_PAYMENTSWS.Domains.Contracts;
using CFM_PAYMENTSWS.DTOs;
using CFM_PAYMENTSWS.Providers.FCB.DTOs;

namespace CFM_PAYMENTSWS.Mappers
{
    public class FcbMapper
    {
        public ResponseDTO MapFcbLoadPayments(FcbResponseDTO fcbResponse)
        {
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<FcbResponseDTO, ResponseDTO>()
                    .ForPath(dest => dest.response, act => act.MapFrom(src => GetStatusCode(src)))
                    .ForPath(dest => dest.Data, act => act.MapFrom(_ => fcbResponse.ToString()))
            );

            var mapper = new Mapper(config);
            return mapper.Map<ResponseDTO>(fcbResponse);
        }

        private static ResponseCodesDTO GetStatusCode(FcbResponseDTO response)
        {
            if (string.Equals(response.StatusCode, "0000", StringComparison.OrdinalIgnoreCase))
            {
                return WebTransactionCodes.SUCCESS;
            }

            if (string.IsNullOrWhiteSpace(response.StatusCode))
            {
                return new ResponseCodesDTO("0007", string.IsNullOrWhiteSpace(response.StatusDescription)
                    ? "Resposta inválida do serviço FCB"
                    : response.StatusDescription);
            }

            return new ResponseCodesDTO(response.StatusCode, string.IsNullOrWhiteSpace(response.StatusDescription)
                ? "Erro ao submeter pagamento para o FCB"
                : response.StatusDescription);
        }
    }
}
