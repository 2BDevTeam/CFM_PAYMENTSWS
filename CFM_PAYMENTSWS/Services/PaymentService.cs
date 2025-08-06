//using Microsoft.Data.SqlClient.Server;
using CFM_PAYMENTSWS.Domains.Interface;
using CFM_PAYMENTSWS.Domains.Interfaces;
using CFM_PAYMENTSWS.Domains.Models;
using CFM_PAYMENTSWS.DTOs;
using CFM_PAYMENTSWS.Extensions;
using CFM_PAYMENTSWS.Helper;
using CFM_PAYMENTSWS.Persistence.Contexts;
using CFM_PAYMENTSWS.Providers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Globalization;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore.Storage;
using CFM_PAYMENTSWS.Mappers;
using CFM_PAYMENTSWS.Providers.Nedbank.DTOs;
using CFM_PAYMENTSWS.Providers.Nedbank.Repository;
using System.Threading.Tasks.Dataflow;
using MPesa;
using CFM_PAYMENTSWS.Providers.Mpesa;
using CFM_PAYMENTSWS.Providers.BCI.Repository;
using CFM_PAYMENTSWS.Providers.BCI.DTOs;
using Hangfire.States;
using CFM_PAYMENTSWS.Providers.Bim.Repository;
using CFM_PAYMENTSWS.Providers.Bim.DTOs;

