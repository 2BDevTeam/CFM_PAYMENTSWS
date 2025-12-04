using MPesa;
using CFM_PAYMENTSWS.DTOs;
using CFM_PAYMENTSWS.Providers.Nedbank.DTOs;
using CFM_PAYMENTSWS.Providers.BCI.DTOs;
using CFM_PAYMENTSWS.Providers.Bim.DTOs;
using CFM_PAYMENTSWS.Providers.FCB.DTOs;
using CFM_PAYMENTSWS.Providers.Moza.DTOs;

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

                case 106:
                    var bciMapper = new BCIMapper();
                    BCIResponseDTO bciResponse = (BCIResponseDTO)response;
                    return bciMapper.mapBCILoadPayments(bciResponse);

                case 107:
                    var bimMapper = new BimMapper();
                    BimResponseDTO bimResponse = (BimResponseDTO)response;
                    return bimMapper.mapBimLoadPayments(bimResponse);

                case 108:
                    var fcbMapper = new FcbMapper();
                    FcbResponseDTO fcbResponse = (FcbResponseDTO)response;
                    return fcbMapper.MapFcbLoadPayments(fcbResponse);

                case 109:
                    var mozaMapper = new MozaMapper();
                    MozaPaymentResponseDTO mozaResponse = (MozaPaymentResponseDTO)response;
                    return mozaMapper.MapMozaLoadPayments(mozaResponse);

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
