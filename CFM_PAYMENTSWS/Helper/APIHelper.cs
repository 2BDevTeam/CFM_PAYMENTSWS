using System.Collections.Generic;
using CFM_PAYMENTSWS.Domains.Models;
using CFM_PAYMENTSWS.DTOs;
using CFM_PAYMENTSWS.Providers.FCB.DTOs;

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

        public PaymentCamelCase ConvertPaymentToCamelCase_MOZA(Payment payment)
        {
            if (payment == null) throw new ArgumentNullException(nameof(payment));

            return new PaymentCamelCase
            {
                BatchId = payment.BatchId,
                Description = payment.Description.Length > 40
                    ? payment.Description[..40]
                    : payment.Description,
                ProcessingDate = payment.ProcessingDate.ToString("yyyy-MM-ddTHH:mm:ss"),
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
                ProcessingDate = payment.ProcessingDate.ToString("yyyy-MM-ddTHH:mm:ss"),
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

        public FcbPaymentDTO ConvertPaymentToFcb(Payment payment)
        {
            if (payment == null) throw new ArgumentNullException(nameof(payment));

            return new FcbPaymentDTO
            {
                BatchId = payment.BatchId,
                Description = payment.Description.Length > 140
                    ? payment.Description[..140]
                    : payment.Description,
                ProcessingDate = payment.ProcessingDate.ToString("yyyy-MM-ddTHH:mm:ss.fff"),
                DebitAccount = payment.DebitAccount,
                InitgPtyCode = string.IsNullOrWhiteSpace(payment.initgPtyCode) ? "CFM" : payment.initgPtyCode,
                BatchBooking = string.IsNullOrWhiteSpace(payment.BatchBooking) ? "SUPPLIERS" : payment.BatchBooking,
                PaymentRecords = payment.PaymentRecords?.Select(pr => new FcbPaymentRecordDTO
                {
                    TransactionId = pr.TransactionId,
                    CreditAccount = pr.CreditAccount,
                    BeneficiaryName = pr.BeneficiaryName,
                    TransactionDescription = pr.TransactionDescription.Length > 140
                        ? pr.TransactionDescription[..140]
                        : pr.TransactionDescription,
                    Currency = pr.Currency,
                    Amount = pr.Amount,
                    BeneficiaryEmail = string.IsNullOrWhiteSpace(pr.BeneficiaryEmail) ? null : pr.BeneficiaryEmail
                }).ToList() ?? new List<FcbPaymentRecordDTO>()
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

        public string CalcularReferenciaComCheckDigit(string stringInput, string refPagCheck)
        {
            // Definir os pesos como no SQL original
            Dictionary<int, int> pesos = new Dictionary<int, int>
            {
                {1, 1}, {2, 10}, {3, 3}, {4, 30}, {5, 9}, {6, 90}, {7, 27}, {8, 76}, {9, 81}, {10, 34},
                {11, 49}, {12, 5}, {13, 50}, {14, 15}, {15, 53}, {16, 45}, {17, 62}, {18, 38}, {19, 89},
                {20, 17}, {21, 73}, {22, 51}, {23, 25}, {24, 56}, {25, 75}, {26, 71}, {27, 31}, {28, 19},
                {29, 93}, {30, 57}
            };

            // Calcular o check digit
            int i = stringInput.Length;
            int nTotal = 0;

            while (i > 0)
            {
                int posicao = i + 2;
                char c = stringInput[stringInput.Length - i];

                if (int.TryParse(c.ToString(), out int digito) && pesos.ContainsKey(posicao))
                {
                    nTotal += digito * pesos[posicao];
                }

                i--;
            }

            int check = 98 - (nTotal % 97);
            string finalCheck = check.ToString().PadLeft(2, '0');

            // Retornar a referência completa com o check digit
            return refPagCheck + finalCheck;
        }

    }
}
