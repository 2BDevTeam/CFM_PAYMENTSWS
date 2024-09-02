using Newtonsoft.Json;

namespace CFM_PAYMENTSWS.Providers.Nedbank.DTOs
{
    public class NedBankCheckPaymentReportResponseDTO
    {
        public string BatchId { get; set; }
        public string Description { get; set; }
        public DateTime ProcessingDate { get; set; }
        public string DebitAccount { get; set; }
        public string StatusCode { get; set; }
        public string StatusDescription { get; set; }

        public List<PaymentRecordsDTO> PaymentRecordsStatus { get; set; }


        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
