using MPesa;
using CFM_PAYMENTSWS.Domains.Interface;
using CFM_PAYMENTSWS.Domains.Models;
using CFM_PAYMENTSWS.DTOs;
using CFM_PAYMENTSWS.Helper;
using CFM_PAYMENTSWS.Mappers;
using CFM_PAYMENTSWS.Providers.Nedbank.DTOs;
using CFM_PAYMENTSWS.Providers.Nedbank.Repository;
using System.Diagnostics;

namespace CFM_PAYMENTSWS.Providers
{
    public class ProviderRoute
    {
        private readonly ProviderHelper providerHelper = new ProviderHelper();
        private readonly RouteMapper routeMapper = new RouteMapper();
        //private readonly IPaymentRepository _iPaymentRespository;
        //private readonly U2BPaymentsHs FF = new U2BPaymentsHs();

       

        public async Task<ResponseDTO> loadPaymentRoute(PaymentsQueue payment)
        {
            Debug.Print("Entra no LoadPayments");
            //Lista dos dados do provedor da tabela u_provider. Dados das keys e o código do provider.....
            List<UProvider> providerData = providerHelper.getProviderData(payment.canal);

            //Caso não encontre nenhum provedor...
            if (!providerData.Any())
            {
                return new ResponseDTO(new ResponseCodesDTO("000404", "Não foram encontrados os dados do provedor de pagamento"), "", payment.ToString());

            }

            Debug.Print($"Service providercode DO PROVERDOR {providerData.ToString()} ");
            switch (payment.canal)
            {
                case 105:
                    Debug.Print("Chamar repositorio do Nedbank e metodo loadpayments enviando paymentsQueue");
                    //Chamar repositorio do Nedbank e metodo loadpayments enviando paymentsQueue
                    NedbankRepository nedbankRepository = new NedbankRepository();
                    NedbankResponseDTO nedbankResponseDTO = nedbankRepository.loadPayments(payment.payment);
                    //Debug.Print("DadosHistorico" + nedbankResponseDTO.ToString() + "///" + payment.payment.ToString());
                    

                    
                    var nedbankmappedResponse = routeMapper.mapLoadPaymentResponse(105, nedbankResponseDTO);
                    return nedbankmappedResponse;

                default:
                    throw new Exception("INVALID_PROVIDER_CODE_ON_MAP_PROVIDER_ROUTE");
            }


        }
          
        public async Task<CheckPaymentReportResponseDTO> checkPaymentsRoute(PaymentsQueue payment)
        {

            
            List<UProvider> providerData = providerHelper.getProviderData(payment.canal);
            if (!providerData.Any())
            {
                return new CheckPaymentReportResponseDTO { batchResponse= new ResponseDTO(new ResponseCodesDTO("000404", "Não foram encontrados os dados do provedor de pagamento"), "", payment.ToString()) };
               
            }
            switch (payment.canal)
            {
                
                case 105:

                    //Chamar repositorio do Nedbank e metodo validatePayments
                    NedbankRepository nedbankRepository = new NedbankRepository();
                    NedBankCheckPaymentReportResponseDTO nedbankResponseDTO = nedbankRepository.validatePayments(payment.payment.BatchId);

                    var nedbankmappedResponse = routeMapper.mapProviderChekPaymentResponse(105, nedbankResponseDTO);
                    return nedbankmappedResponse;

                default:
                    throw new Exception("INVALID_PROVIDER_CODE_ON_MAP_PROVIDER_ROUTE");
            }
        }


       

        }
    }

