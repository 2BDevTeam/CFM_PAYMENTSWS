using AutoMapper;
using MPesa;
using CFM_PAYMENTSWS.DTOs;
using System.Diagnostics;

namespace CFM_PAYMENTSWS.Mappers
{
    public class MpesaMapper
    {
        public ResponseDTO mapMpesaTransaction(Response response)
        {

            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<Response, ResponseDTO>()
                .ForPath(dest => dest.response, act => act.MapFrom(src => new ResponseCodesDTO((response.Code== "INS-0" ? "0000":"0002"), response.IsSuccessfully?"Success":"Error")))
                .ForPath(dest => dest.Data, act => act.MapFrom(src => response.ToString())
                ));
            var mapper = new Mapper(config);
            var responseDTO = mapper.Map<ResponseDTO>(response);

            return responseDTO;
        }
        public ResponseDTO mapMpesaQueryTransaction(Response response)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Response, ResponseDTO>()
                    .ForPath(dest => dest.response, act => act.MapFrom(src => getQueryResponse(src).response))
                    .ForPath(dest => dest.Data, act => act.MapFrom(src => src.ToString()));
            });
            var mapper = new Mapper(config);
            var responseDTO = mapper.Map<ResponseDTO>(response);

            return responseDTO;
        }

        public ResponseDTO getQueryResponse(Response response)
        {
            var responseCodes= new ResponseCodesDTO("0000", "Success");

            Debug.Print($" RESPOSTA QUERY TRANSACT {response.ToString()}");
           
            if (response.Code == "INS-20")
            {
                responseCodes = new ResponseCodesDTO("0002", "Error");
                return new ResponseDTO(responseCodes, response.ToString(), null);

            }
            if (!response.IsSuccessfully)
            {
                 responseCodes = new ResponseCodesDTO("00077", "Error");
               return new ResponseDTO(responseCodes, response.ToString(), null);
               
            }

            if(response.TransactionStatus == "Completed")
            {
                 responseCodes = new ResponseCodesDTO("0000", "Success");
               return new ResponseDTO(responseCodes, response.ToString(), null);
            }

             responseCodes = new ResponseCodesDTO("0002", "Error");
            return new ResponseDTO(responseCodes, response.ToString(), null);

        }
    }
}
