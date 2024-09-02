using Newtonsoft.Json;

namespace CFM_PAYMENTSWS.DTOs
{
    public class PaymentRecordResponseDTO
    {

        public string transactionId { get; set; }
        public string bankReference { get; set; }
        public ResponseDTO paymentRecordsResponse { get; set; }
        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
