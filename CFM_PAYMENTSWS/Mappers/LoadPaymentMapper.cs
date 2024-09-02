using AutoMapper;
using CFM_PAYMENTSWS.DTOs;
using CFM_PAYMENTSWS.Providers.Nedbank.DTOs;

namespace CFM_PAYMENTSWS.Mappers
{
    public class LoadPaymentMapper
    {
        //public ResponseDTO mapMpesaTransaction(NedbankResponseDTO response)
        //{

        //    var config = new MapperConfiguration(cfg =>
        //        cfg.CreateMap<NedbankResponseDTO, ResponseDTO>()
        //        .ForPath(dest => dest.response, act => act.MapFrom(src => new ResponseCodesDTO((response.code == "INS-0" ? "0000" : "0002"), response.IsSuccessfully ? "Success" : "Error")))
        //        .ForPath(dest => dest.Data, act => act.MapFrom(src => response.ToString())
        //        ));
        //    var mapper = new Mapper(config);
        //    var responseDTO = mapper.Map<ResponseDTO>(response);

        //    return responseDTO;
        //}
    }
}
