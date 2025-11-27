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
using System.Globalization;
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

        #endregion


        #region Funções de Recebimentos


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
                            lstRespostas.Add(new RespostaDTO(payment.IdPagamento, WebTransactionCodes.INVALIDREFERENCE, payment.Entidade.ToString(), payment.IdPagamento));
                            continue;
                        }

                        bool duplicated = await _paymentRespository.PaymentExistsAsync(payment);
                        if (duplicated)
                        {
                            lstRespostas.Add(new RespostaDTO(payment.IdPagamento, WebTransactionCodes.DUPLICATEDPAYMENT, payment.IdPagamento));
                            continue;
                        }

                        //Adicionar Pagamento
                        await _paymentRespository.AddPayment(payment);
                        lstRespostas.Add(new RespostaDTO(payment.IdPagamento, WebTransactionCodes.SUCCESS, payment.IdPagamento));
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
                logHelper.generateLogJB(response, "ProcessarRecebimentos" + Guid.NewGuid(), "PaymentService.ProcessarRecebimentosAsync", ex.Message);
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

                Ft ft = await _phcRepository.GetFtByRef(u2bpayments.Referencia);
                Cl cl = await _phcRepository.getClienteByNo(ft.No);

                List<Cc> contacorrente = new List<Cc>();
                contacorrente = await _phcRepository.getContaCorrenteByStamp(ft.Ftstamp);

                Debug.Print("Nº do cliente " + cl.No.ToString());
                Debug.Print($"Conta corrente: {JsonConvert.SerializeObject(contacorrente)}");

                decimal saldo = 0;
                decimal pagamentoVal = u2bpayments.Valor;

                if (contacorrente.Any())
                    saldo = contacorrente.Sum(cc => (cc.Deb - cc.Debf));

                Debug.Print("COUNT DO CC " + contacorrente.Count().ToString());
                if (pagamentoVal <= saldo)
                {
                    Debug.Print(" APENAS CRIA RECIBO CC");
                    criarReciboCC(cl,
                                    pagamentoVal,
                                    recibos,
                                    contacorrente,
                                    u2bpayments
                                    );
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


                _paymentRespository.updateTransactionStatus(u2bpayments);

                ResponseDTO saveChangesresponse = await _genericPHCRepository.SaveChangesRespDTO();

                Debug.Print("Facturacao Save Changes Result " + saveChangesresponse.ToString());

                if (saveChangesresponse.response.cod != "0000")
                    throw new GeneralException(saveChangesresponse);

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
            var stamp = KeysExtension.UseThisSizeForStamp(25);
            string restamp = stamp;
            var rno = _phcRepository.getMaxRecibo();

            var config = _phcRepository.getConfiguracaoRecibo();
            Debug.Print("Configs" + config.ToString());
            Re re = new Re(
                    restamp: stamp,
                    ccusto: cl.Ccusto,
                    chdata: data == default ? DateTime.Now.Date : data.Date,
                    contado: 0,
                    //contado: config.UNoconta,
                    etotal: pagamento,
                    etotow: 0,
                    fref: cl.Fref,
                    local: cl.Local,
                    memissao: "PTE",
                    morada: cl.Morada,
                    ncont: cl.Ncont,
                    ndoc: config.Ndoc,
                    nmdoc: config.Nmdoc,
                    no: cl.No,
                    nome: cl.Nome,
                    olcodigo: "R00002",
                    ollocal: "",
                    //ollocal: config.UDnoconta,
                    ousrdata: DateTime.Now.Date,
                    usrdata: DateTime.Now.Date,
                    ousrhora: DateTime.Now.ToString("HH:MM:SS"),
                    usrhora: DateTime.Now.ToString("HH:MM:SS"),
                    ousrinis: "FIPAGONLINEPAYMENTSAPI",
                    usrinis: "FIPAGONLINEPAYMENTSAPI",
                    process: true,
                    rdata: data == default ? DateTime.Now.Date : data.Date,
                    reano: data == default ? DateTime.Now.Year : data.Year,
                    rno: rno,
                    segmento: cl.Segmento,
                    telocal: "B",
                    total: pagamento,
                    totow: 0,
                    procdata: data == default ? DateTime.Now.Date : data.Date,
                    moeda: "MT",
                    UTransid: u2BPayments.IdPagamento,
                    UEntps: u2BPayments.Entidade,
                    URefps: u2BPayments.Referencia

                    //uAgid: agid,
                    //uAgnome: agnome,
                    //uAgcodigo: agcodigo
                    );

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

                _phcRepository.addLinhasRecibo(
                    new Rl(
                        restamp: stamp,
                        rlstamp: stamprl,
                        ccstamp: cc.Ccstamp,
                        cdesc: cc.Cmdesc,
                        cm: cc.Cm,
                        datalc: cc.Datalc,
                        dataven: cc.Dataven,
                        enaval: cc.Deb - cc.Debf,
                        eval: cc.Deb - cc.Debf,
                        escrec: vPagar,
                        escval: cc.Deb - cc.Debf,
                        erec: vPagar,
                        evori: cc.Deb,
                        moeda: _phcRepository.getMoeda(),
                        val: cc.Deb - cc.Debf,
                        rec: vPagar,

                        ndoc: config.Ndoc,
                        nrdoc: cc.Nrdoc,
                        process: true,

                        rno: rno,
                        rdata: data == default ? DateTime.Now.Date : data.Date,
                        ousrdata: DateTime.Now.Date,
                        usrdata: DateTime.Now.Date,
                        ousrhora: $"{DateTime.Now.Hour}:{DateTime.Now.Minute}",
                        usrhora: $"{DateTime.Now.Hour}:{DateTime.Now.Minute}",
                        ousrinis: "ONLINEPAYMENTSAPI",
                        usrinis: "ONLINEPAYMENTSAPI"

                        ));
                pagamento -= vPagar;

            }

            Debug.Print("totalFacturaCorrente " + totalFacturaCorrente.ToString());
            recibos.Add(
                new ReciboAux(
                    stamp: stamp,
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

            logHelper.generateLogJB(new ResponseDTO(), paymentHeader.BatchId, "PaymentService.actualizarPagamentos", json1);
            //
            List<string> listas = new List<string>
            {
                "im225102157370704000002","MLQ25100264923.949469947","DNF25081352576.6520000_1",
                "DNF25091159551.2890000_1",
                "DNF25091146226.0610000_2",
                "DNF25081339161.417000001_1","DNF25081352913.6760000_1",
                "DNF25081352576.652000002_1","DNF25081339161.4170000_1",
                "DNF25081353369.858000002_1","DNF25081342632.4280000_1",
                "DNF25081342632.428000002_1", "NFE25081945556.1843020_1","DNF25081353369.8580000_1",
            };
            if (listas.Contains(paymentHeader.BatchId))
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

                insere2bHistorico("", pagamento.payment.BatchId, pagamento.payment.BatchId, bimResponse.response.cod, bimResponse.response.codDesc, "", "");

                Debug.Print("insere2bHistorico");

                switch (bimResponse.response.cod)
                {

                    case "0" or "0000" or "0011" or "0404":
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

        async Task FcbProcessing(List<PaymentsQueue> pagamentos)
        {

            foreach (var pagamento in pagamentos)
            {
                try
                {
                    FcbAPI fcbRepository = new FcbAPI();
                    FcbPaymentDTO fcbPayment = apiHelper.ConvertPaymentToFcb(pagamento.payment);
                    Debug.Print($"FCB {JsonConvert.SerializeObject(fcbPayment)}");
                    FcbResponseDTO fcbResponseDTO = await fcbRepository.LoadPaymentsAsync(fcbPayment);

                    ResponseDTO fcbResponse = routeMapper.mapLoadPaymentResponse(108, fcbResponseDTO);

                    insere2bHistorico("", pagamento.payment.BatchId, pagamento.payment.BatchId, fcbResponse.response.cod, fcbResponse.response.codDesc, "", "");

                    switch (fcbResponse.response.cod)
                    {
                        case "0000":
                            actualizarEstadoDoPagamento(pagamento, "Por processar", "Pagamento enviado por processar");
                            break;

                        default:
                            actualizarEstadoDoPagamento(pagamento, "Por corrigir", fcbResponse.response.codDesc);
                            break;
                    }

                    logHelper.generateLogJB(fcbResponse, pagamento.payment.BatchId, "PaymentService.processarPagamento - FCB", pagamento.payment);
                }
                catch (Exception ex)
                {
                    var response = new ResponseDTO(new ResponseCodesDTO("0007", "Erro no processamento com o FCB"), ex.Message, null);
                    actualizarEstadoDoPagamento(pagamento, "Por corrigir", ex.Message);
                    logHelper.generateLogJB(response, pagamento.payment.BatchId, "PaymentService.processarPagamento - FCB", pagamento.payment);
                }
            }
        }


        async Task BCIProcessing(List<PaymentsQueue> pagamentos, bool checkPayments)
        {

            foreach (var pagamento in pagamentos)
            {

                insere2bHistorico("", pagamento.payment.BatchId, pagamento.payment.BatchId, "bciResponse.response.cod", "bciResponse.response.codDesc", "", "");
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

        async Task MozaProcessing(List<PaymentsQueue> pagamentos)
        {

            foreach (var pagamento in pagamentos)
            {

                insere2bHistorico("", pagamento.payment.BatchId, pagamento.payment.BatchId, "MozaProcessing.response.cod", "MozaProcessing.response.codDesc", "", "");
                BCIAPI bciRepository = new BCIAPI();

                PaymentCamelCase paymentCamel = apiHelper.ConvertPaymentToCamelCase(pagamento.payment);

                Debug.Print($"paymentCamel {paymentCamel}");

                BCIResponseDTO bciResponseDTO = new BCIResponseDTO();

                bciResponseDTO = bciRepository.loadPayments(paymentCamel);

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


                logHelper.generateLogJB(bciResponse, pagamento.payment.BatchId, "PaymentService.processarPagamento - MOZA", paymentCamel);

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

            responseDto.Id = responseID.ToString();

            return responseDto;
        }

    }

}
