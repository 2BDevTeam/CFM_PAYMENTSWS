using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace CFM_PAYMENTSWS.Providers.FCB.DTOs
{
    public class FcbPaymentDTO
    {
        public string BatchId { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ProcessingDate { get; set; } = string.Empty;
        public string DebitAccount { get; set; } = string.Empty;
        public string? InitgPtyCode { get; set; }
        public string? BatchBooking { get; set; }
        public List<FcbPaymentRecordDTO> PaymentRecords { get; set; } = new();

        public override string ToString()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                WriteIndented = false
            };

            return System.Text.Json.JsonSerializer.Serialize(this, options);
        }
    }

    public class FcbPaymentRecordDTO
    {
        public string TransactionId { get; set; } = string.Empty;
        public string CreditAccount { get; set; } = string.Empty;
        public string BeneficiaryName { get; set; } = string.Empty;
        public string TransactionDescription { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string? BeneficiaryEmail { get; set; }

        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
