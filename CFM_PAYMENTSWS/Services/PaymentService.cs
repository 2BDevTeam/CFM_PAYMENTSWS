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

        public void ProcessarEmails()
        {
            string lockKey = "processarEmails";

            if (VerificarJobActivos(lockKey))
                return;

            try
            {
                var liames = _phcRepository.GetLiameProcessado(false);

                foreach (var liame in liames)
                {
                    var suliame = _paymentRespository.getUserEmail(int.Parse(liame.Userno));

                    //O liame.Corpo só vem com o trecho de codigo do OTP, o restante do html deve ser completado pelo Fullbody
                    var fullBody = _phcRepository.GetFullBody(liame.Corpo);

                    //Chamada da SP de Envio de Emails
                    var email = _phcRepository.SendEmail(suliame.Email, liame.Assunto, fullBody);

                    Debug.Print($"email {suliame.Email}");
                    Debug.Print($"corpo {liame.Corpo}");
                    Debug.Print($"emailReport {email}");

                    liame.Processado = true;
                    _genericPHCRepository.Update(liame);
                    _genericPHCRepository.SaveChanges();

                    var response = new ResponseDTO(new ResponseCodesDTO("0000", "Emails"), email, null);
                    logHelper.generateLogJB(response, "processarEmails-" + Guid.NewGuid(), "PaymentService.processarEmails", email);
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

            logHelper.generateLogJB(new ResponseDTO(), paymentHeader.BatchId, "PaymentService.validarPagamentos", paymentHeader.PaymentCheckedRecords.ToString());

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

                        //var pagamentosRecebidos = _paymentRespository.GetPagamentQueue("Por enviar");

                        /*
                        var pagamentosRecebidos = new U2bPaymentsQueueTs
                        {
                            U2bPaymentsQueueTsstamp = 25.UseThisSizeForStamp(),
                            BatchId = batchId,
                            transactionId = pagamento.TransactionId,
                            transactionDescription = pagamento.StatusDescription,
                            Oristamp = batchId,
                            estado = "Sucesso",
                            descricao = "Pagamento recebido mas não processado.",
                            Ousrdata = DateTime.Now,
                            //Ousrhora = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second,
                        };

                        _genericPaymentRepository.Add(pagamentosRecebidos);
                        _genericPaymentRepository.SaveChanges();
                        */

                        insere2bHistorico(pagamento.TransactionId, paymentHeader.BatchId, paymentHeader.BatchId, paymentHeader.StatusCode, paymentHeader.StatusDescription, pagamento.StatusCode, pagamento.StatusDescription);

                        switch (pagamento.StatusCode)
                        {
                            case "1000":
                                actualizarEstadoDoPagamentoByTransactionId("Sucesso", "Pagamento processado com sucesso", paymentHeader, pagamento);
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
                                actualizarEstadoDoPagamentoByTransactionId("Sucesso", "Pagamento processado com sucesso", paymentHeader, pagamento);
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
                                     .Where(u2BPaymentsQueue => encryptionHelper.DecryptText(u2BPaymentsQueue.transactionId, u2BPaymentsQueue.keystamp) == pagamento.TransactionId)
                                     .FirstOrDefault();

            var wspayment = _phcRepository.GetWspaymentsByDestino(paymentHeader.BatchId, payment.Destino);

            Debug.Print("Prontos para actualziar");
            if (payment != null)
            {
                payment.dataprocessado = DateTime.Now;
                payment.estado = estado;
                payment.descricao = descricao;
                payment.usrdata = DateTime.Now;
                payment.bankReference = pagamento.BankReference;

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
                    var po = _phcRepository.GetPo(paymentHeader.BatchId);

                    po.Process = true;
                    po.URefbanco = pagamento.BankReference;
                    po.Dvalor = paymentHeader.ProcessingDate;

                    _genericPHCRepository.SaveChanges();
                    break;

                default:
                    break;
            }


            _genericPaymentRepository.SaveChanges();
            _genericPHCRepository.SaveChanges();

        }



        public void actualizarEstadoDoPagamento(PaymentsQueue u2BPayments, string estado, string descricao)
        {
            Debug.Print("Entrou na atualizacao");

            //var paymentsToUpdate = _wSCTX.U2bPaymentsTs.Where(upayment => upayment.BatchId == u2BPayments.payment.BatchId).ToList();
            var paymentsToUpdate = _paymentRespository.GetPaymentsBatchId(u2BPayments.payment.BatchId);
            var paymentQueueToUpdate = _paymentRespository.GetPaymentsQueueBatchId(u2BPayments.payment.BatchId);
            var wspaymentToUpdate = _phcRepository.GetWspayments(u2BPayments.payment.BatchId);

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
            //U2bPaymentsHsTs u2Bhistoric = new U2bPaymentsHsTs { transactionId, "", "", "", "", 0, "", codStatus, descStatus, batchid, DateTime.Now, codStatusHs, descStatusHs, stampHs, "", DateTime.Now };

            U2bPaymentsHsTs u2Bhistoric = new U2bPaymentsHsTs
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
                U2bPaymentsHsTsstamp = stampHs,
                DebitAccount = "",
                Ousrdata = DateTime.Now
            };

            _genericPaymentRepository.Add(u2Bhistoric);

            _genericPaymentRepository.SaveChanges();
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
