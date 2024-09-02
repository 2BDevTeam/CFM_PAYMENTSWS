using Newtonsoft.Json;

namespace CFM_PAYMENTSWS.DTOs
{
    public class CheckPaymentReportResponseDTO
    {
       public ResponseDTO batchResponse { get; set; }
       public List<PaymentRecordResponseDTO> paymentRecordsResponse { get; set; }
       public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
