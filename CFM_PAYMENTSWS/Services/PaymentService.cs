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

namespace CFM_PAYMENTSWS.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly LogHelper logHelper = new LogHelper();
        private readonly ProviderRoute providerRoute = new ProviderRoute();


        //private readonly IPHCRepository _PHCRespository;
        private readonly IPHCRepository<AppDbContext> _phcRepository;
        private readonly IGenericRepository<AppDbContext> _genericRepository;
        private readonly IPaymentRepository<AppDbContext> _paymentRespository;

        public PaymentService(IPHCRepository<AppDbContext> phcRepository, IGenericRepository<AppDbContext> genericRepository, IPaymentRepository<AppDbContext> paymentRepository)
        {
            _phcRepository = phcRepository;
            _genericRepository = genericRepository;
            _paymentRespository = paymentRepository;
        }


        public PaymentService()
        {

        }

        public async Task processarPagamentos()
        {

            try
            {
                //Busca os pagamentos na fila

                var pagamentos = _paymentRespository.GetPagamentQueue("Por enviar");

                foreach (var pagamento in pagamentos)
                {
                    ResponseDTO nedbankResponse = await providerRoute.loadPaymentRoute(pagamento);
                    Debug.Print("Resposta do Load");

                    switch (nedbankResponse.response.cod)
                    {
                        case "0011":

                            actualizarEstadoDoPagamento(pagamento, "Por corrigir", nedbankResponse.response.codDesc);
                            Debug.Print("Teste Por Corrigir" + nedbankResponse.response.codDesc);
                            insere2bHistorico("", pagamento.payment.BatchId, pagamento.payment.BatchId, nedbankResponse.response.cod, nedbankResponse.response.codDesc, "", "");
                            logHelper.generateResponseLog(nedbankResponse, pagamento.payment.BatchId, "PaymentService.processarPagamento", pagamento.payment.ToString());
                            break;

                        case "0000":
                            actualizarEstadoDoPagamento(pagamento, "Por processar", "Pagamento enviado por processar");
                            Debug.Print("Teste Por processar" + nedbankResponse.response.codDesc);
                            insere2bHistorico("", pagamento.payment.BatchId, pagamento.payment.BatchId, nedbankResponse.response.cod, nedbankResponse.response.codDesc, "", "");

                            logHelper.generateResponseLog(nedbankResponse, pagamento.payment.BatchId, "PaymentService.processarPagamento", pagamento.payment.ToString());
                            break;

                        case "0010":
                        case "0007":
                            Debug.Print("Teste HS2" + nedbankResponse.response.codDesc);

                            insere2bHistorico("", pagamento.payment.BatchId, pagamento.payment.BatchId, nedbankResponse.response.cod, nedbankResponse.response.codDesc, "", "");

                            logHelper.generateResponseLog(nedbankResponse, pagamento.payment.BatchId, "PaymentService.processarPagamento", pagamento.payment.ToString());
                            break;
                        default:
                            Debug.Print("Teste HS3" + nedbankResponse.response.codDesc);
                            insere2bHistorico("", pagamento.payment.BatchId, pagamento.payment.BatchId, nedbankResponse.response.cod, nedbankResponse.response.codDesc, "", "");

                            logHelper.generateResponseLog(nedbankResponse, pagamento.payment.BatchId, "PaymentService.processarPagamento", pagamento.payment.ToString());
                            break;

                    }
                }

            }
            catch (Exception ex)
            {
                Debug.Print($"FALHA GLOBAL SERVICE EXCPETION {ex.Message.ToString()} INNER EXEPTION {ex.InnerException}");
                var response = new ResponseDTO(new ResponseCodesDTO("0007", "Internal error"), $"MESSAGE :{ex.Message} STACK:{ex.StackTrace} INNER{ex.InnerException}", null);
                logHelper.generateResponseLogJB(response, "processarPagamento" + Guid.NewGuid(), "PaymentService.processarPagamento", ex.Message);
                //Debug.Print("EXEPCAO_GERAR_FICHEIRO_SYNCRONIZACAO" + ex.Message + " Inner  exx2b " + ex.InnerException);

            }




        }

        public async Task<ResponseDTO> actualizarPagamentos(PaymentCheckedDTO paymentHeader)
        {
            List<PaymentRecordResponseDTO> paymentRecordResponseDTOs = new List<PaymentRecordResponseDTO>();
            string batchId = "";

            //Insere em HS e logs
            bool existe = false;

            //Validar Batchid que retorna logico e caso exista retorna "OK" e caso não informamos que não existe o batchid

            logHelper.generateResponseLog(new ResponseDTO(), paymentHeader.BatchId, "PaymentService.validarPagamentos", paymentHeader.PaymentCheckedRecords.ToString());

            try
            {
                if (paymentHeader != null)
                {
                    batchId = paymentHeader.BatchId;
                    Debug.Print("Entrou porque tem pagamentos");

                    //Validar se o batchid existe

                    existe = _paymentRespository.verificaBatchId(batchId);

                    if (existe == false)
                    {
                        return new ResponseDTO(new ResponseCodesDTO("0050", "Batchid not found", logHelper.generateResponseID()), null, null);
                    }

                    foreach (var pagamento in paymentHeader.PaymentCheckedRecords)
                    {
                        insere2bHistorico(pagamento.TransactionId, paymentHeader.BatchId, paymentHeader.BatchId, paymentHeader.StatusCode, paymentHeader.StatusDescription, pagamento.StatusCode, pagamento.StatusDescription);

                        switch (pagamento.StatusCode)
                        {
                            case "1000":
                                actualizarEstadoDoPagamentoByTransactionId(pagamento.TransactionId, "Sucesso", "Pagamento processado com sucesso", pagamento.BankReference, batchId);
                                // Aqui, você precisa criar um objeto PaymentRecordResponseDTO e adicioná-lo à lista paymentRecordResponseDTOs, se necessário.
                                // Exemplo:
                                // var responseDTO = new ResponseDTO();
                                // var paymentRecordResponseDTO = new PaymentRecordResponseDTO
                                // {
                                //     // Defina as propriedades do paymentRecordResponseDTO com base nas informações do pagamento, batchId, e responseDTO, se aplicável.
                                // };
                                // paymentRecordResponseDTOs.Add(paymentRecordResponseDTO);
                                break;

                            default:
                                actualizarEstadoDoPagamentoByTransactionId(pagamento.TransactionId, "Sucesso", "Pagamento processado com sucesso", pagamento.BankReference, batchId);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Lógica de tratamento de exceção, se necessário.
                Debug.Print("Erro: " + ex.Message);
            }

            return new ResponseDTO(new ResponseCodesDTO("0404", "Operação Inválida.", logHelper.generateResponseID()), null, null);
        }

        public void actualizarEstadoDoPagamentoByTransactionId(string transactionId, string estado, string descricao, string bankReference, string batchId)
        {

            EncryptionHelper encryptionHelper = new EncryptionHelper();

            Debug.Print("Entrou na actualizacao por ID");
            var payment = _paymentRespository.GetPayment(transactionId, batchId);

            //var decryptTran = encryptionHelper.DecryptText(connString, u2BPaymentsQueue.transactionId, u2BPaymentsQueue.keystamp, u2BPaymentsQueue.BatchId);
            var encryptedData = _paymentRespository.GetPaymentsQueueBatchId(batchId);

            var paymentQueue = encryptedData
                                     .Where(u2BPaymentsQueue => encryptionHelper.DecryptText(u2BPaymentsQueue.transactionId, u2BPaymentsQueue.keystamp) == transactionId)
                                     .FirstOrDefault();


            Debug.Print("Prontos para actualziar");
            if (payment != null)
            {
                if (estado == "Sucesso")
                    payment.dataprocessado = DateTime.Now;

                payment.estado = estado;
                payment.descricao = descricao;
                payment.usrdata = DateTime.Now;
                payment.bankReference = bankReference;


            }
            Debug.Print("After Payment");

            if (paymentQueue != null)
            {

                paymentQueue.estado = estado;
                paymentQueue.descricao = descricao;
                paymentQueue.usrdata = DateTime.Now;
            }

            Debug.Print("Afer Payment Queue");

            _genericRepository.SaveChanges();

        }



        public void actualizarEstadoDoPagamento(PaymentsQueue u2BPayments, string estado, string descricao)
        {
            Debug.Print("Entrou na atualizacao");

            //var paymentsToUpdate = _wSCTX.U2bPaymentsTs.Where(upayment => upayment.BatchId == u2BPayments.payment.BatchId).ToList();
            var paymentsToUpdate = _paymentRespository.GetPaymentsBatchId(u2BPayments.payment.BatchId);
            var paymentQueueToUpdate = _paymentRespository.GetPaymentsQueueBatchId(u2BPayments.payment.BatchId);

            foreach (var payment in paymentsToUpdate)
            {
                payment.estado = estado;
                payment.descricao = descricao;
                payment.usrdata = DateTime.Now;
            }

            foreach (var paymentQueue in paymentQueueToUpdate)
            {
                paymentQueue.estado = estado;
                paymentQueue.descricao = descricao;
                paymentQueue.usrdata = DateTime.Now;
            }

            //_wSCTX.ChangeTracker.AutoDetectChangesEnabled = false;
            _genericRepository.SaveChanges();
        }



        public void insere2bHistorico(string transactionId, string batchid, string oristamp, string codStatusHs, string descStatusHs, string codStatus, string descStatus)
        {
            Debug.Print("Insere HS na actualizacao");
            string stampHs = 25.UseThisSizeForStamp();
            U2bPaymentsHsTs u2Bhistoric = new U2bPaymentsHsTs(transactionId, "", "", "", "", 0, "", codStatus, descStatus, batchid, DateTime.Now, codStatusHs, descStatusHs, stampHs, "", DateTime.Now);
            _genericRepository.Add(u2Bhistoric);

            _genericRepository.SaveChanges();
        }


        /*
        public async Task<ResponseDTO> GetGLTransactions(string inicialDate, string finDate, int? pageIndex, int? pageSize)
        {
            try
            {
                var responseID = logHelper.generateResponseID();

                var e1 = _phcRepository.GetE1ByEstab(1);
                var para1 = _phcRepository.GetPara1ByDescricao("mc_dataf");

                DateTime mcDataf = DateTime.ParseExact(para1.Valor.Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture);


                DateTime iDate = DateTime.ParseExact(inicialDate, "yyyyMMdd", CultureInfo.InvariantCulture);
                DateTime finalDate = DateTime.ParseExact(finDate, "yyyyMMdd", CultureInfo.InvariantCulture);

                var glt = _phcRepository.GetGLTransactions(iDate, finalDate, e1.Ncont, mcDataf, pageIndex, pageSize);

                if (glt.Count == 0)
                {
                    throw new Exception("No data.");
                }


                var records = glt.Count;

                int pageI = 1;
                var pageS = glt.Count;
                int recordsPPage = glt.Count;

                if (pageIndex.HasValue && pageSize.HasValue)
                {
                    records = _phcRepository.GetGLTransactionsCount(iDate, finalDate);
                    pageI = pageIndex.Value;
                    pageS = pageSize.Value;
                }

                return new ResponseDTO(new HeaderDTO(records, pageI, pageS, recordsPPage)
                        , new ResponseCodesDTO("0000", "Success.", responseID), glt, null);
            }

            catch (FormatException)
            {
                return new ResponseDTO(new HeaderDTO(0, 0, 0, 0)
                        , new ResponseCodesDTO("0404", "Formatação da data inválida. Por favor, utilize 'yyyyMMdd'.", 0), null, null);
            }
            catch (Exception ex)
            {
                var errorDTO = new ErrorDTO { message = ex?.Message, stack = ex?.StackTrace?.ToString(), inner = ex?.InnerException?.ToString() + "  " };
                Debug.Print($"GetGLTransactions ERROR {errorDTO}");
                var finalResponse = new ResponseDTO(new ResponseCodesDTO("0007", "Error", logHelper.generateResponseID()), errorDTO.ToString(), null);
                logHelper.generateResponseLogJB(finalResponse, logHelper.generateResponseID().ToString(), "GetGLTransactions", errorDTO?.ToString());

                return new ResponseDTO(new HeaderDTO(0, 0, 0, 0)
                        , new ResponseCodesDTO("0404", errorDTO.message, 0), null, null);
            }
        }
        */

    }

}