namespace CFM_PAYMENTSWS.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly APIHelper apiHelper = new APIHelper();
        private readonly LogHelper logHelper = new LogHelper();
        private readonly ProviderHelper providerHelper = new ProviderHelper();
        private readonly RouteMapper routeMapper = new RouteMapper();
        private readonly MpesaAPI mpesaApi = new MpesaAPI();

        private readonly TimeSpan _lockTimeout = TimeSpan.FromMinutes(5);

        private readonly IPHCRepository<PHCDbContext> _phcRepository;
        private readonly IGenericRepository<PHCDbContext> _genericPHCRepository;
        private readonly IPaymentRepository<AppDbContext> _paymentRespository;
        private readonly IGenericRepository<AppDbContext> _genericPaymentRepository;

        public PaymentService(
            IPHCRepository<PHCDbContext> phcRepository, IGenericRepository<AppDbContext> genericPaymentRepository,
            IPaymentRepository<AppDbContext> paymentRepository, IGenericRepository<PHCDbContext> genericE14Repository)
        {
            _phcRepository = phcRepository;
            _genericPaymentRepository = genericPaymentRepository;
            _genericPHCRepository = genericE14Repository;
            _paymentRespository = paymentRepository;
        }


        public PaymentService()
        {

        }


        public async Task<bool> VerificarJobActivos(string lockKey)
        {
            var jobLock = _phcRepository.GetJobLocks(lockKey);

            if (jobLock != null && jobLock.IsRunning)
            {
                Debug.Print("O job já está em execução");
                return true;
            }

            if (jobLock == null)
            {
                jobLock = new JobLocks { JobId = lockKey, IsRunning = true };
                _genericPHCRepository.Add(jobLock);
            }
            else
            {
                jobLock.IsRunning = true;
            }
            _genericPHCRepository.SaveChanges();
            return false;
        }

        public void TerminarJob(string lockKey)
        {
            var jobLock = _phcRepository.GetJobLocks(lockKey);

            _genericPHCRepository.Delete(jobLock);
            _genericPHCRepository.SaveChanges();
        }


        public async Task ProcessarPagamentosAsync()
        {
            string lockKey = "processarPagamentos";

            if (await VerificarJobActivos(lockKey))
                return;

            try
            {
                var canaisPagamento = _paymentRespository.GetCanais_UPaymentQueue();
                Debug.Print($"canaisPagamento   {JsonConvert.SerializeObject(canaisPagamento)} ");

                List<U2bPaymentsQueue> pagamentoQueue = new();
                List<PaymentsQueue> pagamentos = new();
                foreach (var canal in canaisPagamento)
                {

                    switch (canal)
                    {
                        case 101:

                            //pagamentoQueue = _paymentRespository.GetPagamentosEmFila("Por enviar", 101);
                            Debug.Print($"Pagamento queue   {JsonConvert.SerializeObject(pagamentoQueue)} ");
                            //await MpesaProcessing(pagamentoQueue);
                            break;

                        case 105:
                            pagamentos = _paymentRespository.GetPagamentQueue("Por enviar", 105);
                            await NedBankProcessing(pagamentos);
                            break;

                        case 106:
                            pagamentos = _paymentRespository.GetPagamentQueue("Por enviar", 106);
                            await BCIProcessing(pagamentos, false);
                            break;

                        case 107:
                            pagamentos = _paymentRespository.GetPagamentQueue("Por enviar", 107);
                            await BimProcessing(pagamentos, false);
                            break;

                        default:
                            break;
                    }

                }


            }
            catch (Exception ex)
            {
                Debug.Print($"FALHA GLOBAL SERVICE EXCPETION {ex.Message.ToString()} INNER EXEPTION {ex.InnerException}");
                var response = new ResponseDTO(new ResponseCodesDTO("0007", "Internal error"), $"MESSAGE :{ex.Message} STACK:{ex.StackTrace} INNER{ex.InnerException}", null);
                logHelper.generateLogJB(response, "processarPagamento" + Guid.NewGuid(), "PaymentService.processarPagamento", ex.Message);
            }
            finally
            {
                TerminarJob(lockKey);
            }

        }


        public async Task VerificarPagamentos()
        {
            string lockKey = "verificarPagamentos";

            if (await VerificarJobActivos(lockKey))
                return;

            try
            {
                var canaisPagamento = _paymentRespository.GetCanais_UPaymentQueue();
                Debug.Print($"canaisPagamento   {JsonConvert.SerializeObject(canaisPagamento)} ");

                List<U2bPaymentsQueue> pagamentoQueue = new();
                List<PaymentsQueue> pagamentos = new();
                foreach (var canal in canaisPagamento)
                {

                    switch (canal)
                    {
                        case 101:

                            break;

                        case 105:
                            //pagamentos = _paymentRespository.GetPagamentQueue("Por enviar", 105);
                            //NedBankProcessing(pagamentos);
                            break;

                        case 106:
                            pagamentos = _paymentRespository.GetPagamentQueue("Por processar", 106);
                            await BCIProcessing(pagamentos, true);
                            break;

                        default:
                            break;
                    }

                }


            }
            catch (Exception ex)
            {
                Debug.Print($"FALHA GLOBAL SERVICE EXCPETION {ex.Message.ToString()} INNER EXEPTION {ex.InnerException}");
                var response = new ResponseDTO(new ResponseCodesDTO("0007", "Internal error"), $"MESSAGE :{ex.Message} STACK:{ex.StackTrace} INNER{ex.InnerException}", null);
                logHelper.generateLogJB(response, "processarPagamento" + Guid.NewGuid(), "PaymentService.processarPagamento", ex.Message);
            }
            finally
            {
                TerminarJob(lockKey);
            }

        }

        public async Task<ResponseDTO> actualizarPagamentos(PaymentCheckedDTO paymentHeader)
        {
            List<PaymentRecordResponseDTO> paymentRecordResponseDTOs = new List<PaymentRecordResponseDTO>();

            string json1 = JsonConvert.SerializeObject(paymentHeader);

            Debug.Print("paymentRecordResponseDTOs" + json1);

            logHelper.generateLogJB(new ResponseDTO(), paymentHeader.BatchId, "PaymentService.actualizarPagamentos", json1);
            //
            if (paymentHeader.BatchId== "DNF25062062160.282000003" || paymentHeader.BatchId == "DNF25062062160.282000002" || paymentHeader.BatchId == "DNF25062440823.442000003")
            {
                return new ResponseDTO(new ResponseCodesDTO("0000", "Pagamento processado com sucesso."), null, null);
            }
            //validarPagamentos
            try
            {
                if (paymentHeader != null)
                {
                    string batchId = paymentHeader.BatchId;
                    Debug.Print("Entrou porque tem pagamentos");

                    //Validar se o batchid existe

                    bool existe = _paymentRespository.verificaBatchId(batchId);

                    if (existe == false)
                    {
                        return new ResponseDTO(new ResponseCodesDTO("0050", "Batchid not found"), null, null);
                    }

                    foreach (var pagamento in paymentHeader.PaymentCheckedRecords)
                    {

                        insere2bHistorico(pagamento.TransactionId.Trim(), paymentHeader.BatchId, paymentHeader.BatchId, paymentHeader.StatusCode, paymentHeader.StatusDescription, pagamento.StatusCode, pagamento.StatusDescription);

                        switch (pagamento.StatusCode)
                        {
                            case "0000" or "3005":
                                actualizarEstadoDoPagamentoByTransactionId("Sucesso", "Pagamento processado com sucesso", paymentHeader, pagamento);
                                break;

                            case "0001":
                                actualizarEstadoDoPagamentoByTransactionId("Recebido", "Atualização recebida", paymentHeader, pagamento);
                                break;

                            case "0010":
                                actualizarEstadoDoPagamentoByTransactionId("Erro", "NIB de crédito inválido", paymentHeader, pagamento);
                                break;

                            case "0011":
                                actualizarEstadoDoPagamentoByTransactionId("Erro", "Montante inválido", paymentHeader, pagamento);
                                break;

                            case "0012":
                                actualizarEstadoDoPagamentoByTransactionId("Erro", "Conta de débito inválida", paymentHeader, pagamento);
                                break;

                            case "0013":
                                actualizarEstadoDoPagamentoByTransactionId("Erro", "Incompatibilidade de moeda", paymentHeader, pagamento);
                                break;

                            case "0014":
                                actualizarEstadoDoPagamentoByTransactionId("Erro", "Data de processamento inválida ou passada", paymentHeader, pagamento);
                                break;

                            case "0015":
                                actualizarEstadoDoPagamentoByTransactionId("Erro", "Moeda inválida", paymentHeader, pagamento);
                                break;

                            case "0016":
                                actualizarEstadoDoPagamentoByTransactionId("Erro", "BatchId já submetido", paymentHeader, pagamento);
                                break;

                            case "0017":
                                actualizarEstadoDoPagamentoByTransactionId("Erro", "Registros vazios", paymentHeader, pagamento);
                                break;

                            case "1000":
                                actualizarEstadoDoPagamentoByTransactionId("Sucesso", "Pagamento processado com sucesso", paymentHeader, pagamento);
                                break;

                            case "1001":
                                actualizarEstadoDoPagamentoByTransactionId("Pendente", "Pagamento pendente", paymentHeader, pagamento);
                                break;

                            case "0019":
                                actualizarEstadoDoPagamentoByTransactionId("Processado", "Pagamento processado ou pendente", paymentHeader, pagamento);
                                break;

                            case "0020":
                                actualizarEstadoDoPagamentoByTransactionId("Erro", "Batch rejeitado", paymentHeader, pagamento);
                                break;

                            case "0050":
                                actualizarEstadoDoPagamentoByTransactionId("Erro", "BatchId não encontrado", paymentHeader, pagamento);
                                break;


                            case "0018" or "0022":
                                actualizarEstadoDoPagamentoByTransactionId("Erro", "Saldo insuficiente", paymentHeader, pagamento);
                                break;

                            case "0021":
                                actualizarEstadoDoPagamentoByTransactionId("Erro", "Conta não movimentável", paymentHeader, pagamento);
                                break;


                            case "0007":
                                actualizarEstadoDoPagamentoByTransactionId("Erro", "Montante inválido", paymentHeader, pagamento);
                                break;

                            default:
                                actualizarEstadoDoPagamentoByTransactionId("Erro",pagamento.StatusDescription, paymentHeader, pagamento);
                                break;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                // Lógica de tratamento de exceção, se necessário.
                Debug.Print("Erro: " + ex.Message);
                return new ResponseDTO(new ResponseCodesDTO("0404", "Erro no processamento."), null, null);
            }

            return new ResponseDTO(new ResponseCodesDTO("0000", "Pagamento processado com sucesso."), null, null);
        }


        async Task BimProcessing(List<PaymentsQueue> pagamentos, bool checkPayments)
        {

            foreach (var pagamento in pagamentos)
            {

                BimAPI bimRepository = new BimAPI();

                Paymentv1_5 paymentv1_5 = apiHelper.ConvertPaymentToV1_5(pagamento.payment);

                BimResponseDTO bimResponseDTO = new BimResponseDTO();

                if (checkPayments)
                {
                    //bciResponseDTO = bimRepository.checkPayments(pagamento.payment.BatchId, pagamento.payment.initgPtyCode ?? "");
                }
                else
                {
                    Debug.Print("BimProcessing");
                    Debug.Print($"{pagamento.payment}");

                    bimResponseDTO = await bimRepository.loadPayments(paymentv1_5);
                }


                ResponseDTO bimResponse = routeMapper.mapLoadPaymentResponse(107, bimResponseDTO);

                Debug.Print("Resposta do Load");
                Debug.Print(bimResponse.ToString());

                insere2bHistorico("", pagamento.payment.BatchId, pagamento.payment.BatchId, bimResponse.response.cod, bimResponse.response.codDesc, "", "");

                Debug.Print("insere2bHistorico");

                switch (bimResponse.response.cod)
                {

                    case "0" or "0000" or "0011":
                        actualizarEstadoDoPagamento(pagamento, "Por processar", "Pagamento enviado por processar");
                        Debug.Print("Teste Por processar" + bimResponse.response.codDesc);
                        break;

                    case "2028":
                        actualizarEstadoDoPagamento(pagamento, "Por corrigir", bimResponse.response.codDesc);
                        Debug.Print("Teste Por Corrigir" + bimResponse.response.codDesc);
                        break;
                    default:
                        //actualizarEstadoDoPagamento(pagamento, "Por corrigir", bimResponse.response.codDesc);
                        //Debug.Print("Teste HS3" + bimResponse.response.codDesc);
                        break;

                }


                logHelper.generateLogJB(bimResponse, pagamento.payment.BatchId, "PaymentService.processarPagamento - Bim", pagamento.payment);

            }

        }


        async Task BCIProcessing(List<PaymentsQueue> pagamentos, bool checkPayments)
        {

            foreach (var pagamento in pagamentos)
            {

                insere2bHistorico("", pagamento.payment.BatchId, pagamento.payment.BatchId, "bciResponse.response.cod", "bciResponse.response.codDesc", "", "");
                BCIAPI bciRepository = new BCIAPI();

                PaymentCamelCase paymentCamel = apiHelper.ConvertPaymentToCamelCase(pagamento.payment);

                BCIResponseDTO bciResponseDTO = new BCIResponseDTO();

                if (checkPayments)
                {
                    //bciResponseDTO = bciRepository.checkPayments(pagamento.payment.BatchId, pagamento.payment.initgPtyCode ?? "");
                }
                else
                {
                    bciResponseDTO = bciRepository.loadPayments(paymentCamel);
                }


                ResponseDTO bciResponse = routeMapper.mapLoadPaymentResponse(106, bciResponseDTO);


                Debug.Print("Resposta do Load");
                Debug.Print(bciResponse.ToString());

                insere2bHistorico("", pagamento.payment.BatchId, pagamento.payment.BatchId, bciResponse.response.cod, bciResponse.response.codDesc, "", "");


                switch (bciResponse.response.cod)
                {
                    case "2028":
                        actualizarEstadoDoPagamento(pagamento, "Por corrigir", bciResponse.response.codDesc);
                        Debug.Print("Teste Por Corrigir" + bciResponse.response.codDesc);
                        break;

                    case "0011" or "3002" or "0000" or "1001":
                        actualizarEstadoDoPagamento(pagamento, "Por processar", "Pagamento enviado por processar");
                        Debug.Print("Teste Por processar" + bciResponse.response.codDesc);
                        break;

                    default:
                        Debug.Print("Teste HS3" + bciResponse.response.codDesc);
                        break;

                }


                logHelper.generateLogJB(bciResponse, pagamento.payment.BatchId, "PaymentService.processarPagamento - BCI", paymentCamel);

            }

        }

        async Task NedBankProcessing(List<PaymentsQueue> pagamentos)
        {

            foreach (var pagamento in pagamentos)
            {

                NedbankAPI nedbankRepository = new NedbankAPI();
                NedbankResponseDTO nedbankResponseDTO = nedbankRepository.loadPayments(pagamento.payment);

                ResponseDTO nedbankResponse = routeMapper.mapLoadPaymentResponse(105, nedbankResponseDTO);

                Debug.Print("Resposta do Load");
                Debug.Print(nedbankResponse.ToString());

                insere2bHistorico("", pagamento.payment.BatchId, pagamento.payment.BatchId, nedbankResponse.response.cod, nedbankResponse.response.codDesc, "", "");

                switch (nedbankResponse.response.cod)
                {
                    case "0011":
                        actualizarEstadoDoPagamento(pagamento, "Por corrigir", nedbankResponse.response.codDesc);
                        Debug.Print("Teste Por Corrigir" + nedbankResponse.response.codDesc);
                        break;

                    case "0000":
                        actualizarEstadoDoPagamento(pagamento, "Por processar", "Pagamento enviado por processar");
                        Debug.Print("Teste Por processar" + nedbankResponse.response.codDesc);
                        break;

                    case "0010":
                        break;

                    case "0007":
                        Debug.Print("Teste HS2" + nedbankResponse.response.codDesc);
                        break;

                    default:
                        Debug.Print("Teste HS3" + nedbankResponse.response.codDesc);
                        break;

                }
                logHelper.generateLogJB(nedbankResponse, pagamento.payment.BatchId, "PaymentService.processarPagamento - nedbank", pagamento.payment);

            }

        }

        async Task MpesaProcessing(List<U2bPaymentsQueue> pagamentos)
        {
            foreach (var pagamento in pagamentos)
            {
                try
                {
                    Debug.Print($" PROCESSANDO O PAGAMENTO {pagamento.TransactionId}");
                    var estadoDoPagamento = await mpesaApi.PaymentQueryProviderRoute(pagamento);
                    var response = new ResponseDTO(estadoDoPagamento.response, estadoDoPagamento.Data, pagamento.ToString());


                    //var response = await providerRoute.paymentProviderRoute(pagamento);
                    //logHelper.generateLogJB(response, pagamento.transactionId, "PaymentService.processarPagamento");
                    Debug.Print($"ESTADO DO PAGAMENTO {response.ToString()}");
                    //_iPaymentRespository.actualizarEstadoDoPagamento(pagamento, response);

                    switch (estadoDoPagamento.response.cod)
                    {
                        case "00077":
                            response = await mpesaApi.B2CpaymentProviderRoute(pagamento);
                            Debug.Print($"RESPOSTA DO PAGAMENTO 00077 {response.ToString()}");
                            logHelper.generateLogJB_PHC(response, pagamento.TransactionId, "PaymentService.processarPagamento");
                            _paymentRespository.actualizarEstadoDoPagamento(pagamento, response);
                            actualizarEstadoDoPagamentoByTransactionId("Sucesso", "Pagamento efectuado com sucesso.", pagamento);

                            break;


                        case "0000":
                            Debug.Print($"Pagamento Existente");
                            response = new ResponseDTO(new ResponseCodesDTO("0000", "Success"), estadoDoPagamento.Data, pagamento.ToString());
                            logHelper.generateLogJB_PHC(response, pagamento.TransactionId, "PaymentService.processarPagamento");
                            _paymentRespository.actualizarEstadoDoPagamento(pagamento, response);
                            actualizarEstadoDoPagamentoByTransactionId("Sucesso", "Pagamento efectuado com sucesso.", pagamento);

                            break;

                        case "0002":

                            response = await mpesaApi.B2CpaymentProviderRoute(pagamento);
                            Debug.Print($"RESPOSTA DO PAGAMENTO 0002 {response.ToString()}");
                            logHelper.generateLogJB_PHC(response, pagamento.TransactionId, "PaymentService.processarPagamento");
                            _paymentRespository.actualizarEstadoDoPagamento(pagamento, response);
                            actualizarEstadoDoPagamentoByTransactionId("Sucesso", "Pagamento efectuado com sucesso.", pagamento);

                            break;

                    }


                }

                catch (Exception ex)
                {
                    Debug.Print($" ERRO ao processar o pagamento com transactionId {pagamento.TransactionId.ToString()}   MESSAGE :{ex.Message} STACK:{ex.StackTrace} INNER{ex.InnerException}");

                    var response = new ResponseDTO(new ResponseCodesDTO("0007", "Internal error"), $" ERRO ao processar o pagamento com transactionId {pagamento.TransactionId.ToString()}   MESSAGE :{ex.Message} STACK:{ex.StackTrace} INNER{ex.InnerException}", null);

                    logHelper.generateLogJB_PHC(response, pagamento.TransactionId, "PaymentService.processarPagamento");


                }
            }


        }



        public async Task<RespostaDTO> ProcessarPagamentos(PaymentDetailsDTO pagamento)
        {
            //decimal responseID = logHelper.generateResponseID().Value;
            decimal responseID = 0;
            RespostaDTO respostaDTO = new();

            try
            {

                List<UProvider> providerData = _phcRepository.GetUProvider(pagamento.Canal);
                var serviceProviderCodeData = providerData.Where(providerData => providerData.chave == "serviceProviderCode").FirstOrDefault();

                U2bPayments u2BPayments = new U2bPayments
                {
                    U2bPaymentsstamp = 25.UseThisSizeForStamp(),
                    Oristamp = pagamento.Oristamp,
                    Tabela = pagamento.Tabela,
                    Valor = pagamento.Valor,
                    Moeda = "MZN",
                    Canal = (int)pagamento.Canal,
                    Transactionid = pagamento.Referencia,
                    Origem = pagamento.MSISDN,
                    Destino = serviceProviderCodeData.valor,
                    //Tipo = "Carteira Móvel",
                    Estado = "Por Enviar",
                    Descricao = "Pagamento por enviar",

                    Ousrdata = DateTime.Now.Date,
                    Ousrhora = DateTime.Now.ToString("HH:mm"),
                    Ousrinis = "2bPayments",
                };

                Debug.Print($"U2bPayments    {JsonConvert.SerializeObject(u2BPayments)}");

                _genericPHCRepository.Add(u2BPayments);
                _genericPHCRepository.SaveChanges();


                Debug.Print($" PROCESSANDO O PAGAMENTO {pagamento.Referencia}");
                var estadoDoPagamento = await mpesaApi.PaymentQueryProviderRoute(pagamento);
                var response = new ResponseDTO(estadoDoPagamento.response, estadoDoPagamento.Data, pagamento.ToString());


                //var response = await providerRoute.paymentProviderRoute(pagamento);
                //logHelper.generateLogJB(response, pagamento.transactionId, "PaymentService.processarPagamento");
                Debug.Print($"ESTADO DO PAGAMENTO {response}");
                //_iPaymentRespository.actualizarEstadoDoPagamento(pagamento, response);

                switch (estadoDoPagamento.response.cod)
                {
                    case "00077":
                        response = await mpesaApi.C2BPaymentProviderRoute(pagamento);
                        Debug.Print($"RESPOSTA DO PAGAMENTO 00077 {(response.Data as Response)?.Description}");
                        logHelper.generateLogJB_PHC(response, pagamento.Referencia, "PaymentService.processarPagamento");

                        //_iPaymentRespository.actualizarEstadoDoPagamento(pagamento, response);
                        break;


                    case "0000":
                    case "000":
                        Debug.Print($"Pagamento Existente  {(response.Data as Response)?.Description} .");
                        response = new ResponseDTO(new ResponseCodesDTO("0000", "Success"), estadoDoPagamento.Data, pagamento.ToString());
                        logHelper.generateLogJB_PHC(response, pagamento.Referencia, "PaymentService.processarPagamento");

                        //_iPaymentRespository.actualizarEstadoDoPagamento(pagamento, response);
                        break;

                    case "0002":

                        response = await mpesaApi.C2BPaymentProviderRoute(pagamento);
                        Debug.Print($"RESPOSTA DO PAGAMENTO  0002  {response}");
                        logHelper.generateLogJB_PHC(response, pagamento.Referencia, "PaymentService.processarPagamento");

                        //_iPaymentRespository.actualizarEstadoDoPagamento(pagamento, response);
                        break;

                }

                //var response = new ResponseDTO(new ResponseCodesDTO("0000", "Success", responseID), null, null);

                respostaDTO = GerarResposta((response.Data as Response), responseID);

                u2BPayments.Estado = respostaDTO.Estado;
                u2BPayments.Descricao = respostaDTO.Descricao;
                _genericPHCRepository.SaveChanges();


                return respostaDTO;
            }
            catch (Exception ex)
            {
                var errorDTO = new ErrorDTO { message = ex?.Message, stack = ex?.StackTrace?.ToString(), inner = ex?.InnerException?.ToString() + "  " };
                Debug.Print($"GetGLTransactions ERROR {errorDTO}");
                var finalResponse = new ResponseDTO(new ResponseCodesDTO("0007", "Error", logHelper.generateResponseID()), errorDTO.ToString(), null);
                //logHelper.generateResponseLogJB(finalResponse, logHelper.generateResponseID().ToString(), "GetGLTransactions", errorDTO?.ToString());
                logHelper.generateLogJB_PHC(finalResponse, logHelper.generateResponseID().ToString(), "GetGLTransactions");

                respostaDTO = new RespostaDTO("", "0007", errorDTO.message);
                return respostaDTO;
            }
        }

        public void actualizarEstadoDoPagamentoByTransactionId(string estado, string descricao, PaymentCheckedDTO paymentHeader, PaymentCheckedRecordsDTO pagamento)
        {

            EncryptionHelper encryptionHelper = new EncryptionHelper();

            Debug.Print("Entrou na actualizacao por ID");
            var payment = _paymentRespository.GetPayment(pagamento.TransactionId.Trim(), paymentHeader.BatchId);

            //var decryptTran = encryptionHelper.DecryptText(connString, u2BPaymentsQueue.transactionId, u2BPaymentsQueue.keystamp, u2BPaymentsQueue.BatchId);
            var encryptedData = _paymentRespository.GetPaymentsQueueBatchId(paymentHeader.BatchId);


            var paymentQueue = encryptedData
                                     .Where(u2BPaymentsQueue => encryptionHelper.DecryptText(u2BPaymentsQueue.TransactionId, u2BPaymentsQueue.Keystamp) == pagamento.TransactionId.Trim())
                                     .FirstOrDefault();

            var wspayment = _phcRepository.GetWspaymentsByDestino(paymentHeader.BatchId, payment.Oristamp);
            /*
            */


            Debug.Print("Prontos para actualziar");
            Debug.Print($"Payment Queue {JsonConvert.SerializeObject(paymentQueue)}");
            if (payment != null)
            {
                payment.Dataprocessado = DateTime.Now;
                payment.Estado = estado;
                payment.Descricao = descricao;
                payment.Usrdata = DateTime.Now;
                payment.BankReference = pagamento.BankReference;
            }

            if (wspayment != null)
            {
                wspayment.Dataprocessado = DateTime.Now;
                wspayment.Estado = estado;
                wspayment.Descricao = descricao;
                wspayment.Usrdata = DateTime.Now;
                wspayment.Bankreference = pagamento.BankReference;
            }
            /*
            */
            if (paymentQueue != null)
            {
                //_genericPaymentRepository.Delete(paymentQueue);
            }

            DateTime processingDate = ParseProcessingDate(paymentHeader.ProcessingDate);
            Debug.Print($"processingDateprocessingDate: {paymentHeader.ProcessingDate}");


            switch (payment.Tabela)
            {
                case "PO":
                    var po = _phcRepository.GetPo(paymentQueue.Oristamp);

                    po.Process = true;
                    po.URefbanco = pagamento.BankReference;
                    po.Dvalor = processingDate;
                    //po.Tbcheque = pagamento.BankReference;

                    break;

                case "PD":
                    var pd = _phcRepository.GetPd(paymentQueue.Oristamp);

                    //pd.Process = true;
                    pd.URefbanco = pagamento.BankReference;
                    pd.Rdata = processingDate;
                    //pd.Cheque = pagamento.BankReference.ToString();

                    break;

                case "OW":
                    var ol = _phcRepository.GetOw(paymentQueue.Oristamp);

                    //ol.Process = true;
                    ol.Dvalor = processingDate;
                    //ol.Cheque = pagamento.BankReference;

                    break;
                case "TB":
                    var tb = _phcRepository.GetTb(paymentQueue.Oristamp);

                    //tb.Dvalor = paymentHeader.ProcessingDate;
                    //tb.Cheque = pagamento.BankReference;

                    break;
                default:
                    break;

            }
            /*
            */

            var trfb = _phcRepository.GetUTrfb(paymentQueue.BatchId);
            if (trfb != null)
            {
                trfb.Rdata = processingDate;
            }
            /*
            */
            _genericPaymentRepository.SaveChanges();
            _genericPHCRepository.SaveChanges();

        }

        DateTime ParseProcessingDate(string processingDateString)
        {
            if (DateTime.TryParse(processingDateString, out DateTime result))
            {
                return result;
            }

            throw new ArgumentException($"Invalid date format: {processingDateString}");
        }

        public void actualizarEstadoDoPagamentoByTransactionId(string estado, string descricao, U2bPaymentsQueue paymentQueue)
        {

            EncryptionHelper encryptionHelper = new EncryptionHelper();

            Debug.Print("Entrou na actualizacao por ID");
            var payment = _paymentRespository.GetPaymentByStamp(paymentQueue.U2bPaymentsQueuestamp);

            //var decryptTran = encryptionHelper.DecryptText(connString, u2BPaymentsQueue.transactionId, u2BPaymentsQueue.keystamp, u2BPaymentsQueue.BatchId);


            var wspayment = _phcRepository.GetWspaymentsByStamp(paymentQueue.U2bPaymentsQueuestamp);


            Debug.Print("Prontos para actualziar");
            Debug.Print($"Payment Queue {JsonConvert.SerializeObject(paymentQueue)}");
            if (payment != null)
            {
                payment.Dataprocessado = DateTime.Now;
                payment.Estado = estado;
                payment.Descricao = descricao;
                payment.Usrdata = DateTime.Now;
                //payment.BankReference = pagamento.BankReference;
            }

            if (wspayment != null)
            {
                wspayment.Dataprocessado = DateTime.Now;
                wspayment.Estado = estado;
                wspayment.Descricao = descricao;
                wspayment.Usrdata = DateTime.Now;
                //wspayment.Bankreference = pagamento.BankReference;
            }


            switch (payment.Tabela)
            {
                case "PO":
                    var po = _phcRepository.GetPo(paymentQueue.Oristamp);

                    po.Process = true;
                    //po.URefbanco = pagamento.BankReference;
                    //po.Tbcheque = pagamento.BankReference;
                    po.Dvalor = paymentQueue.ProcessingDate.Value;

                    break;

                case "PD":
                    var pd = _phcRepository.GetPd(paymentQueue.Oristamp);

                    //pd.URefbanco = pagamento.BankReference;
                    //pd.Cheque = pagamento.BankReference.ToString();
                    pd.Rdata = paymentQueue.ProcessingDate.Value;

                    break;

                case "OW":
                    var ol = _phcRepository.GetOw(paymentQueue.Oristamp);

                    //ol.Cheque = pagamento.BankReference;
                    ol.Dvalor = paymentQueue.ProcessingDate.Value;

                    break;
                case "TB":
                    var tb = _phcRepository.GetTb(paymentQueue.Oristamp);

                    //tb.Dvalor = paymentHeader.ProcessingDate;
                    //tb.Cheque = pagamento.BankReference;

                    break;
                default:
                    break;

            }

            var trfb = _phcRepository.GetUTrfb(paymentQueue.BatchId);
            if (trfb != null)
            {
                trfb.Rdata = paymentQueue.ProcessingDate.Value;
            }

            _genericPaymentRepository.SaveChanges();
            _genericPHCRepository.SaveChanges();

        }

        public void actualizarEstadoDoPagamento(PaymentsQueue u2BPayments, string estado, string descricao)
        {
            Debug.Print("Entrou na atualizacao");

            //var paymentsToUpdate = _wSCTX.U2bPayments.Where(upayment => upayment.BatchId == u2BPayments.payment.BatchId).ToList();
            var paymentsToUpdate = _paymentRespository.GetPaymentsBatchId(u2BPayments.payment.BatchId);
            var paymentQueueToUpdate = _paymentRespository.GetPaymentsQueueBatchId(u2BPayments.payment.BatchId);
            var wspaymentToUpdate = _phcRepository.GetWspayments(u2BPayments.payment.BatchId);

            foreach (var payment in paymentsToUpdate)
            {
                payment.Estado = estado;
                payment.Descricao = descricao;
                payment.Usrdata = DateTime.Now;
            }

            foreach (var paymentQueue in paymentQueueToUpdate)
            {
                paymentQueue.Estado = estado;
                paymentQueue.Descricao = descricao;
                paymentQueue.Usrdata = DateTime.Now;
            }

            foreach (var wspayment in wspaymentToUpdate)
            {
                wspayment.Estado = estado;
                wspayment.Descricao = descricao;
                wspayment.Usrdata = DateTime.Now;

                _genericPHCRepository.Update(wspayment);
                _genericPHCRepository.SaveChanges();
            }



            _genericPaymentRepository.SaveChanges();
        }

        public void insere2bHistorico(string transactionId, string batchid, string oristamp, string codStatusHs, string descStatusHs, string codStatus, string descStatus)
        {
            Debug.Print("Insere HS na actualizacao");
            string stampHs = 25.UseThisSizeForStamp();
            //U2bPaymentsHs u2Bhistoric = new U2bPaymentsHs { transactionId, "", "", "", "", 0, "", codStatus, descStatus, batchid, DateTime.Now, codStatusHs, descStatusHs, stampHs, "", DateTime.Now };

            if (_genericPaymentRepository == null)
            {
                throw new InvalidOperationException("O repositório foi descartado antes da chamada.");
            }
            U2bPaymentsHs u2Bhistoric = new U2bPaymentsHs
            {
                TransactionId = transactionId,
                CreditAccount = "",
                BeneficiaryName = "",
                TransactionDescription = "",
                Currency = "",
                Amount = 0,
                BankReference = "",
                StatusCode = codStatus,
                StatusDescription = descStatus,
                BatchId = batchid,
                ProcessingDate = DateTime.Now,
                StatusCodeHs = codStatusHs,
                StatusDescriptionHs = descStatusHs,
                U2bPaymentsHsstamp = stampHs,
                DebitAccount = "",
                Ousrdata = DateTime.Now
            };


            try
            {
                _genericPaymentRepository.Add(u2Bhistoric);
                _genericPaymentRepository.SaveChanges();
                Debug.Print("Insere HS na actualizacao   SaveChanges");
            }
            catch (ObjectDisposedException ex)
            {
                Debug.Print("Erro: " + ex.Message);
                throw;
            }

        }

        public RespostaDTO GerarResposta(Response response, decimal responseID)
        {
            RespostaDTO responseDto = new RespostaDTO();

            switch (response.Code)
            {
                case "INS-0":
                    responseDto = new RespostaDTO("0000", "Sucesso", "Processado com sucesso");
                    break;
                case "INS-6":
                    responseDto = new RespostaDTO("0001", "Erro", "Transação falhou");
                    break;
                case "INS-9":
                    responseDto = new RespostaDTO("0002", "Erro", "Timeout na requisição");
                    break;

            }

            responseDto.Id = responseID;

            return responseDto;
        }

    }

}
