using CFM_PAYMENTSWS.Domains.Interface;
using CFM_PAYMENTSWS.Domains.Models;
using CFM_PAYMENTSWS.DTOs;
using CFM_PAYMENTSWS.Extensions;
using CFM_PAYMENTSWS.Helper;
using CFM_PAYMENTSWS.Persistence.Contexts;
using CFM_PAYMENTSWS.Providers.Nedbank.DTOs;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Transactions;
using Z.EntityFramework.Plus;
using static System.Net.WebRequestMethods;

namespace CFM_PAYMENTSWS.Persistence.Repositories
{
    public class PaymentRepository<TContext> : IPaymentRepository<TContext> where TContext : DbContext
    {

        private readonly ConversionExtension conversionExtension = new ConversionExtension();
        private readonly TContext _context;
        EncryptionHelper encryptionHelper = new EncryptionHelper();

        public PaymentRepository(TContext context)
        {
            _context = context;
        }

        public List<U2bPaymentsQueue> GetPagamentosEmFila(string estado, decimal canal)
        {

            var result = _context.Set<U2bPaymentsQueue>().
                AsNoTracking().
                Where(pq => pq.Estado == estado
                    && pq.Canal == canal)
                .ToList()
                .Select(res => new U2bPaymentsQueue
                {
                    Canal = res.Canal,
                    Destino = encryptionHelper.DecryptText(res.Destino, res.Keystamp),
                    Valor = res.Valor,
                    TransactionId = encryptionHelper.DecryptText(res.TransactionId, res.Keystamp),
                    U2bPaymentsQueuestamp = res.U2bPaymentsQueuestamp,
                    Lordem = res.Lordem
                });


            return result.ToList();
        }


        public void actualizarEstadoDoPagamento(U2bPaymentsQueue u2BPayments, ResponseDTO responseDTO)
        {
            if (responseDTO.response.cod != "0000")
            {
                var paymentStatus = _context.Set<U2bPayments>().Where(u2bpayments => u2bpayments.U2bPaymentsstamp == u2BPayments.U2bPaymentsQueuestamp).FirstOrDefault();

                if (paymentStatus.Estado != "SUCESSO")
                {

                    _context.Set<U2bPayments>().Where(u2bpayments =>
                    u2bpayments.U2bPaymentsstamp == u2BPayments.U2bPaymentsQueuestamp)
                    .Update(x => new U2bPayments()
                    {
                        Processado = true,
                        Estado = (responseDTO.response.cod == "0000" ? "SUCESSO" : "ERRO"),
                        Descricao = (responseDTO.response.cod == "0000" ? "Sucesso" : "Erro ao processar pagamento")
                    });

                }

                return;

            }


            Debug.Print("Update payment ");
            //var payment = _context.Set<U2bPayments>().Where(upayment => upayment.U2bPaymentsstamp == u2BPayments.U2bPaymentsQueuestamp).FirstOrDefault();

            var payment = GetPaymentByStamp(u2BPayments.U2bPaymentsQueuestamp);
            Debug.Print($"Update payment2   {JsonConvert.SerializeObject(payment)}");

            payment.Processado = true;
            payment.Estado = "SUCESSO";
            payment.Descricao = "Sucesso";

            var paymentQueue = _context.Set<U2bPaymentsQueue>().Where(upayment => upayment.U2bPaymentsQueuestamp == u2BPayments.U2bPaymentsQueuestamp).FirstOrDefault();
            Debug.Print($"Update payment2   {JsonConvert.SerializeObject(paymentQueue)}");

            _context.Remove(paymentQueue);
            _context.SaveChanges();
        }

