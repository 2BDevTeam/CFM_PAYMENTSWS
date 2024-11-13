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

namespace CFM_PAYMENTSWS.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly LogHelper logHelper = new LogHelper();
        private readonly ProviderHelper providerHelper = new ProviderHelper();
        private readonly RouteMapper routeMapper = new RouteMapper();

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


        public void ProcessarPagamentos()
        {
            string lockKey = "processarPagamentos";

            if (VerificarJobActivos(lockKey))
                return;

            try
            {
                var pagamentos = _paymentRespository.GetPagamentQueue("Por enviar");

                foreach (var pagamento in pagamentos)
                {
                    switch (pagamento.canal)
                    {
                        case 105:
                            NedBankProcessing(pagamento);

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


        public bool VerificarJobActivos(string lockKey)
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

        void NedBankProcessing(PaymentsQueue pagamento)
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
            logHelper.generateLogJB(nedbankResponse, pagamento.payment.BatchId, "PaymentService.processarPagamento", pagamento.payment.ToString());

        }


        public async Task<ResponseDTO> actualizarPagamentos(PaymentCheckedDTO paymentHeader)
        {
            List<PaymentRecordResponseDTO> paymentRecordResponseDTOs = new List<PaymentRecordResponseDTO>();

            string json1 = JsonConvert.SerializeObject(paymentHeader);

            Debug.Print("paymentRecordResponseDTOs" + json1);

            logHelper.generateLogJB(new ResponseDTO(), paymentHeader.BatchId, "PaymentService.actualizarPagamentos", json1);
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

                        insere2bHistorico(pagamento.TransactionId, paymentHeader.BatchId, paymentHeader.BatchId, paymentHeader.StatusCode, paymentHeader.StatusDescription, pagamento.StatusCode, pagamento.StatusDescription);

                        switch (pagamento.StatusCode)
                        {
                            case "0000":
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

                            default:
                                actualizarEstadoDoPagamentoByTransactionId("Erro", "Código de status desconhecido", paymentHeader, pagamento);
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

        public void actualizarEstadoDoPagamentoByTransactionId(string estado, string descricao, PaymentCheckedDTO paymentHeader, PaymentCheckedRecordsDTO pagamento)
        {

            EncryptionHelper encryptionHelper = new EncryptionHelper();

            Debug.Print("Entrou na actualizacao por ID");
            var payment = _paymentRespository.GetPayment(pagamento.TransactionId, paymentHeader.BatchId);

            //var decryptTran = encryptionHelper.DecryptText(connString, u2BPaymentsQueue.transactionId, u2BPaymentsQueue.keystamp, u2BPaymentsQueue.BatchId);
            var encryptedData = _paymentRespository.GetPaymentsQueueBatchId(paymentHeader.BatchId);


            var paymentQueue = encryptedData
                                     .Where(u2BPaymentsQueue => encryptionHelper.DecryptText(u2BPaymentsQueue.TransactionId, u2BPaymentsQueue.Keystamp) == pagamento.TransactionId)
                                     .FirstOrDefault();

            var wspayment = _phcRepository.GetWspaymentsByDestino(paymentHeader.BatchId, payment.Oristamp);



            Debug.Print("Prontos para actualziar");
            Debug.Print($"Payment Queue {JsonConvert.SerializeObject(paymentQueue)}");
            if (payment != null)
            {
                payment.Dataprocessado = DateTime.Now;
                payment.Estado = estado;
                payment.Descricao= descricao;
                payment.Usrdata = DateTime.Now;
                payment.BankReference= pagamento.BankReference;
            }

            if (wspayment != null)
            {
                wspayment.Dataprocessado = DateTime.Now;
                wspayment.Estado = estado;
                wspayment.Descricao = descricao;
                wspayment.Usrdata = DateTime.Now;
                wspayment.Bankreference = pagamento.BankReference;
            }

            if (paymentQueue != null)
            {
                _genericPaymentRepository.Delete(paymentQueue);
            }


            switch (payment.Tabela)
            {
                case "PO":
                    var po = _phcRepository.GetPo(paymentQueue.Oristamp);

                    po.Process = true;
                    po.URefbanco = pagamento.BankReference;
                    po.Dvalor = paymentHeader.ProcessingDate;
                    po.Tbcheque = pagamento.BankReference;
                    
                    break;

                case "PD":
                    var pd = _phcRepository.GetPd(paymentQueue.Oristamp);

                    //pd.Process = true;
                    pd.URefbanco = pagamento.BankReference;
                    pd.Rdata = paymentHeader.ProcessingDate;
                    pd.Cheque=pagamento.BankReference.ToString();

                    break;

                case "OW":
                    var ol = _phcRepository.GetOw(paymentQueue.Oristamp);

                    //ol.Process = true;
                    ol.Dvalor = paymentHeader.ProcessingDate;
                    ol.Cheque = pagamento.BankReference;

                    break;
                case "TB":
                    /*
                    var tb = _phcRepository.GetOw(paymentQueue.Oristamp);

                    tb.Dvalor = paymentHeader.ProcessingDate;
                    tb.Cheque = pagamento.BankReference;
                    */

                    break;
                default:
                    break;

            }

            var trfb = _phcRepository.GetUTrfb(paymentQueue.BatchId);
            if (trfb != null) {
                trfb.Rdata = paymentHeader.ProcessingDate;
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

            _genericPaymentRepository.Add(u2Bhistoric);

            _genericPaymentRepository.SaveChanges();
        }


        
    }

}
