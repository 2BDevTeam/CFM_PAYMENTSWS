using Newtonsoft.Json;

namespace CFM_PAYMENTSWS.Providers.Moza.DTOs
{
    public class MozaPaymentRecordDTO
    {
        public string? Id { get; set; }
        public string? TransactionId { get; set; }
        public string? CreditAccount { get; set; }
        public string? BeneficiaryName { get; set; }
        public string? TransactionDescription { get; set; }
        public string? Currency { get; set; }
        public decimal Amount { get; set; }
        public string? StatusCode { get; set; }
        public string? StatusDescription { get; set; }
        public string? BeneficiaryEmail { get; set; }
        public string? BankReference { get; set; }
    }

    public class MozaPaymentDataDTO
    {
        public string? BatchId { get; set; }
        public string? Description { get; set; }
        public string? CreationDate { get; set; }
        public string? ProcessingDate { get; set; }
        public string? DebitAccount { get; set; }
        public string? StatusCode { get; set; }
        public string? StatusDescription { get; set; }
        public string? McFilename { get; set; }
        public string? InitgPtyCode { get; set; }
        public int? IsSentToClient { get; set; }
        public int? IsSentToMc { get; set; }
        public string? McStatus { get; set; }
        public string? ClientCode { get; set; }
        public List<MozaPaymentRecordDTO>? PaymentRecords { get; set; }
    }

    public class MozaPaymentResponseDTO
    {
        public string? Status { get; set; }
        public string? Message { get; set; }
        public bool Success { get; set; }
        public MozaPaymentDataDTO? Data { get; set; }

        public string? StatusCode => Data?.StatusCode;

        public string? StatusDescription => Data?.StatusDescription;

        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
