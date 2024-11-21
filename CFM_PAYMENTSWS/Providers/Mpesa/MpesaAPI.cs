using MPesa;
using Newtonsoft.Json;
using CFM_PAYMENTSWS.Domains.Models;
using CFM_PAYMENTSWS.DTOs;
using CFM_PAYMENTSWS.Helper;
using CFM_PAYMENTSWS.Mappers;
using System.Diagnostics;
using Environment = MPesa.Environment;

namespace CFM_PAYMENTSWS.Providers.Mpesa
{
    public class MpesaAPI
    {
        private readonly ProviderHelper providerHelper = new ProviderHelper();
        private readonly RouteMapper routeMapper = new RouteMapper();
        public async Task<ResponseDTO> B2CpaymentProviderRoute(U2bPaymentsQueue u2BPaymentsQueue)
        {
            List<UProvider> providerData = providerHelper.getProviderDataPHC(u2BPaymentsQueue.Canal);

            if (!providerData.Any())
            {
                return new ResponseDTO(new ResponseCodesDTO("000404", "Não foram encontrados os dados do provedor de pagamento"), "", u2BPaymentsQueue.ToString());

            }

            Debug.Print($"Service providercode DO PROVERDOR {providerData.ToString()} ");
            switch (u2BPaymentsQueue.Canal)
            {
                case 101:

                    var apikeyData = providerData.Where(providerData => providerData.chave == "apikey").FirstOrDefault();
                    var publiKeyData = providerData.Where(providerData => providerData.chave == "publickey").FirstOrDefault();
                    var serviceProviderCodeData = providerData.Where(providerData => providerData.chave == "serviceProviderCode").FirstOrDefault();
                    Debug.Print($"DADOS DO PROVERDOR apikeyData: {apikeyData.valor} ");
                    var client = new Client.Builder()
                                     .ApiKey(apikeyData.valor.Trim())
                                     .PublicKey(publiKeyData.valor.Trim())
                                     .ServiceProviderCode(serviceProviderCodeData.valor.Trim())
                                     .InitiatorIdentifier("SJGW67fK")
                                     .Environment(Environment.Development)
                                     .SecurityCredential("Mpesa2019")
                                     .Build();

                    var request = new MPesa.Request.Builder()
                                      .Amount(Convert.ToDouble(u2BPaymentsQueue.Valor))
                                      .To(u2BPaymentsQueue.Destino)
                                      .Reference(u2BPaymentsQueue.TransactionId)
                                      .Transaction(u2BPaymentsQueue.TransactionId)
                                      .Build();

                    var response = await client.Send(request);

                    var mappedResponse = routeMapper.mapLoadPaymentResponse(101, response);
                    return mappedResponse;


                default:
                    throw new Exception("INVALID_PROVIDER_CODE_ON_MAP_PROVIDER_ROUTE");
            }



        }

        public async Task<ResponseDTO> C2BPaymentProviderRoute(PaymentDetailsDTO u2BPaymentsQueue)
        {
            List<UProvider> providerData = providerHelper.getProviderDataPHC(u2BPaymentsQueue.Canal);

            if (!providerData.Any())
            {
                return new ResponseDTO(new ResponseCodesDTO("000404", "Não foram encontrados os dados do provedor de pagamento"), "", u2BPaymentsQueue.ToString());

            }

            Debug.Print($"Service providercode DO PROVERDOR {JsonConvert.SerializeObject (providerData)} ");
            switch (u2BPaymentsQueue.Canal)
            {
                case 101:

                    var apikeyData = providerData.Where(providerData => providerData.chave == "apikey").FirstOrDefault();
                    var publiKeyData = providerData.Where(providerData => providerData.chave == "publickey").FirstOrDefault();
                    var serviceProviderCodeData = providerData.Where(providerData => providerData.chave == "serviceProviderCode").FirstOrDefault();
                    Debug.Print($"DADOS DO PROVERDOR apikeyData: {apikeyData.valor} ");
                    var client = new Client.Builder()
                                     .ApiKey(apikeyData.valor.Trim())
                                     .PublicKey(publiKeyData.valor.Trim())
                                     .ServiceProviderCode(serviceProviderCodeData.valor.Trim())
                                     .InitiatorIdentifier("SJGW67fK")
                                     .Environment(Environment.Development)
                                     .SecurityCredential("Mpesa2019")
                                     .Build();

                    var request = new  MPesa.Request.Builder()
                                      .Amount(Convert.ToDouble(u2BPaymentsQueue.Valor))
                                      .From(u2BPaymentsQueue.MSISDN)
                                      .Reference(u2BPaymentsQueue.Referencia)
                                      .Transaction(u2BPaymentsQueue.Referencia)
                                      .Build();

                    var response = await client.Receive(request);

                    var mappedResponse = routeMapper.mapLoadPaymentResponse(101, response);
                    return mappedResponse;


                default:
                    throw new Exception("INVALID_PROVIDER_CODE_ON_MAP_PROVIDER_ROUTE");
            }



        }






        public async Task<ResponseDTO> PaymentQueryProviderRoute(object payment)
        {
            string referencia = "";
            decimal canal=0;
            
            if (payment is U2bPaymentsQueue u2BPaymentsQueue)
            {
                canal = u2BPaymentsQueue.Canal;
                referencia = u2BPaymentsQueue.TransactionId;
            }
            else if (payment is PaymentDetailsDTO paymentDetailsDTO)
            {
                canal = paymentDetailsDTO.Canal;
                referencia = paymentDetailsDTO.Referencia;
            }

            List<UProvider> providerData = providerHelper.getProviderDataPHC(canal);

            if (!providerData.Any())
            {
                return new ResponseDTO(new ResponseCodesDTO("000404", "Não foram encontrados os dados do provedor de pagamento"), "", canal);
                //return new ResponseDTO(new ResponseCodesDTO("000404", "Não foram encontrados os dados do provedor de pagamento"), "", paymentDetails.ToString());
            }

            switch (canal)
            {
                case 101:

                    var apikeyData = providerData.Where(providerData => providerData.chave == "apikey").FirstOrDefault();
                    var publiKeyData = providerData.Where(providerData => providerData.chave == "publickey").FirstOrDefault();
                    var serviceProviderCodeData = providerData.Where(providerData => providerData.chave == "serviceProviderCode").FirstOrDefault();
                    var client = new Client.Builder()
                                     .ApiKey(apikeyData.valor)
                                     .PublicKey(publiKeyData.valor)
                                     .ServiceProviderCode(serviceProviderCodeData.valor)
                                     .InitiatorIdentifier("SJGW67fK")
                                     .Environment(Environment.Development)
                                     .SecurityCredential("Mpesa2019")
                                     .Build();
                    //Debug.Print($"DADOS DO PAGAMENTO {JsonConvert.SerializeObject(paymentDetails)}");
                    var queryRequest = new MPesa.Request.Builder()
                                        .Reference(referencia)
                                        .Subject(referencia)
                                        .Build();

                    var response = await client.Query(queryRequest);
                    Debug.Print($"Query {response.ToString()}");

                    var mappedResponse = routeMapper.mapLoadPaymentResponse(101, response);
                    return mappedResponse;

                default:
                    throw new Exception("INVALID_PROVIDER_CODE_ON_MAP_PROVIDER_ROUTE");
            }
        }

       
    }
}
