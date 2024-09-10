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

            Debug.Print("Get Pagamento queue");
            try
            {
                /*
                var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json");

                var config = configuration.Build();
                var connString = "";
                connString=config.GetConnectionString("ConnStr");
                */

                var pagamentos = _context.Set<U2bPaymentsQueue>()
                    .Where(payment => payment.Estado == estado)
                    .GroupBy(payment => payment.BatchId)
                    .Select(group => new PaymentsQueue
                    {
                        canal = group.First().Canal,
                        payment = new Payment
                        {

                            BatchId = group.Key,
                            Description = group.First().Description,
                            ProcessingDate = (DateTime)((group.First().ProcessingDate < DateTime.Now) ? DateTime.Now : group.First().ProcessingDate),
                            DebitAccount = group.First().Origem,

                            PaymentRecords = group.Select(paymentRecord => new PaymentRecords
                            {
                                Amount = paymentRecord.Valor,
                                Currency = paymentRecord.Moeda,
                                TransactionDescription = paymentRecord.TransactionDescription,
                                BeneficiaryName = encryptionHelper.DecryptText(paymentRecord.BeneficiaryName, paymentRecord.Keystamp),
                                TransactionId = encryptionHelper.DecryptText(paymentRecord.TransactionId, paymentRecord.Keystamp),
                                CreditAccount = encryptionHelper.DecryptText(paymentRecord.Destino, paymentRecord.Keystamp),
                                BeneficiaryEmail = encryptionHelper.DecryptText(paymentRecord.Emailf, paymentRecord.Keystamp) ?? ""

                            }).ToList()
                        }
                    })
                    .ToList();



                // Imprimir o JSON
                Debug.Print("Estadosss");
                string json = JsonConvert.SerializeObject(pagamentos, Formatting.Indented);
                Debug.Print("JSON dos pagamentos no estado:\n" + estado + json);

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

        public List<U2bPayments> GetPaymentsBatchId(string batchId)
        {
            return _context.Set<U2bPayments>()
                        .Where(upayment => upayment.BatchId == batchId).ToList();
        }

        public List<U2bPaymentsQueue> GetPaymentsQueueBatchId( string batchId)
        {
            return _context.Set<U2bPaymentsQueue>()
                          .Where(u2BPaymentsQueue => u2BPaymentsQueue.BatchId == batchId)
                          .ToList();
        }

        public Suliame getUserEmail(int userno)
        {
            return _context.Set<Suliame>()
                    .Where(provider => provider.Userno== userno)
                    .FirstOrDefault();
        }


    }
}
