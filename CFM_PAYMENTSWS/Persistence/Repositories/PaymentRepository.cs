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

namespace CFM_PAYMENTSWS.Persistence.Repositories
{
    public class PaymentRepository<TContext> : IPaymentRepository<TContext> where TContext : DbContext
    {

        private readonly ConversionExtension conversionExtension = new ConversionExtension();
        private readonly TContext _context;

        public PaymentRepository(TContext context)
        {
            _context = context;
        }

        public List<PaymentsQueue> GetPagamentQueue(string estado)
        {
            EncryptionHelper encryptionHelper= new EncryptionHelper();


            try
            {
                var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json");



                var config = configuration.Build();
                var connString = "";
                connString=config.GetConnectionString("ConnStr");

                //var pagamentos = _wSCTX.U2BPaymentsQueue
                var pagamentos = _context.Set<U2bPaymentsQueueTs>()
                    .Where(payment => payment.estado == estado)
                    .GroupBy(payment => payment.BatchId)
                    .Select(group => new PaymentsQueue
                    {
                        canal = group.First().canal,
                        payment = new Payment
                        {

                            BatchId = group.Key,
                            Description = group.First().description,
                            ProcessingDate = (group.First().processingDate < DateTime.Now) ? DateTime.Now : group.First().processingDate,
                            DebitAccount = group.First().origem,

                            PaymentRecords = group.Select(paymentRecord => new PaymentRecords
                            {
                                Amount = paymentRecord.valor,
                                Currency = paymentRecord.moeda,
                                TransactionDescription = paymentRecord.transactionDescription,
                                BeneficiaryName = encryptionHelper.DecryptText(paymentRecord.beneficiaryName, paymentRecord.keystamp),
                                TransactionId = encryptionHelper.DecryptText(paymentRecord.transactionId, paymentRecord.keystamp),
                                //CreditAccount = paymentRecord.destino,
                                CreditAccount = encryptionHelper.DecryptText(paymentRecord.destino, paymentRecord.keystamp),
                                BeneficiaryEmail = encryptionHelper.DecryptText(paymentRecord.BeneficiaryEmail, paymentRecord.keystamp)

                            }).ToList()
                        }
                    })
                    .ToList();


                string json = JsonConvert.SerializeObject(pagamentos, Formatting.Indented);

                // Imprimir o JSON
                Debug.Print("JSON dos pagamentos no estado:\n"+estado + json);

                Debug.Print("Imprimie todos pagamentos" + pagamentos.ToString());
                return pagamentos;
            }
            catch (Exception ex)
            {
                // Lida com a exceção aqui
                Debug.Print($"EXCEPCAO TEST GROUP {ex.Message} INNER {ex.InnerException} STACK TRACE {ex.StackTrace}");
                throw; // Re-lança a exceção para que a chamada do método possa lidar com ela, se necessário
            }
        }



        public bool verificaBatchId(string batchId)
        {
            // Retorna true se o batchId existe na tabela, caso contrário, retorna false
            return _context.Set<U2bPaymentsQueueTs>()
                        .Any(p => p.BatchId == batchId);
        }

        public U2bPaymentsTs GetPayment(string transactionId, string batchId)
        {
            return _context.Set<U2bPaymentsTs>()
                        .Where(upayment => upayment.transactionId == transactionId && upayment.BatchId == batchId)
                        .FirstOrDefault();
        }

        public List<U2bPaymentsTs> GetPaymentsBatchId(string batchId)
        {
            return _context.Set<U2bPaymentsTs>()
                        .Where(upayment => upayment.BatchId == batchId).ToList();
        }

        public List<U2bPaymentsQueueTs> GetPaymentsQueueBatchId( string batchId)
        {
            return _context.Set<U2bPaymentsQueueTs>()
                          .Where(u2BPaymentsQueue => u2BPaymentsQueue.BatchId == batchId)
                          .ToList();
        }



    }
}
