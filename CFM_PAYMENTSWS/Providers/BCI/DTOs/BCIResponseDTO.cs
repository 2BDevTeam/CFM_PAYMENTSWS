using Newtonsoft.Json;

namespace CFM_PAYMENTSWS.Providers.BCI.DTOs
{
    public class BCIResponseDTO
    {
        public string BatchId { get; set; }
        public string Description { get; set; }
        public DateTime ProcessingDate { get; set; }
        public string DebitAccount { get; set; }
        public string StatusCode { get; set; }
        public string StatusDescription { get; set; }
        public List<PaymentRecordsDTO> PaymentRecordsStatus { get; set; }
        public override string ToString() => JsonConvert.SerializeObject(this);
        public BCIResponseDTO() { }

        public BCIResponseDTO(string batchId, string description,string statusCode,string statusDescription)
        {
            BatchId = batchId;
            Description = description;
            StatusCode = statusCode;
            StatusDescription = statusDescription;
        }
        public BCIResponseDTO(string batchId, string description, DateTime processingDate, string origem, string statusCode, string statusDescription, List<PaymentRecordsDTO> paymentRecordsStatus)
        {
            BatchId = batchId;
            Description = description;
            ProcessingDate = processingDate;
            DebitAccount = origem;
            StatusCode = statusCode;
            StatusDescription = statusDescription;
            PaymentRecordsStatus = paymentRecordsStatus;
        }
    }
}
