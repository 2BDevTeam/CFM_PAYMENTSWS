using Newtonsoft.Json;

namespace CFM_PAYMENTSWS.Domains.Models
{
    public class PaymentHeader
    {
        public string BatchId { get; set; }
        public string description { get; set; }
        public DateTime ProcessingDate { get; set; }
        public string DebitAccount { get; set; }
        public override string ToString() => JsonConvert.SerializeObject(this);

    }
}
