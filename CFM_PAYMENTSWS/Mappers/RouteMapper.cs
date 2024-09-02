using MPesa;
using CFM_PAYMENTSWS.DTOs;
using CFM_PAYMENTSWS.Providers.Nedbank.DTOs;

namespace CFM_PAYMENTSWS.Mappers
{
    public class RouteMapper
    {
        public ResponseDTO mapLoadPaymentResponse(decimal providercode, object response)
        {
            switch (providercode)
            {
                case 101:

                    var mpesaMapper=new MpesaMapper();
                    Response mpesaResponse = (Response)response;
                    return mpesaMapper.mapMpesaTransaction(mpesaResponse);

                case 105:
                    var nedBankMapper = new NedbankMapper();
                    NedbankResponseDTO  nedbankResponse= (NedbankResponseDTO)response;
                    return nedBankMapper.mapNedbankLoadPayments(nedbankResponse);

                default:
                     throw new Exception("INVALID_PROVIDER_CODE_ON_MAP_PROVIDER_RESPONSE");

            }
        }
        public CheckPaymentReportResponseDTO mapProviderChekPaymentResponse(decimal providercode, object response)
        {
            switch (providercode)
            {
              
                case 105:

                    var nedBankMapper = new NedbankMapper();

                    //CheckPaymentReportResponseDTO mapNedbankCheckPayment(NedBankCheckPaymentReportResponseDTO nedbankResponseDTO)
                    NedBankCheckPaymentReportResponseDTO nedbankResponse = (NedBankCheckPaymentReportResponseDTO)response;
                    return nedBankMapper.mapNedbankCheckPayment(nedbankResponse);
                default:
                    throw new Exception("INVALID_PROVIDER_CODE_ON_MAP_PROVIDER_QUERY_RESPONSE");
            }
        }
    }
}
