namespace CFM_PAYMENTSWS.Providers.Bim.DTOs
{
    public class PaymentRecordsDTO
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
    }
}
