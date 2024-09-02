using Newtonsoft.Json;

namespace CFM_PAYMENTSWS.Domains.Models
{
    public class U2BPaymentsHs
    {
        public string TransactionId { get; set; }
        public string CreditAccount { get; set; }
        public string BeneficiaryName { get; set; }
        public string TransactionDescription { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public string BankReference { get; set; }
        public string StatusCode { get; set; }
        public string StatusDescription { get; set; }
        public string BatchId { get; set; }
        public DateTime ProcessingDate { get; set; }
        public string StatusCodeHs { get; set; }
        public string StatusDescriptionHs { get; set; }
        public string U2BPaymentsHsStamp { get; set; }
        public string DebitAccount { get; set; }
        public DateTime OusrData { get; set; }
        public override string ToString() => JsonConvert.SerializeObject(this);

        public U2BPaymentsHs(string transactionId,string creditAccount, string beneficiaryName, string transactionDescription, string currency, decimal amount, string bankReference, string statusCode, string statusDescription, string batchId, DateTime processingDate, string statusCodeHs, string statusDescriptionHs,
        string u2BPaymentsHsStamp,
        string debitAccount,
        DateTime ousrData)
        {
            TransactionId = transactionId;
            CreditAccount = creditAccount;
            BeneficiaryName = beneficiaryName;
            TransactionDescription = transactionDescription;
            Currency = currency;
            Amount = amount;
            BankReference = bankReference;
            StatusCode = statusCode;
            StatusDescription = statusDescription;
            BatchId = batchId;
            ProcessingDate = processingDate;
            StatusCodeHs = statusCodeHs;
            StatusDescriptionHs = statusDescriptionHs;
            U2BPaymentsHsStamp = u2BPaymentsHsStamp;
            DebitAccount = debitAccount;
            OusrData = ousrData;
        }

        public U2BPaymentsHs()
        {
        }
    }
}