        async Task UpdateCCusto()
        {

            // 1) UPDATE prévio, fora de transacção ambiente
            using (var scope = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled))
            {
                const string sql =
                    @"UPDATE q
                             SET q.ccusto = p.ccusto
                          FROM dbo.u_2b_paymentsQueue AS q
                          JOIN [nacala].[E14E105BD_CFM].dbo.po AS p
                            ON p.postamp COLLATE DATABASE_DEFAULT = q.oristamp
                          WHERE q.tabela = @tabela
                            AND NULLIF(LTRIM(RTRIM(q.ccusto)), '') IS NULL;";

                var pTabela = new SqlParameter("@tabela", "PO");
                _context.Database.SetCommandTimeout(60);
                await _context.Database.ExecuteSqlRawAsync(sql, pTabela);

                scope.Complete();
            }
            _context.ChangeTracker.Clear();

        }

        public async Task<List<PaymentsQueue>> GetPagamentQueue(string estado, decimal canal)
        {

            Debug.Print("Get Pagamento queue");
            try
            {
                await UpdateCCusto();

                var pagamentos = await _context.Set<U2bPaymentsQueue>()
                    .AsNoTracking()
                    .Where(payment => payment.Estado == estado && payment.Canal == canal && payment.Ccusto.Trim() != "")
                    .GroupBy(payment => payment.BatchId)
                    .Select(group => new PaymentsQueue
                    {
                        canal = group.First().Canal,
                        payment = new Payment
                        {

                            BatchId = group.Key.Trim(),
                            Description = (group.First().TransactionDescription == "") ? $"Transf. " : group.First().TransactionDescription,
                            //ProcessingDate = (DateTime)((group.First().ProcessingDate < DateTime.Now) ? DateTime.Now : group.First().ProcessingDate),
                            ProcessingDate = DateTime.Now,
                            DebitAccount = group.First().Origem,
                            initgPtyCode = GetAuxCamposEntityCode(group.First().Canal, group.First().Ccusto, group.First().Oristamp),
                            BatchBooking = GetAuxCamposBatchBooking(group.First().Tabela, group.First().Canal),
                            PaymentRecords = group.Select(paymentRecord => new PaymentRecords
                            {
                                Amount = paymentRecord.Valor,
                                Currency = paymentRecord.Moeda,
                                TransactionDescription = (paymentRecord.TransactionDescription == "" ?
                                        $"Transf. {encryptionHelper.DecryptText(paymentRecord.BeneficiaryName, paymentRecord.Keystamp)}-{encryptionHelper.DecryptText(paymentRecord.TransactionId, paymentRecord.Keystamp)}" : paymentRecord.TransactionDescription),
                                BeneficiaryName = encryptionHelper.DecryptText(paymentRecord.BeneficiaryName, paymentRecord.Keystamp),
                                TransactionId = encryptionHelper.DecryptText(paymentRecord.TransactionId, paymentRecord.Keystamp),
                                CreditAccount = encryptionHelper.DecryptText(paymentRecord.Destino, paymentRecord.Keystamp),
                                BeneficiaryEmail = string.IsNullOrEmpty(encryptionHelper.DecryptText(paymentRecord.Emailf, paymentRecord.Keystamp))
                                                        ? "NA"
                                                        : FormatSpecialChars(encryptionHelper.DecryptText(paymentRecord.Emailf, paymentRecord.Keystamp))

                            }).ToList()
                        }
                    })
                    .ToListAsync();



                // Imprimir o JSON
                string json = JsonConvert.SerializeObject(pagamentos, Formatting.Indented);
                //Debug.Print("JSON dos pagamentos no estado:\n" + estado + json);

                //Debug.Print("Imprimie todos pagamentos" + pagamentos.ToString());
                return pagamentos;
            }
            catch (Exception ex)
            {
                // Lida com a exceção aqui
                Debug.Print($"EXCEPCAO TEST GROUP {ex.Message} INNER {ex.InnerException} STACK TRACE {ex.StackTrace}");
                throw; // Re-lança a exceção para que a chamada do método possa lidar com ela, se necessário
            }
        }

        public U2bPayments GetPaymentByStamp(string u2bPaymentsStamp)
        {
            var payment = _context.Set<U2bPayments>()
                .FirstOrDefault(upayment => upayment.U2bPaymentsstamp == u2bPaymentsStamp);
            return payment;
        }

