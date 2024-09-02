using Newtonsoft.Json;

namespace CFM_PAYMENTSWS.Domains.Models
{
    public class PaymentRecords
    {
        public string TransactionId { get; set; }
        public string CreditAccount { get; set; }
        public string BeneficiaryName { get; set; }
        public string TransactionDescription { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public string BeneficiaryEmail { get; set; }
        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
