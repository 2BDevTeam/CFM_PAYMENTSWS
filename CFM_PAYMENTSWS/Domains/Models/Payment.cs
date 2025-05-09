﻿using Newtonsoft.Json;
using System.Text.Encodings.Web;
using System.Text.Json;


namespace CFM_PAYMENTSWS.Domains.Models
{
    public class Payment
    {
        public string BatchId { get; set; }
        public string Description { get; set; }
        public DateTime ProcessingDate { get; set; }
        public string DebitAccount { get; set; }
        public string? initgPtyCode { get; set; } = null;
        public string? BatchBooking { get; set; } = null;
        public List<PaymentRecords> PaymentRecords { get; set; }
        public override string ToString() => JsonConvert.SerializeObject(this);
    }


    // Esta classe foi criada somente para permitir que alguns providers recebam os pagamentos com o padrão camelCase
    public class PaymentCamelCase
    {
        public string BatchId { get; set; }
        public string Description { get; set; }
        public string ProcessingDate { get; set; }
        public string DebitAccount { get; set; }
        public string? initgPtyCode { get; set; } = null;
        public string? BatchBooking { get; set; } = null;
        public List<PaymentRecordsCamelCase> PaymentRecords { get; set; }
        public override string ToString()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true // Opcional, para melhor legibilidade.
            };

            return System.Text.Json.JsonSerializer.Serialize(this, options);
        }
    }


    // Esta classe foi criada somente para permitir que o banco Bim receba pagamentos, devido a versão 1.5 da descrição técnica
    public class Paymentv1_5
    {
        public string BatchId { get; set; }
        public string Description { get; set; }
        public string ProcessingDate { get; set; }
        public string DebitAccount { get; set; }
        public string initgPty_Code { get; set; } = null!;
        public int BatchBooking { get; set; }
        public List<PaymentRecords> PaymentRecords { get; set; }
        public override string ToString() => JsonConvert.SerializeObject(this);
        
    }

}