        static string FormatSpecialChars(string campo)
        {
            if (string.IsNullOrEmpty(campo))
                return "";

            if (campo.Contains('/'))
                return campo.Split('/')[0];

            return campo;
        }

        private static string? GetAuxCamposBatchBooking(string tabela, int provider)
        {
            return provider switch
            {
                106 => tabela switch
                {
                    "TB" => "Salários",
                    "PR" => "Salários",
                    _ => "Fornecedores"
                },
                //0 – fornecedor, 1 salarios
                107 => tabela switch
                {
                    "TB" => "1",
                    "PR" => "1",
                    _ => "0"
                },
                _ => null
            };

        }

        private static string? GetAuxCamposEntityCode(int provider, string ccusto, string? oristamp)
        {

            if (provider == 106)
            {
                if (string.IsNullOrEmpty(ccusto))
                {
                    if (string.IsNullOrEmpty(ccusto))
                        return "84d193aa-a6fc-4ada-b367-6b94449f3502";
                }
                string prefix = ccusto[..1];

                return prefix switch
                {
                    "9" => "84d193aa-a6fc-4ada-b367-6b94449f3502",
                    "1" => "eb84bbe7-7e64-4113-ab95-8a98e9227090",
                    "2" => "5c67ce71-65ac-4007-a070-74fcfafd864d",
                    "3" => "1262cba6-53e6-4e43-912e-3f8fcb9373a1",
                    _ => "84d193aa-a6fc-4ada-b367-6b94449f3502"
                };
            }

            return provider switch
            {
                107 => "CFM",
                _ => null
            };
        }

        public List<UProvider> getProviderData(decimal providerCode)
        {
            return _context.Set<UProvider>()
                        .Where(provider => provider.codigo == providerCode).ToList();
        }


        public List<UProvider> getProviderByGroup(decimal providerCode, string grupo)
        {
            return _context.Set<UProvider>()
                    .Where(provider => provider.codigo == providerCode && provider.grupo == grupo).ToList();
        }

        public bool verificaBatchId(string batchId)
        {
            // Retorna true se o batchId existe na tabela, caso contrário, retorna false
            return _context.Set<U2bPaymentsQueue>()
                        .Any(p => p.BatchId == batchId);
        }

        public U2bPayments GetPayment(string transactionId, string batchId)
        {
            return _context.Set<U2bPayments>()
                        .Where(upayment => upayment.Transactionid == transactionId && upayment.BatchId == batchId)
                        .FirstOrDefault();
        }


        public List<int?> GetCanais_UPaymentQueue()
        {

            return _context.Set<U2bPayments>()
                      .Select(u => new { u.Canal })
                      .Distinct()
                      .ToList()
                      .Select(x => (x.Canal))
                      .ToList();
        }


        public List<U2bPayments> GetPaymentsBatchId(string batchId)
        {
            return _context.Set<U2bPayments>()
                        .Where(upayment => upayment.BatchId == batchId).ToList();
        }

        /*
        public List<U2bPayments> GetPaymentsBatchId2(string batchId)
        {
            if (_isTestEnvironment)
            {
                // Use the test table U2bPaymentsTs
                return _context.Set<U2bPaymentsTs>() // Assuming U2bPaymentsTs is another entity
                               .Where(upayment => upayment.BatchId == batchId)
                               .ToList();
            }
            else
            {
                // Use the production table U2bPayments
                return _context.Set<U2bPayments>()
                               .Where(upayment => upayment.BatchId == batchId)
                               .ToList();
            }
        }
        */

        public List<U2bPaymentsQueue> GetPaymentsQueueBatchId(string batchId)
        {
            return _context.Set<U2bPaymentsQueue>()
                          .Where(u2BPaymentsQueue => u2BPaymentsQueue.BatchId == batchId)
                          .ToList();
        }




    }
}
