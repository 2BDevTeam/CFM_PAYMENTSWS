using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CFM_PAYMENTSWS.DTOs
{
    public class PaymentDynamicDTO
    {
        [Required]
        [JsonPropertyName("id")]
        public string PaymentId { get; set; }

        [Required]
        [JsonPropertyName("entidade")]
        public int Entity { get; set; }

        [Required]
        [JsonPropertyName("referencia")]
        public string CustomerRef { get; set; }

        [Required]
        [JsonPropertyName("data")]
        public DateTime Date { get; set; }

        [Required]
        [JsonPropertyName("valor")]
        public decimal Amount { get; set; }

        [Required]
        [JsonPropertyName("moeda")]
        public string Currency { get; set; }

        [Required]
        [JsonPropertyName("canal")]
        public string Method { get; set; }

        [Required]
        [JsonPropertyName("refPagamento")]
        public string PaymentReference{ get; set; }

        public string? Description { get; set; }

        public override string ToString() => JsonConvert.SerializeObject(this);


    }

}
