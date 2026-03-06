//using Microsoft.Data.SqlClient.Server;
using CFM_PAYMENTSWS.Domains.Contracts;
using CFM_PAYMENTSWS.Domains.Exceptions;
using CFM_PAYMENTSWS.Domains.Interface;
using CFM_PAYMENTSWS.Domains.Interfaces;
using CFM_PAYMENTSWS.Domains.Models;
using CFM_PAYMENTSWS.Domains.Models;
using CFM_PAYMENTSWS.Domains.Models.Enum;
using CFM_PAYMENTSWS.DTOs;
using CFM_PAYMENTSWS.Extensions;
using CFM_PAYMENTSWS.Helper;
using CFM_PAYMENTSWS.Mappers;
using CFM_PAYMENTSWS.Persistence.Contexts;
using CFM_PAYMENTSWS.Persistence.Repositories;
using CFM_PAYMENTSWS.Providers;
using CFM_PAYMENTSWS.Providers.BCI.DTOs;
using CFM_PAYMENTSWS.Providers.BCI.Repository;
using CFM_PAYMENTSWS.Providers.Bim.DTOs;
using CFM_PAYMENTSWS.Providers.Bim.Repository;
using CFM_PAYMENTSWS.Providers.FCB.DTOs;
using CFM_PAYMENTSWS.Providers.FCB.Repository;
using CFM_PAYMENTSWS.Providers.Mpesa;
using CFM_PAYMENTSWS.Providers.Nedbank.DTOs;
using CFM_PAYMENTSWS.Providers.Nedbank.Repository;
using Hangfire;
using Hangfire.States;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MPesa;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks.Dataflow;
using System.Xml.Linq;

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

        #region Job Locking

        public async Task<bool> VerificarJobActivos(string lockKey)
        {
            try
            {
                var jobLock = _phcRepository.GetJobLocks(lockKey);

                if (jobLock != null && jobLock.IsRunning)
                {
                    // Verificar se o job está executando há mais de 2 horas
                    var duracaoExecucao = DateTime.Now - jobLock.DataExec;
                    if (duracaoExecucao.TotalHours > 2)
                    {
                        Debug.Print($"Job '{lockKey}' está executando há {duracaoExecucao.TotalHours:F2} horas. Eliminando e recomeçando...");
                        
                        // Eliminar o job antigo
                        _genericPHCRepository.Delete(jobLock);
                        _genericPHCRepository.SaveChanges();
                        
                        // Criar novo lock
                        jobLock = new JobLocks 
                        { 
                            JobId = lockKey, 
                            IsRunning = true,
                            DataExec = DateTime.Now
                        };
                        _genericPHCRepository.Add(jobLock);
                        _genericPHCRepository.SaveChanges();
                        
                        return false; // Job foi resetado, pode executar
                    }
                    
                    Debug.Print("O job já está em execução");
                    return true;
                }

                if (jobLock == null)
                {
                    jobLock = new JobLocks 
                    { 
                        JobId = lockKey, 
                        IsRunning = true,
                        DataExec = DateTime.Now
                    };
                    _genericPHCRepository.Add(jobLock);
                }
                else
                {
                    jobLock.IsRunning = true;
                    jobLock.DataExec = DateTime.Now;
                }
                
                _genericPHCRepository.SaveChanges();
                return false;
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2627)
            {
                // Violação de chave primária - outro job já inseriu o lock
                Debug.Print($"Job lock '{lockKey}' já foi criado por outro processo (race condition)");
                return true; // Job já está em execução
            }
            catch (Exception ex)
            {
                Debug.Print($"Erro ao verificar job lock: {ex.Message}");
                throw;
            }
        }

        public void TerminarJob(string lockKey)
        {
            var jobLock = _phcRepository.GetJobLocks(lockKey);

            _genericPHCRepository.Delete(jobLock);
            _genericPHCRepository.SaveChanges();
        }

        #endregion


        #region Funções de Recebimentos


        public RespostaDTO GetPaymentStatus(string id)
        {
            U2bRecPayments payment = _paymentRespository.GetU2BRecPayments(id);

            if (payment == null)
                return new RespostaDTO(id, WebTransactionCodes.TRANSACTIONNOTFOUND_PT);

            if (payment.StatusCode == "1000")
                return new RespostaDTO(id, WebTransactionCodes.SUCCESSPAYMENT_PT);

            if (payment.StatusCode == "1001")
                return new RespostaDTO(id, WebTransactionCodes.PENDINGPAYMENT_PT);

            return new RespostaDTO(id, payment.StatusCode, payment.StatusDescription);
        }

        public async Task<List<RespostaDTO>> InsertPayments(List<PaymentDynamicDTO> lstPaymentsDTO)
        {
            List<RespostaDTO> lstRespostas = new List<RespostaDTO>();

            try
            {
                List<U2bRecPayments> payments = ConvertToPayment(lstPaymentsDTO);

                foreach (var payment in payments)
                {
                    try
                    {

                        string refPagSemCheck = payment.Referencia[..^2];
                        string aux = payment.Entidade + refPagSemCheck + (Math.Round(payment.Valor, 2) * 100).ToString("F0");
                        string refClienteValida = apiHelper.CalcularReferenciaComCheckDigit(aux, refPagSemCheck);

                        if (refClienteValida != payment.Referencia)
                        {
                            lstRespostas.Add(new RespostaDTO(payment.IdPagamento, WebTransactionCodes.INVALIDREFERENCE_PT, payment.Referencia.ToString(), payment.IdPagamento));
                            continue;
                        }

                        bool duplicated = await _paymentRespository.PaymentExistsAsync(payment);
                        if (duplicated)
                        {
                            lstRespostas.Add(new RespostaDTO(payment.IdPagamento, WebTransactionCodes.DUPLICATEDPAYMENT_PT, payment.IdPagamento));
                            continue;
                        }

                        Bl bl = _phcRepository.getBlByBancagr(payment.Metodo);
                        if (bl == null)
                        {
                            lstRespostas.Add(new RespostaDTO(payment.IdPagamento, WebTransactionCodes.INVALIDPAYMENTMETHOD_PT, payment.Metodo.ToString(), payment.IdPagamento));
                            continue;
                        }

                        //Adicionar Pagamento
                        await _paymentRespository.AddPayment(payment);
                        lstRespostas.Add(new RespostaDTO(payment.IdPagamento, WebTransactionCodes.SUCCESS_PT, payment.IdPagamento));
                    }
                    catch (Exception paymentEx)
                    {
                        Debug.Print($"  {paymentEx.Message}");
                        //_logger.LogError(paymentEx, $"Error processing payment {payment.IdPagamento}");
                        lstRespostas.Add(new RespostaDTO(
                            payment.IdPagamento,
                            WebTransactionCodes.ERROR,
                            paymentEx.Message
                        ));
                    }

                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return lstRespostas;
        }

        private List<U2bRecPayments> ConvertToPayment(List<PaymentDynamicDTO> paymentDTOs)
        {
            List<U2bRecPayments> paymentList = new List<U2bRecPayments>();

            foreach (var paymentDTO in paymentDTOs)
            {
                U2bRecPayments payment = new U2bRecPayments
                {
                    U2bRecPaymentsStamp = Guid.NewGuid().ToString().Substring(0, 24),
                    IdPagamento = paymentDTO.PaymentId,
                    Entidade = paymentDTO.Entity,
                    Referencia = paymentDTO.CustomerRef,
                    Valor = paymentDTO.Amount,
                    Data = paymentDTO.Date,
                    Metodo = paymentDTO.Method,
                    Provider = "Prov1",
                    Moeda = paymentDTO.Currency,
                    Descricao = paymentDTO.Description ?? "",
                    StatusCode = WebTransactionCodes.PENDINGBATCH.cod,
                    StatusDescription = WebTransactionCodes.PENDINGBATCH.codDesc,
                    RefPagamento = paymentDTO.PaymentReference,
                    Ousrinis = "CFM_Payments",
                    Usrinis = "CFM_Payments",
                };

                paymentList.Add(payment);
            }

            return paymentList;
        }

        public async Task ProcessarRecebimentosAsync()
        {
            string lockKey = "processarRecebimentos";

            if (await VerificarJobActivos(lockKey))
                return;

            try
            {

                List<U2bRecPayments> transacoesPENDINGs = _paymentRespository.GetPendingTransactions();

                Debug.Print($"PagamentoService.processarRecibos.TOTALPAGAMENTOS: {transacoesPENDINGs.Count}");

                foreach (U2bRecPayments transacao in transacoesPENDINGs)
                {
                    Debug.Print("Referência :" + transacao.Referencia);
                    var resp = await processarReciboFt(transacao);
                }

            }
            catch (Exception ex)
            {
                Debug.Print($"FALHA GLOBAL SERVICE EXCPETION {ex.Message.ToString()} INNER EXEPTION {ex.InnerException}");
                var response = new ResponseDTO(new ResponseCodesDTO("0007", "Internal error"), $"MESSAGE :{ex.Message} STACK:{ex.StackTrace} INNER{ex.InnerException}", null);
                logHelper.generateLogJB(
                    response,
                    "ProcessarRecebimentos" + Guid.NewGuid(),
                    "PaymentService.ProcessarRecebimentosAsync",
                    ex.Message,
                    "127.0.0.1",
                    "Error",
                    null,
                    null,
                    null,
                    null,
                    null,
                    "ProcessReceiptsError");
            }
            finally
            {
                TerminarJob(lockKey);
            }
        }

        public async Task<ResponseDTO> processarReciboFt(U2bRecPayments u2bpayments)
        {
            List<ReciboAux> recibos = new();
            try
            {
                string restamp = "";
                string rdstamp = "";

                Debug.Print($"u2bpayments.Ref {u2bpayments.Referencia} ");

                Ft? ft = await _phcRepository.GetFtByRef(u2bpayments.Referencia);
                if (ft == null)
                {
                    // Não deve processar pagamento sem fatura - atualizar status para erro
                    u2bpayments.StatusCode = WebTransactionCodes.INVOICENOTFOUND.cod;
                    u2bpayments.StatusDescription = WebTransactionCodes.INVOICENOTFOUND.codDesc;
                    _paymentRespository.updateTransactionStatus(u2bpayments);
                    // Já é salvo dentro do updateTransactionStatus
                    
                    throw new Exception(WebTransactionCodes.INVOICENOTFOUND.codDesc + ". Pagamento não processado.");
                }

                Cl? cl = await _phcRepository.getClienteByNo(ft.No);
                if (cl == null)
                {
                    // Cliente não encontrado
                    u2bpayments.StatusCode = WebTransactionCodes.CLIENTNOTFOUND.cod;
                    u2bpayments.StatusDescription = WebTransactionCodes.CLIENTNOTFOUND.codDesc;
                    _paymentRespository.updateTransactionStatus(u2bpayments);
                    // Já é salvo dentro do updateTransactionStatus
                    
                    throw new Exception(WebTransactionCodes.CLIENTNOTFOUND.codDesc + ".");
                }

                List<Cc> contacorrente = new List<Cc>();
                contacorrente = await _phcRepository.getContaCorrenteByStamp(ft.Ftstamp);

                Debug.Print("Nº do cliente " + cl.No.ToString());
                Debug.Print($"Conta corrente: {JsonConvert.SerializeObject(contacorrente)}");

                // Validar se existe conta corrente para esta fatura
                if (!contacorrente.Any())
                {
                    u2bpayments.StatusCode = WebTransactionCodes.ACCOUNTNOTFOUND.cod;
                    u2bpayments.StatusDescription = WebTransactionCodes.ACCOUNTNOTFOUND.codDesc;
                    _paymentRespository.updateTransactionStatus(u2bpayments);
                    // Já é salvo dentro do updateTransactionStatus
                    
                    throw new Exception(WebTransactionCodes.ACCOUNTNOTFOUND.codDesc + ". Pagamento não processado.");
                }

                decimal saldo = 0;
                decimal pagamentoVal = u2bpayments.Valor;

                if (contacorrente.Any())
                    saldo = contacorrente.Sum(cc => (cc.Deb - cc.Debf));

                Debug.Print("COUNT DO CC " + contacorrente.Count().ToString());
                if (pagamentoVal <= saldo || true)
                {
                    Debug.Print(" APENAS CRIA RECIBO CC");
                    
                    // Iniciar transação para garantir atomicidade do recibo
                    var transaction = _genericPHCRepository.BeginTransaction();
                    try
                    {
                        criarReciboCC(cl,
                                        pagamentoVal,
                                        recibos,
                                        contacorrente,
                                        u2bpayments
                                        );
                        
                        // Commit da transação se tudo correu bem
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        // Rollback em caso de erro
                        transaction.Rollback();
                        throw new Exception($"Erro ao criar recibo: {ex.Message}", ex);
                    }
                    finally
                    {
                        transaction.Dispose();
                    }
                }
                else
                {
                    throw new Exception("Saldo insuficiente para processar o pagamento.");
                }
                /*
                else if (pagamentoVal > saldo)
                {
                    if (saldo == 0)
                    {
                        Debug.Print(" APENAS CRIA ADIANDAMENTO");

                        criarReciboAdiantamento(cl,
                                    pagamentoVal,
                                    recibos,
                                    u2bpayments
                                     );

                    }
                    else
                    {

                        Debug.Print("  CRIA ADIANDAMENTO E RECIBO CC");

                        criarReciboCC(cl,
                                    saldo,
                                    recibos,
                                    contacorrente,
                                    u2bpayments

                                    );

                        criarReciboAdiantamento(cl,
                                    pagamentoVal - saldo,
                                    recibos,
                                    u2bpayments
                                    );

                    }
                }
                */

                ResponseDTO saveChangesresponse = await _genericPHCRepository.SaveChangesRespDTO();
                Debug.Print("Facturacao Save Changes Result " + saveChangesresponse.ToString());

                if (saveChangesresponse.response.cod != "0000")
                    throw new GeneralException(saveChangesresponse);

                // Atualizar status para sucesso APENAS após tudo correr bem
                u2bpayments.StatusCode = WebTransactionCodes.SUCCESSPAYMENT.cod;
                u2bpayments.StatusDescription = WebTransactionCodes.SUCCESSPAYMENT.codDesc;
                _paymentRespository.updateTransactionStatus(u2bpayments);


                var reciboResult = new PagamentoResultDTO(recibos, contacorrente);

                ResponseDTO response = new ResponseDTO(WebTransactionCodes.SUCCESS, reciboResult, u2bpayments);

                List<ReciboAux> recibosCriados = reciboResult.recibos;

                Debug.Print("PagamentoService.processarCobrancas.RECIBOS: " + JsonConvert.SerializeObject(recibosCriados));

                return response;

            }
            catch (Exception ex)
            {
                // 3) Nunca aceder ex.InnerException sem null-check
                var stack = ex.StackTrace ?? "";
                var deep = DeepMessage(ex); // inclui tipo(s) e SqlNumber se existir

                /*
                var transact = new U2bPayments(
                    transactionID: u2bpayments?.transactionID?.ToString(),
                    @ref: u2bpayments?.Referencia,
                    date: u2bpayments?.Data ?? DateTime.Now,
                    amount: u2bpayments?.Valor ?? 0m,
                    entity: u2bpayments?.Entidade ?? 0,
                    status: "ERRO",
                    message: deep,
                    codigo: ex.GetType().Name
                );
                await _transactionRepository.AddU2bPayments(transact);
                */
                var safeMsg = $"Exceção gerada no pagamento CFM: {deep}. Em {stack}";
                var response = new ResponseDTO(WebTransactionCodes.INTERNALERROR, safeMsg, u2bpayments);

                Debug.Print("PagamentoService.getRecibosAsync.INTERNALERROR: " + deep);
                //logHelper.generateLogJB(response, "", "PagamentoService.getRecibosAsync.INTERNALERROR:");

                return response;
            }
        }

        public void criarReciboCC(Cl cl, decimal pagamento, List<ReciboAux> recibos, List<Cc> contacorrente, U2bRecPayments u2BPayments)
        {
            DateTime data = u2BPayments.Data;
            var restamp = KeysExtension.UseThisSizeForStamp(25);
            var rno = _phcRepository.getMaxRecibo();

            Tsre config = _phcRepository.getConfiguracaoRecibo();
            Bl bl = _phcRepository.getBlByBancagr(u2BPayments.Metodo);
            string banco = bl.Banco, conta = bl.Conta;
            string contaTesouraria = banco.PadRight(10) + " " + conta;
            string moeda = _phcRepository.getMoeda();

            Debug.Print("Configs" + config.ToString());

            Re re = new Re();

            re.Restamp = restamp;
            re.Ccusto = cl.Ccusto;

            re.Chdata = (data == default ? DateTime.Now.Date : data.Date);
            re.Chmoeda = moeda;
            re.Chtotal = pagamento;
            re.Echtotal = pagamento;
            re.Cheque = true;

            re.Contado = bl.Noconta;
            re.Etotal = pagamento;
            re.Etotow = 0;

            re.Fref = cl.Fref;
            re.Local = cl.Local;
            re.Memissao = "PTE";
            re.Morada = cl.Morada;
            re.Ncont = cl.Ncont;

            re.Ndoc = config.Ndoc;
            re.Nmdoc = config.Nmdoc;

            re.No = cl.No;
            re.Nome = cl.Nome;

            re.Olcodigo = "R78211";
            re.Ollocal = contaTesouraria;

            re.Ousrdata = DateTime.Now.Date;
            re.Usrdata = DateTime.Now.Date;

            re.Ousrhora = DateTime.Now.ToString("HH:mm:ss");
            re.Usrhora = DateTime.Now.ToString("HH:mm:ss");

            re.Ousrinis = "FIPAGONLINEPAYMENTSAPI";
            re.Usrinis = "FIPAGONLINEPAYMENTSAPI";

            re.Process = true;

            re.Rdata = (data == default ? DateTime.Now.Date : data.Date);
            re.Reano = (data == default ? DateTime.Now.Year : data.Year);

            re.Rno = rno;

            re.Segmento = cl.Segmento;
            re.Telocal = "B";

            re.Total = pagamento;
            re.Totow = 0;

            re.Procdata = (data == default ? DateTime.Now.Date : data.Date);

            re.Moeda = moeda;

            re.UTransid = u2BPayments.IdPagamento;
            re.UEntps = u2BPayments.Entidade;
            re.URefps = u2BPayments.Referencia;


            Debug.Print("Reeeeeeceba: " + re.ToString());
            _phcRepository.addRecibo(re);


            decimal totalFacturaCorrente = 0;
            decimal totalDividas = 0;

            Debug.Print("Total CC " + contacorrente.Count.ToString());
            foreach (var cc in contacorrente)
            {
                if (pagamento == 0)
                    break;

                var stamprl = KeysExtension.UseThisSizeForStamp(25);

                decimal vPagar = 0;
                if (pagamento <= (cc.Deb - cc.Debf))
                {
                    vPagar = pagamento;
                }
                else
                {
                    vPagar = (cc.Deb - cc.Debf);
                }

                if (cc.Dataven >= new DateTime(data.Year, data.Month, 1))
                {
                    totalFacturaCorrente += vPagar;
                }

                if (cc.Dataven < new DateTime(data.Year, data.Month, 1))
                {
                    totalDividas += vPagar;
                }

                Rl rl = new Rl();

                rl.Restamp = restamp;
                rl.Rlstamp = stamprl;
                rl.Ccstamp = cc.Ccstamp;
                rl.Cdesc = cc.Cmdesc;
                rl.Cm = cc.Cm;

                rl.Datalc = cc.Datalc;
                rl.Dataven = cc.Dataven;

                rl.Enaval = cc.Deb - cc.Debf;
                rl.Eval = cc.Deb - cc.Debf;

                rl.Escrec = vPagar;
                rl.Escval = cc.Deb - cc.Debf;

                rl.Erec = vPagar;
                rl.Evori = cc.Deb;

                rl.Moeda = moeda;
                rl.Val = cc.Deb - cc.Debf;
                rl.Rec = vPagar;

                rl.Ndoc = config.Ndoc;
                rl.Nrdoc = cc.Nrdoc;
                rl.Process = true;

                rl.Rno = rno;
                rl.Rdata = (data == default ? DateTime.Now.Date : data.Date);

                rl.Ousrdata = DateTime.Now.Date;
                rl.Usrdata = DateTime.Now.Date;
                rl.Ousrhora = $"{DateTime.Now.Hour}:{DateTime.Now.Minute}";
                rl.Usrhora = $"{DateTime.Now.Hour}:{DateTime.Now.Minute}";
                rl.Ousrinis = "FIPAGONLINEPAYMENTSAPI";
                rl.Usrinis = "FIPAGONLINEPAYMENTSAPI";

                _phcRepository.addLinhasRecibo(rl);
                pagamento -= vPagar;

            }

            Rech titulo = new();
            titulo.Rechstamp = KeysExtension.UseThisSizeForStamp(25);
            titulo.Restamp = restamp;
            titulo.Clbanco = bl.UBancagr;
            titulo.Clcheque = u2BPayments.IdPagamento;
            titulo.Chdata = data == default ? DateTime.Now.Date : data.Date;
            titulo.Chvalor = u2BPayments.Valor;
            titulo.Echvalor = u2BPayments.Valor;
            titulo.Tptit = "Pag. de Serviços Online";
            titulo.Ousrinis = "FIPAGONLINEPAYMENTSAPI";
            titulo.Usrinis = "FIPAGONLINEPAYMENTSAPI";
            titulo.Ousrdata = DateTime.Now.Date;
            titulo.Usrdata = DateTime.Now.Date;
            titulo.Ousrhora = DateTime.Now.ToString("HH:MM:SS");
            titulo.Usrhora = DateTime.Now.ToString("HH:MM:SS");

            _phcRepository.addTitulos(titulo);

            Debug.Print("totalFacturaCorrente " + totalFacturaCorrente.ToString());
            recibos.Add(
                new ReciboAux(
                    stamp: restamp,
                    numeroCliente: cl.No,
                    nomeCliente: cl.Nome,
                    tipo: TipoReciboEnum.CONTACORRENTE,
                    total: pagamento,
                    paymentResponse: WebTransactionCodes.SUCCESS,
                    totalFacturaCorrente: totalFacturaCorrente,
                    totalDividas: totalDividas
                    )
                );

        }

        private static string DeepMessage(Exception ex)
        {
            var msgs = new List<string>();
            for (var e = ex; e != null; e = e.InnerException)
            {
                msgs.Add($"{e.GetType().Name}: {e.Message}");
                if (e is SqlException se) msgs.Add($"SqlNumber={se.Number}");
            }
            return string.Join(" | ", msgs);
        }

        private string GetCanalNome(int? canal)
        {
            if (canal == null)
                return "Desconhecido";

            var bl = _phcRepository.getBlByCodeprov(canal.Value);
            if (bl == null || string.IsNullOrWhiteSpace(bl.UBancagr))
                return "Desconhecido";

            return bl.UBancagr;
        }

        public void ExpirarPendentesDiaAnterior()
        {
            var today = DateTime.Now.Date;
            var pendentes = _paymentRespository.GetPaymentsQueuePendentesDiaAnterior(today);

            if (pendentes == null || pendentes.Count == 0)
                return;

            EncryptionHelper encryptionHelper = new EncryptionHelper();
            var batchIds = pendentes.Select(p => p.BatchId).Distinct().ToList();

            foreach (var batchId in batchIds)
            {
                var paymentsBatch = _paymentRespository.GetPaymentsBatchId(batchId);
                var wspaymentToUpdate = _phcRepository.GetWspayments(batchId);

                foreach (var queueItem in pendentes.Where(p => p.BatchId == batchId))
                {
                    var transactionId = encryptionHelper.DecryptText(queueItem.TransactionId ?? "", queueItem.Keystamp ?? "");
                    var creditAccount = encryptionHelper.DecryptText(queueItem.Destino ?? "", queueItem.Keystamp ?? "");
                    var beneficiaryName = encryptionHelper.DecryptText(queueItem.BeneficiaryName ?? "", queueItem.Keystamp ?? "");
                    var debitAccount = encryptionHelper.DecryptText(queueItem.Origem ?? "", queueItem.Keystamp ?? "");

                    var payment = paymentsBatch
                        .FirstOrDefault(p => p.Transactionid != null && p.Transactionid.Trim() == transactionId.Trim());

                    var canalNome = GetCanalNome(payment?.Canal ?? queueItem.Canal);

                    insere2bHistorico(
                        transactionId,
                        batchId,
                        creditAccount,
                        beneficiaryName,
                        queueItem.TransactionDescription ?? "",
                        queueItem.Moeda ?? "MZN",
                        queueItem.Valor,
                        payment?.BankReference ?? "",
                        debitAccount,
                        payment?.Oristamp ?? "",
                        payment?.Tabela ?? "",
                        payment?.Canal ?? queueItem.Canal,
                        canalNome,
                        "",
                        "",
                        "9999",
                        "Pendente expirado"
                    );

                    if (payment != null)
                    {
                        payment.Estado = "Erro";
                        payment.Descricao = "Pendente expirado";
                        payment.Usrdata = DateTime.Now;
                    }

                    _genericPaymentRepository.Delete(queueItem);
                }

                foreach (var wspayment in wspaymentToUpdate)
                {
                    wspayment.Estado = "Erro";
                    wspayment.Descricao = "Pendente expirado";
                    wspayment.Usrdata = DateTime.Now;

                    _genericPHCRepository.Update(wspayment);
                }
            }

            _genericPaymentRepository.SaveChanges();
            _genericPHCRepository.SaveChanges();
        }



        #endregion





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
                            pagamentos = await _paymentRespository.GetPagamentQueue("Por enviar", 105);
                            await NedBankProcessing(pagamentos);
                            break;

                        case 106:
                            pagamentos = await _paymentRespository.GetPagamentQueue("Por enviar", 106);
                            await BCIProcessing(pagamentos, false);
                            break;

                        case 107:
                            pagamentos = await _paymentRespository.GetPagamentQueue("Por enviar", 107);
                            await BimProcessing(pagamentos, false);
                            //ScheduleUpdatePayments(pagamentos);
                            break;

                        case 108:
                            pagamentos = await _paymentRespository.GetPagamentQueue("Por enviar", 108);
                            await FcbProcessing(pagamentos);
                            break;


                        case 109:
                            pagamentos = await _paymentRespository.GetPagamentQueue("Por enviar", 109);
                            await MozaProcessing(pagamentos);
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
                            pagamentos = await _paymentRespository.GetPagamentQueue("Por processar", 106);
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

/*
            logHelper.generateLogJB(
                new ResponseDTO(),
                paymentHeader.BatchId,
                "PaymentService.actualizarPagamentos",
                json1,
                "",
                "Info",
                null,
                "POST",
                null,
                null,
                null,
                "UpdatePaymentStatus");
*/

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
                        // Verificar se o pagamento já foi processado com sucesso anteriormente
                        var paymentExistente = _paymentRespository.GetPayment(pagamento.TransactionId.Trim(), paymentHeader.BatchId);

                        if (paymentExistente != null && paymentExistente.Estado == "Sucesso")
                        {
                            Debug.Print($"Pagamento {pagamento.TransactionId} já foi processado com sucesso anteriormente. Request ignorado.");
                            logHelper.generateLogJB(
                                new ResponseDTO(new ResponseCodesDTO("0000", "Pagamento já processado"), null, null),
                                paymentHeader.BatchId,
                                "PaymentService.actualizarPagamentos - Duplicado Ignorado",
                                $"TransactionId: {pagamento.TransactionId}",
                                "127.0.0.1",
                                "Warning",
                                null,
                                null,
                                null,
                                null,
                                null,
                                "DuplicatePaymentIgnored");
                            continue; // Ignora este pagamento e passa para o próximo
                        }

                        // Inserir histórico com dados completos do pagamento
                        if (paymentExistente != null)
                        {

                            string canalNome = paymentExistente.Canal switch
                            {
                                105 => "NEDBANK",
                                106 => "BCI",
                                107 => "BIM",
                                108 => "FCB",
                                109 => "MOZA",
                                _ => "Desconhecido"
                            };

                            insere2bHistorico(
                                pagamento.TransactionId.Trim(),
                                paymentHeader.BatchId,
                                paymentExistente.Destino ?? "",  // NIB de crédito/destino
                                /*
                                paymentExistente.BeneficiaryName ?? "",
                                paymentExistente.TransactionDescription ?? "",
                                */
                                "", "", // BeneficiaryName e TransactionDescription não estão disponíveis na tabela de pagamentos, então deixamos vazios no histórico
                                paymentExistente.Moeda ?? "MZN",
                                paymentExistente.Valor,
                                pagamento.BankReference ?? "",
                                paymentExistente.Origem ?? "",  // Conta de débito/origem
                                paymentExistente.Oristamp ?? "",
                                paymentExistente.Tabela ?? "",
                                paymentExistente.Canal,
                                canalNome,
                                paymentHeader.StatusCode,
                                paymentHeader.StatusDescription,
                                pagamento.StatusCode,
                                pagamento.StatusDescription
                            );

                        }
                        else
                        {
                            Debug.Print($"AVISO: Pagamento {pagamento.TransactionId} não encontrado na base. Histórico não será inserido.");
                        }

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
                                actualizarEstadoDoPagamentoByTransactionId("Erro", pagamento.StatusDescription, paymentHeader, pagamento);
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
                Debug.Print($"paymentv1_5 {paymentv1_5}");
                BimResponseDTO bimResponseDTO = new BimResponseDTO();
                if (checkPayments)
                {
                    //bciResponseDTO = bimRepository.checkPayments(pagamento.payment.BatchId, pagamento.payment.initgPtyCode ?? "");
                }
                else
                {
                    Debug.Print("BimProcessing");
                    Debug.Print($"{paymentv1_5}");
                    bimResponseDTO = await bimRepository.loadPayments(paymentv1_5);
                }


                ResponseDTO bimResponse = routeMapper.mapLoadPaymentResponse(107, bimResponseDTO);

                Debug.Print("Resposta do Load");
                Debug.Print(bimResponse.ToString());


                var paymentsBatch = _paymentRespository.GetPaymentsBatchId(pagamento.payment.BatchId);

                // Inserir histórico com dados completos para cada payment record
                foreach (var paymentRecord in pagamento.payment.PaymentRecords)
                {
                    // Filtrar em memória pelo TransactionId
                    var payment = paymentsBatch.FirstOrDefault(p => p.Transactionid?.Trim() == paymentRecord.TransactionId.Trim());

                    insere2bHistorico(
                        paymentRecord.TransactionId,
                        pagamento.payment.BatchId,
                        paymentRecord.CreditAccount,
                        paymentRecord.BeneficiaryName,
                        paymentRecord.TransactionDescription,
                        paymentRecord.Currency,
                        paymentRecord.Amount,
                        "",
                        pagamento.payment.DebitAccount,
                        payment?.Oristamp ?? "",
                        payment?.Tabela ?? "",
                        107,
                        "BIM","","",
                        bimResponse.response.cod,
                        bimResponse.response.codDesc
                    );
                }

                Debug.Print("insere2bHistorico");

                switch (bimResponse.response.cod)
                {

                    case "0" or "0000" or "0016" or "1001":
                        actualizarEstadoDoPagamento(pagamento, "Por processar", "Pagamento enviado por processar");
                        Debug.Print("Teste Por processar" + bimResponse.response.codDesc);
                        break;

                    case "2028":
                        actualizarEstadoDoPagamento(pagamento, "Por Corrigir", bimResponse.response.codDesc);
                        Debug.Print("Teste Por Corrigir" + bimResponse.response.codDesc);
                        break;

                    default:
                        actualizarEstadoDoPagamento(pagamento, "Erro", bimResponse.response.codDesc);
                        Debug.Print("Teste HS3 - Erro: " + bimResponse.response.codDesc);
                        break;

                }

                logHelper.generateLogJB(
                    bimResponse,
                    pagamento.payment.BatchId,
                    "PaymentService.processarPagamento - Bim",
                    pagamento.payment,
                    "127.0.0.1",
                    "",
                    "BIM",
                    "POST",
                    bimResponseDTO.HttpStatusCode,
                    bimResponseDTO.DurationMs,
                    bimResponseDTO.EndpointUrl,
                    "LoadPayment");

            }

        }

        async Task FcbProcessing(List<PaymentsQueue> pagamentos)
        {
            FcbPaymentDTO fcbPayment = new FcbPaymentDTO();

            foreach (var pagamento in pagamentos)
            {
                try
                {
                    FcbAPI fcbRepository = new FcbAPI();
                    fcbPayment = apiHelper.ConvertPaymentToFcb(pagamento.payment);
                    Debug.Print($"FCB {fcbPayment}");
                    FcbResponseDTO fcbResponseDTO = await fcbRepository.LoadPaymentsAsync(fcbPayment);

                    ResponseDTO fcbResponse = routeMapper.mapLoadPaymentResponse(108, fcbResponseDTO);


                    var paymentsBatch = _paymentRespository.GetPaymentsBatchId(pagamento.payment.BatchId);

                    // Inserir histórico com dados completos para cada payment record
                    foreach (var paymentRecord in pagamento.payment.PaymentRecords)
                    {
                        // Filtrar em memória pelo TransactionId
                        var payment = paymentsBatch.FirstOrDefault(p => p.Transactionid?.Trim() == paymentRecord.TransactionId.Trim());

                        insere2bHistorico(
                            paymentRecord.TransactionId,
                            pagamento.payment.BatchId,
                            paymentRecord.CreditAccount,
                            paymentRecord.BeneficiaryName,
                            paymentRecord.TransactionDescription,
                            paymentRecord.Currency,
                            paymentRecord.Amount,
                            "",
                            pagamento.payment.DebitAccount,
                            payment?.Oristamp ?? "",
                            payment?.Tabela ?? "",
                            108,
                            "FCB","","",
                            fcbResponse.response.cod,
                            fcbResponse.response.codDesc
                        );
                    }

                    switch (fcbResponse.response.cod)
                    {
                        case "0000":
                            actualizarEstadoDoPagamento(pagamento, "Por processar", "Pagamento enviado por processar");
                            break;

                        default:
                            actualizarEstadoDoPagamento(pagamento, "Erro", fcbResponse.response.codDesc);
                            break;
                    }

                    logHelper.generateLogJB(
                        fcbResponse,
                        pagamento.payment.BatchId,
                        "PaymentService.processarPagamento - FCB",
                        fcbPayment,
                        "127.0.0.1",
                        "",
                        "FCB",
                        "POST",
                        fcbResponseDTO.HttpStatusCode,
                        fcbResponseDTO.DurationMs,
                        fcbResponseDTO.EndpointUrl,
                        "LoadPayment");
                }
                catch (Exception ex)
                {
                    var response = new ResponseDTO(new ResponseCodesDTO("0007", "Erro no processamento com o FCB"), ex.Message, null);
                    actualizarEstadoDoPagamento(pagamento, "Erro", ex.Message);
                    logHelper.generateLogJB(
                        response,
                        pagamento.payment.BatchId,
                        "PaymentService.processarPagamento - FCB",
                        fcbPayment,
                        "127.0.0.1",
                        "Error",
                        "FCB",
                        "POST",
                        null,
                        null,
                        null,
                        "LoadPayment");
                }
            }
        }


        async Task BCIProcessing(List<PaymentsQueue> pagamentos, bool checkPayments)
        {

            foreach (var pagamento in pagamentos)
            {

                BCIAPI bciRepository = new BCIAPI();

                PaymentCamelCase paymentCamel = apiHelper.ConvertPaymentToCamelCase(pagamento.payment);

                Debug.Print($"paymentCamel {paymentCamel}");

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

                var paymentsBatch = _paymentRespository.GetPaymentsBatchId(pagamento.payment.BatchId);

                // Inserir histórico com dados completos para cada payment record
                foreach (var paymentRecord in pagamento.payment.PaymentRecords)
                {
                    // Filtrar em memória pelo TransactionId
                    var payment = paymentsBatch.FirstOrDefault(p => p.Transactionid?.Trim() == paymentRecord.TransactionId.Trim());

                    insere2bHistorico(
                        paymentRecord.TransactionId,
                        pagamento.payment.BatchId,
                        paymentRecord.CreditAccount,
                        paymentRecord.BeneficiaryName,
                        paymentRecord.TransactionDescription,
                        paymentRecord.Currency,
                        paymentRecord.Amount,
                        "",
                        pagamento.payment.DebitAccount,
                        payment?.Oristamp ?? "",
                        payment?.Tabela ?? "",
                        106,
                        "BCI","","",
                        bciResponse.response.cod,
                        bciResponse.response.codDesc
                        
                    );
                }


                switch (bciResponse.response.cod)
                {
                    case "2028":
                        actualizarEstadoDoPagamento(pagamento, "Por Corrigir", bciResponse.response.codDesc);
                        Debug.Print("Teste Por Corrigir" + bciResponse.response.codDesc);
                        break;

                    case "3002" or "0000" or "1001":
                        actualizarEstadoDoPagamento(pagamento, "Por processar", "Pagamento enviado por processar");
                        Debug.Print("Teste Por processar" + bciResponse.response.codDesc);
                        break;

                    default:
                        actualizarEstadoDoPagamento(pagamento, "Erro", bciResponse.response.codDesc);
                        Debug.Print("Teste HS3" + bciResponse.response.codDesc);
                        break;

                }


                logHelper.generateLogJB(
                    bciResponse,
                    pagamento.payment.BatchId,
                    "PaymentService.processarPagamento - BCI",
                    paymentCamel,
                    "127.0.0.1",
                    "",
                    "BCI",
                    "POST",
                    bciResponseDTO.HttpStatusCode,
                    bciResponseDTO.DurationMs,
                    bciResponseDTO.EndpointUrl,
                    "LoadPayment");

            }

        }

        async Task MozaProcessing(List<PaymentsQueue> pagamentos)
        {

            foreach (var pagamento in pagamentos)
            {

                var mozaRepository = new Providers.Moza.Repository.MozaAPI();

                PaymentCamelCase paymentCamel = apiHelper.ConvertPaymentToCamelCase_MOZA(pagamento.payment);

                Debug.Print($"paymentCamelMoza {paymentCamel}");

                var mozaResponseDTO = await mozaRepository.LoadPaymentsAsync(paymentCamel);

                ResponseDTO mozaResponse = routeMapper.mapLoadPaymentResponse(109, mozaResponseDTO);

                Debug.Print("Resposta do Load");
                Debug.Print(mozaResponse.ToString());

                var paymentsBatch = _paymentRespository.GetPaymentsBatchId(pagamento.payment.BatchId);

                // Inserir histórico com dados completos para cada payment record
                foreach (var paymentRecord in pagamento.payment.PaymentRecords)
                {
                    // Filtrar em memória pelo TransactionId
                    var payment = paymentsBatch.FirstOrDefault(p => p.Transactionid?.Trim() == paymentRecord.TransactionId.Trim());

                    insere2bHistorico(
                        paymentRecord.TransactionId,
                        pagamento.payment.BatchId,
                        paymentRecord.CreditAccount,
                        paymentRecord.BeneficiaryName,
                        paymentRecord.TransactionDescription,
                        paymentRecord.Currency,
                        paymentRecord.Amount,
                        "",
                        pagamento.payment.DebitAccount,
                        payment?.Oristamp ?? "",
                        payment?.Tabela ?? "",
                        109,
                        "MOZA","","",
                        mozaResponse.response.cod,
                        mozaResponse.response.codDesc
                    );
                }


                switch (mozaResponse.response.cod)
                {
                    case "2028":
                        actualizarEstadoDoPagamento(pagamento, "Por Corrigir", mozaResponse.response.codDesc);
                        Debug.Print("Teste Por Corrigir" + mozaResponse.response.codDesc);
                        break;

                    case "0000" or "1001":
                        actualizarEstadoDoPagamento(pagamento, "Por processar", "Pagamento enviado por processar");
                        Debug.Print("Teste Por processar" + mozaResponse.response.codDesc);
                        break;

                    default:
                        actualizarEstadoDoPagamento(pagamento, "Erro", mozaResponse.response.codDesc);
                        Debug.Print("Teste HS3 - Erro: " + mozaResponse.response.codDesc);
                        break;

                }

                logHelper.generateLogJB(
                    mozaResponse,
                    pagamento.payment.BatchId,
                    "PaymentService.processarPagamento - MOZA",
                    paymentCamel,
                    "127.0.0.1",
                    "",
                    "MOZA",
                    "POST",
                    mozaResponseDTO.HttpStatusCode,
                    mozaResponseDTO.DurationMs,
                    mozaResponseDTO.EndpointUrl,
                    "LoadPayment");
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


                var paymentsBatch = _paymentRespository.GetPaymentsBatchId(pagamento.payment.BatchId);

                // Inserir histórico com dados completos para cada payment record
                foreach (var paymentRecord in pagamento.payment.PaymentRecords)
                {
                    // Filtrar em memória pelo TransactionId
                    var payment = paymentsBatch.FirstOrDefault(p => p.Transactionid?.Trim() == paymentRecord.TransactionId.Trim());

                    insere2bHistorico(
                        paymentRecord.TransactionId,
                        pagamento.payment.BatchId,
                        paymentRecord.CreditAccount,
                        paymentRecord.BeneficiaryName,
                        paymentRecord.TransactionDescription,
                        paymentRecord.Currency,
                        paymentRecord.Amount,
                        "",
                        pagamento.payment.DebitAccount,
                        payment?.Oristamp ?? "",
                        payment?.Tabela ?? "",
                        105,
                        "NEDBANK","","",
                        nedbankResponse.response.cod,
                        nedbankResponse.response.codDesc
                    );
                }

                switch (nedbankResponse.response.cod)
                {
                    case "0011":
                        actualizarEstadoDoPagamento(pagamento, "Por Corrigir", nedbankResponse.response.codDesc);
                        Debug.Print("Teste Por Corrigir" + nedbankResponse.response.codDesc);
                        break;

                    case "0000":
                        actualizarEstadoDoPagamento(pagamento, "Por processar", "Pagamento enviado por processar");
                        Debug.Print("Teste Por processar" + nedbankResponse.response.codDesc);
                        break;

                    case "0010":
                        actualizarEstadoDoPagamento(pagamento, "Por Corrigir", nedbankResponse.response.codDesc);
                        Debug.Print("Teste Por Corrigir - Erro 0010: " + nedbankResponse.response.codDesc);
                        break;

                    case "0007":
                        actualizarEstadoDoPagamento(pagamento, "Erro", nedbankResponse.response.codDesc);
                        Debug.Print("Teste HS2 - Erro: " + nedbankResponse.response.codDesc);
                        break;

                    default:
                        actualizarEstadoDoPagamento(pagamento, "Erro", nedbankResponse.response.codDesc);
                        Debug.Print("Teste HS3 - Erro: " + nedbankResponse.response.codDesc);
                        break;

                }
                logHelper.generateLogJB(
                    nedbankResponse,
                    pagamento.payment.BatchId,
                    "PaymentService.processarPagamento - nedbank",
                    pagamento.payment,
                    "127.0.0.1",
                    "",
                    "NEDBANK",
                    "POST",
                    nedbankResponseDTO.HttpStatusCode,
                    nedbankResponseDTO.DurationMs,
                    nedbankResponseDTO.EndpointUrl,
                    "LoadPayment");

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

                    switch (estadoDoPagamento.response.cod)
                    {
                        case "00077":
                            response = await mpesaApi.B2CpaymentProviderRoute(pagamento);
                            Debug.Print($"RESPOSTA DO PAGAMENTO 00077 {response.ToString()}");
                            actualizarEstadoDoPagamentoByTransactionId("Sucesso", "Pagamento efectuado com sucesso.", pagamento);

                            break;


                        case "0000":
                            Debug.Print($"Pagamento Existente");
                            response = new ResponseDTO(new ResponseCodesDTO("0000", "Success"), estadoDoPagamento.Data, pagamento.ToString());
                            actualizarEstadoDoPagamentoByTransactionId("Sucesso", "Pagamento efectuado com sucesso.", pagamento);

                            break;

                        case "0002":

                            response = await mpesaApi.B2CpaymentProviderRoute(pagamento);
                            Debug.Print($"RESPOSTA DO PAGAMENTO 0002 {response.ToString()}");
                            actualizarEstadoDoPagamentoByTransactionId("Sucesso", "Pagamento efectuado com sucesso.", pagamento);

                            break;

                    }


                }

                catch (Exception ex)
                {
                    Debug.Print($" ERRO ao processar o pagamento com transactionId {pagamento.TransactionId.ToString()}   MESSAGE :{ex.Message} STACK:{ex.StackTrace} INNER{ex.InnerException}");

                    var response = new ResponseDTO(new ResponseCodesDTO("0007", "Internal error"), $" ERRO ao processar o pagamento com transactionId {pagamento.TransactionId.ToString()}   MESSAGE :{ex.Message} STACK:{ex.StackTrace} INNER{ex.InnerException}", null);
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

                switch (estadoDoPagamento.response.cod)
                {
                    case "00077":
                        response = await mpesaApi.C2BPaymentProviderRoute(pagamento);
                        Debug.Print($"RESPOSTA DO PAGAMENTO 00077 {(response.Data as Response)?.Description}");
                        break;

                    case "0000":
                    case "000":
                        Debug.Print($"Pagamento Existente  {(response.Data as Response)?.Description} .");
                        response = new ResponseDTO(new ResponseCodesDTO("0000", "Success"), estadoDoPagamento.Data, pagamento.ToString());
                        break;

                    case "0002":

                        response = await mpesaApi.C2BPaymentProviderRoute(pagamento);
                        Debug.Print($"RESPOSTA DO PAGAMENTO  0002  {response}");
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

            Debug.Print("Prontos para actualziar");
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

            // Eliminar sempre da Queue após ter resultado do banco (sucesso ou erro)
            if (paymentQueue != null)
            {
                _genericPaymentRepository.Delete(paymentQueue);
                Debug.Print($"Pagamento com TransactionId {pagamento.TransactionId} eliminado da Queue após processamento. Estado: {estado}");
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
                    po.Adoc = Truncar(pagamento.BankReference, 20);

                    break;

                case "PD":
                    var pd = _phcRepository.GetPd(paymentQueue.Oristamp);

                    //pd.Process = true;
                    pd.URefbanco = pagamento.BankReference;
                    pd.Rdata = processingDate;
                    //pd.Cheque = pagamento.BankReference.ToString();
                    pd.Adoc = pagamento.BankReference;

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

            var trfb = _phcRepository.GetUTrfb(paymentQueue.BatchId);
            if (trfb != null)
            {
                trfb.Rdata = processingDate;
            }

            _genericPaymentRepository.SaveChanges();
            _genericPHCRepository.SaveChanges();

        }

        private static string Truncar(string? s, int max)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;
            return s.Length <= max ? s : s.Substring(0, max);
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

            var isErro = estado == "Erro";
            var isPorCorrigir = isErro && !string.IsNullOrWhiteSpace(descricao)
                && descricao.Contains("Por Corrigir", StringComparison.OrdinalIgnoreCase);

            var estadoPersist = (isErro && !isPorCorrigir) ? "Por Enviar" : estado;
            var descricaoPersist = (isErro && !isPorCorrigir) ? "Pagamento aprovado e por enviar" : descricao;

            foreach (var payment in paymentsToUpdate)
            {
                payment.Estado = estadoPersist;
                payment.Descricao = descricaoPersist;
                payment.Usrdata = DateTime.Now;
            }

            foreach (var paymentQueue in paymentQueueToUpdate)
            {
                paymentQueue.Estado = estadoPersist;
                paymentQueue.Descricao = descricaoPersist;
                paymentQueue.Usrdata = DateTime.Now;
            }

            foreach (var wspayment in wspaymentToUpdate)
            {
                wspayment.Estado = estadoPersist;
                wspayment.Descricao = descricaoPersist;
                wspayment.Usrdata = DateTime.Now;

                _genericPHCRepository.Update(wspayment);
                _genericPHCRepository.SaveChanges();
            }

            // Eliminar da Queue apenas quando o envio foi bem sucedido
            // ou quando o erro é "Por Corrigir" (parametrizado)
            if (estadoPersist == "Por processar" || estadoPersist == "Sucesso" || isPorCorrigir)
            {
                foreach (var paymentQueue in paymentQueueToUpdate)
                {
                    _genericPaymentRepository.Delete(paymentQueue);
                    Debug.Print($"Pagamento com BatchId {u2BPayments.payment.BatchId} eliminado da Queue. Estado: {estadoPersist}");
                }
            }

            _genericPaymentRepository.SaveChanges();
        }

        /// <summary>
        /// Insere histórico de alteração de estado do pagamento na tabela u_2b_payments_hs.
        /// Garante rastreamento completo e sequencial de todas as alterações de estado.
        /// </summary>
        /// <param name="transactionId">ID da transação (obrigatório)</param>
        /// <param name="batchId">ID do lote (obrigatório)</param>
        /// <param name="creditAccount">Conta de crédito/NIB destino (obrigatório)</param>
        /// <param name="beneficiaryName">Nome do beneficiário</param>
        /// <param name="transactionDescription">Descrição da transação</param>
        /// <param name="currency">Moeda</param>
        /// <param name="amount">Valor do pagamento</param>
        /// <param name="bankReference">Referência bancária</param>
        /// <param name="debitAccount">Conta de débito/origem</param>
        /// <param name="oristamp">Stamp do documento de origem (PO/PD/OW/TB)</param>
        /// <param name="tabela">Tabela de origem (PO/PD/OW/TB)</param>
        /// <param name="canal">Código do canal/banco (105-NEDBANK, 106-BCI, 107-BIM, 108-FCB, 109-MOZA)</param>
        /// <param name="canalNome">Nome do canal/banco</param>
        /// <param name="statusCodeHs">Código do estado retornado pelo banco</param>
        /// <param name="statusDescriptionHs">Descrição do estado retornado pelo banco</param>
        /// <param name="statusCode">Código do estado interno do pagamento</param>
        /// <param name="statusDescription">Descrição do estado interno do pagamento</param>
        public void insere2bHistorico(
            string transactionId,
            string batchId,
            string creditAccount,
            string beneficiaryName,
            string transactionDescription,
            string currency,
            decimal amount,
            string bankReference,
            string debitAccount,
            string oristamp,
            string tabela,
            int? canal,
            string? canalNome,
            string statusCodeHs,
            string statusDescriptionHs,
            string statusCode,
            string statusDescription)
        {
            // Validações de campos obrigatórios
            if (string.IsNullOrWhiteSpace(transactionId))
            {
                Debug.Print("ERRO: TransactionId é obrigatório para inserir histórico");
                return;
            }

            if (string.IsNullOrWhiteSpace(batchId))
            {
                Debug.Print("ERRO: BatchId é obrigatório para inserir histórico");
                return;
            }

            Debug.Print($"Insere histórico - TransactionId: {transactionId}, BatchId: {batchId}, Canal: {canalNome ?? "N/A"}, Status: {statusCode}");

            string stampHs = 25.UseThisSizeForStamp();

            if (_genericPaymentRepository == null)
            {
                throw new InvalidOperationException("O repositório foi descartado antes da chamada.");
            }

            U2bPaymentsHs u2Bhistoric = new U2bPaymentsHs
            {
                TransactionId = transactionId,
                CreditAccount = creditAccount,
                BeneficiaryName = beneficiaryName ?? "",
                TransactionDescription = transactionDescription ?? "",
                Currency = currency ?? "",
                Amount = amount,
                BankReference = bankReference ?? "",
                StatusCode = statusCode ?? "",
                StatusDescription = statusDescription ?? "",
                BatchId = batchId,
                ProcessingDate = DateTime.Now,
                StatusCodeHs = statusCodeHs ?? "",
                StatusDescriptionHs = statusDescriptionHs ?? "",
                U2bPaymentsHsstamp = stampHs,
                DebitAccount = debitAccount ?? "",
                Ousrdata = DateTime.Now,
                Oristamp = oristamp ?? "",
                Tabela = tabela,
                Canal = canal,
                CanalNome = canalNome
            };

            try
            {
                _genericPaymentRepository.Add(u2Bhistoric);
                _genericPaymentRepository.SaveChanges();
                Debug.Print($"Histórico inserido com sucesso - Stamp: {stampHs}");
            }
            catch (ObjectDisposedException ex)
            {
                Debug.Print("Erro ao inserir histórico: " + ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                Debug.Print($"Erro inesperado ao inserir histórico: {ex.Message}");
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

            responseDto.Id = responseID.ToString();

            return responseDto;
        }

    }

}
