using CFM_PAYMENTSWS.Domains.Models;
using CFM_PAYMENTSWS.DTOs;

namespace CFM_PAYMENTSWS.Helper
{
    public class APIHelper
    {

        public PaymentCamelCase ConvertPaymentToCamelCase(Payment payment)
        {
            if (payment == null) throw new ArgumentNullException(nameof(payment));

            return new PaymentCamelCase
            {
                BatchId = payment.BatchId,
                Description = payment.Description.Length > 40
                    ? payment.Description[..40]
                    : payment.Description,
                ProcessingDate = payment.ProcessingDate.ToString("yyyy-MM-dd"),
                DebitAccount = payment.DebitAccount,
                initgPtyCode = payment.initgPtyCode,
                BatchBooking = payment.BatchBooking,
                PaymentRecords = payment.PaymentRecords?.Select(pr => new PaymentRecordsCamelCase
                {
                    TransactionId = pr.TransactionId,
                    CreditAccount = pr.CreditAccount,
                    BeneficiaryName = pr.BeneficiaryName,
                    TransactionDescription = pr.TransactionDescription.Length > 40
                        ? pr.TransactionDescription[..40]
                        : pr.TransactionDescription,
                    Currency = pr.Currency,
                    Amount = pr.Amount,
                    BeneficiaryEmail = pr.BeneficiaryEmail
                }).ToList()
            };
        }

        public Paymentv1_5 ConvertPaymentToV1_5(Payment payment)
        {
            if (payment == null) throw new ArgumentNullException(nameof(payment));

            return new Paymentv1_5
            {
                BatchId = payment.BatchId,
                Description = payment.Description,
                ProcessingDate = payment.ProcessingDate.ToString("yyyy-MM-ddThh:mm:ss"),
                DebitAccount = payment.DebitAccount,
                initgPty_Code = payment.initgPtyCode,
                BatchBooking = int.Parse(payment.BatchBooking),
                PaymentRecords = payment.PaymentRecords?.Select(pr => new PaymentRecords
                {
                    TransactionId = pr.TransactionId,
                    CreditAccount = pr.CreditAccount,
                    BeneficiaryName = pr.BeneficiaryName,
                    TransactionDescription = pr.TransactionDescription,
                    Currency = pr.Currency,
                    Amount = pr.Amount,
                    BeneficiaryEmail = int.Parse(payment.BatchBooking) == 1 ? null : pr.BeneficiaryEmail
                }).ToList()
            };
        }


        public API getApiEntity(string entity, string operationCode)
        {
            var configuration = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile($"appsettings.json");

            var config = configuration.Build();
            API[] configuracoes = config.GetSection("APIS").Get<API[]>();

            var apiEntityData = configuracoes.Where(apiEntity => apiEntity.entity == entity).FirstOrDefault();

            if (apiEntityData != null)
            {

                var endpointData = apiEntityData.endpoints.Where(endpoint => endpoint.operationCode == operationCode);

                if (endpointData != null)
                {
                    apiEntityData.status = "1";
                    return apiEntityData;

                }

                return new API { status = "0", message = "Não foi encontrado o endpoint com o código indicado para a respectiva entidade." };


            }

            return new API { status = "0", message = "Os dados da API da entidade indicada não foram encontrados" };
        }
    }
}
