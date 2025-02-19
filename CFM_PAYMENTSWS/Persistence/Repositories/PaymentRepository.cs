using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using CFM_PAYMENTSWS.Domains.Interface;
using CFM_PAYMENTSWS.Domains.Models;
using CFM_PAYMENTSWS.DTOs;
using CFM_PAYMENTSWS.Extensions;
using CFM_PAYMENTSWS.Helper;
using CFM_PAYMENTSWS.Persistence.Contexts;
using CFM_PAYMENTSWS.Providers.Nedbank.DTOs;
using System.Diagnostics;
using System.Security.Cryptography;
using static System.Net.WebRequestMethods;
using Microsoft.Data.SqlClient;
using Z.EntityFramework.Plus;

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

        public List<PaymentsQueue> GetPagamentQueue(string estado, decimal canal)
        {


            Debug.Print("Get Pagamento queue");
            try
            {

                var pagamentos = _context.Set<U2bPaymentsQueue>()
                    .Where(payment => payment.Estado == estado && payment.Canal == canal)
                    .GroupBy(payment => payment.BatchId)
                    .Select(group => new PaymentsQueue
                    {
                        canal = group.First().Canal,
                        payment = new Payment
                        {

                            BatchId = group.Key.Trim(),
                            Description = group.First().Description,
                            ProcessingDate = (DateTime)((group.First().ProcessingDate < DateTime.Now) ? DateTime.Now : group.First().ProcessingDate),
                            DebitAccount = group.First().Origem,
                            initgPtyCode = GetAuxCamposEntityCode(group.First().Canal),
                            BatchBooking = GetAuxCamposBatchBooking(group.First().Tabela, group.First().Canal),
                            PaymentRecords = group.Select(paymentRecord => new PaymentRecords
                            {
                                Amount = paymentRecord.Valor,
                                Currency = paymentRecord.Moeda,
                                TransactionDescription = paymentRecord.TransactionDescription,
                                BeneficiaryName = encryptionHelper.DecryptText(paymentRecord.BeneficiaryName, paymentRecord.Keystamp),
                                TransactionId = encryptionHelper.DecryptText(paymentRecord.TransactionId, paymentRecord.Keystamp),
                                CreditAccount = encryptionHelper.DecryptText(paymentRecord.Destino, paymentRecord.Keystamp),
                                BeneficiaryEmail = string.IsNullOrEmpty(encryptionHelper.DecryptText(paymentRecord.Emailf, paymentRecord.Keystamp))
                                                        ? "N/A"
                                                        : encryptionHelper.DecryptText(paymentRecord.Emailf, paymentRecord.Keystamp)

                            }).ToList()
                        }
                    })
                    .ToList();



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

        private static string? GetAuxCamposBatchBooking(string tabela, int provider)
        {
            return provider == 106 ? tabela switch
            {
                "TB" => "Salários",
                _ => "Fornecedores"
            } : null;
        }

        private static string? GetAuxCamposEntityCode(int provider)
        {
            return provider switch
            {
                106 => "3a45f122-4d6c-430e-ac6a-90079cb3831d",
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
