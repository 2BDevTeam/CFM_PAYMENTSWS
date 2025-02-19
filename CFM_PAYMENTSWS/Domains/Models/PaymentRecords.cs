using Newtonsoft.Json;
using System.Text.Json;

namespace CFM_PAYMENTSWS.Domains.Models
{
    public class PaymentRecords
    {
        public string TransactionId { get; set; }
        public string CreditAccount { get; set; }
        public string BeneficiaryName { get; set; }
        public string TransactionDescription { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public string? BeneficiaryEmail { get; set; }
        public override string ToString() => JsonConvert.SerializeObject(this);
    }


    // Esta classe foi criada somente para permitir que alguns providers recebam os pagamentos com o padrão camelCase
    public class PaymentRecordsCamelCase
    {
        public string TransactionId { get; set; }
        public string CreditAccount { get; set; }
        public string BeneficiaryName { get; set; }
        public string TransactionDescription { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public string BeneficiaryEmail { get; set; }
        public override string ToString()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true // Opcional, para melhor legibilidade.
            };

            return System.Text.Json.JsonSerializer.Serialize(this, options);
        }
    }
}
