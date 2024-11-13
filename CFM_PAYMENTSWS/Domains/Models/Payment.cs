using Newtonsoft.Json;

namespace CFM_PAYMENTSWS.Domains.Models
{
    public class Payment
    {
        public string BatchId { get; set; }
        public string Description { get; set; }
        public DateTime ProcessingDate { get; set; }
        public string DebitAccount { get; set; }
        public string? initgPty_Code { get; set; } = null;
        public string? BatchBooking { get; set; } = null;
        public List<PaymentRecords> PaymentRecords { get; set; }
        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
