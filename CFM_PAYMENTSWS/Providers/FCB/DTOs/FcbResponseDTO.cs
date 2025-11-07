using System.Collections.Generic;
using Newtonsoft.Json;

namespace CFM_PAYMENTSWS.Providers.FCB.DTOs
{
    public class FcbResponseDTO
    {
        [JsonProperty("batchId")]
        public string BatchId { get; set; } = string.Empty;

        [JsonProperty("description")]
        public string Description { get; set; } = string.Empty;

        [JsonProperty("processingDate")]
        public string ProcessingDate { get; set; } = string.Empty;

        [JsonProperty("initgPtyCode")]
        public string InitgPtyCode { get; set; } = string.Empty;

        [JsonProperty("batchBooking")]
        public string BatchBooking { get; set; } = string.Empty;

        [JsonProperty("debitAccount")]
        public string DebitAccount { get; set; } = string.Empty;

        [JsonProperty("statusCode")]
        public string StatusCode { get; set; } = string.Empty;

        [JsonProperty("statusDescription")]
        public string StatusDescription { get; set; } = string.Empty;

        [JsonProperty("paymentRecordsStatus")]
        public List<FcbPaymentRecordStatusDTO> PaymentRecordsStatus { get; set; } = new();

        [JsonProperty("resultInfo")]
        public FcbResultInfoDTO ResultInfo { get; set; } = new();

        public override string ToString() => JsonConvert.SerializeObject(this);
    }

    public class FcbPaymentRecordStatusDTO
    {
        [JsonProperty("transactionId")]
        public string TransactionId { get; set; } = string.Empty;

        [JsonProperty("creditAccount")]
        public string CreditAccount { get; set; } = string.Empty;

        [JsonProperty("beneficiaryName")]
        public string BeneficiaryName { get; set; } = string.Empty;

        [JsonProperty("transactionDescription")]
        public string TransactionDescription { get; set; } = string.Empty;

        [JsonProperty("currency")]
        public string Currency { get; set; } = string.Empty;

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("beneficiaryEmail")]
        public string BeneficiaryEmail { get; set; } = string.Empty;

        [JsonProperty("statusCode")]
        public string StatusCode { get; set; } = string.Empty;

        [JsonProperty("statusDescription")]
        public string StatusDescription { get; set; } = string.Empty;

        public override string ToString() => JsonConvert.SerializeObject(this);
    }

    public class FcbResultInfoDTO
    {
        [JsonProperty("rcode")]
        public int RCode { get; set; }

        [JsonProperty("messages")]
        public List<string> Messages { get; set; } = new();

        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
