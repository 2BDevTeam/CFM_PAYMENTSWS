using AutoMapper;
using CFM_PAYMENTSWS.Domains.Contracts;
using CFM_PAYMENTSWS.DTOs;
using CFM_PAYMENTSWS.Providers.Moza.DTOs;

namespace CFM_PAYMENTSWS.Mappers
{
    public class MozaMapper
    {
        public ResponseDTO MapMozaLoadPayments(MozaPaymentResponseDTO mozaResponse)
        {
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<MozaPaymentResponseDTO, ResponseDTO>()
                    .ForPath(dest => dest.response, act => act.MapFrom(src => GetStatusCode(src)))
                    .ForPath(dest => dest.Data, act => act.MapFrom(src => src.ToString()))
            );

            var mapper = new Mapper(config);
            var responseDTO = mapper.Map<ResponseDTO>(mozaResponse);

            return responseDTO;
        }

        private ResponseCodesDTO GetStatusCode(MozaPaymentResponseDTO mozaResponse)
        {
            var statusCode = mozaResponse.StatusCode ?? "0011";
            var statusDescription = mozaResponse.StatusDescription ?? mozaResponse.Message ?? "Sem resposta";

            if (statusCode == "0000")
                return WebTransactionCodes.SUCCESS;

            if (statusCode == "1001")
                return WebTransactionCodes.PENDINGBATCH;

            if (int.TryParse(statusCode, out var number))
            {
                if (number is >= 400 and <= 499)
                    return new ResponseCodesDTO("0007", statusDescription);

                if (number is >= 500 and <= 599)
                    return new ResponseCodesDTO("0010", statusDescription);
            }

            return new ResponseCodesDTO("0011", statusDescription);
        }
    }
}
