using Newtonsoft.Json;

namespace CFM_PAYMENTSWS.Providers.Bim.DTOs
{
    public class BimResponseDTO
    {
        public string BatchId { get; set; }
        public string Description { get; set; }
        public string ProcessingDate { get; set; }
        public string DebitAccount { get; set; }
        public string StatusCode { get; set; }
        public string StatusDescription { get; set; }
        public List<PaymentRecordsDTO> paymentCheckedRecords { get; set; }
        public override string ToString() => JsonConvert.SerializeObject(this);
        public BimResponseDTO() { }

        public BimResponseDTO(string batchId, string description,string statusCode,string statusDescription)
        {
            BatchId = batchId;
            Description = description;
            StatusCode = statusCode;
            StatusDescription = statusDescription;
        }
        public BimResponseDTO(string batchId, string description, string processingDate, string origem, string statusCode, string statusDescription, List<PaymentRecordsDTO> paymentRecordsStatus)
        {
            BatchId = batchId;
            Description = description;
            ProcessingDate = processingDate;
            DebitAccount = origem;
            StatusCode = statusCode;
            StatusDescription = statusDescription;
            paymentCheckedRecords = paymentRecordsStatus;
        }
    }
}
