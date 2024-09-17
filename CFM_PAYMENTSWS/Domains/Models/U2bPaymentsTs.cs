using System;
using System.Collections.Generic;

namespace CFM_PAYMENTSWS.Domains.Models
{
    public partial class U2bPaymentsTs
    {
        public string U2bPaymentsTsstamp { get; set; } = null!;
        public decimal Valor { get; set; }
        public string Tabela { get; set; } = null!;
        public string Oristamp { get; set; } = null!;
        public bool Processado { get; set; }
        public DateTime Dataprocessado { get; set; }
        public string Horaprocessado { get; set; } = null!;
        public int? Canal { get; set; }
        public string Moeda { get; set; } = null!;
        public string Estado { get; set; } = null!;
        public string Descricao { get; set; } = null!;
        public int Lordem { get; set; }
        public string Usrinis { get; set; } = null!;
        public DateTime Usrdata { get; set; }
        public string Usrhora { get; set; } = null!;
        public DateTime? Ousrdata { get; set; }
        public string? Ousrinis { get; set; }
        public string? Ousrhora { get; set; }
        public bool Marcada { get; set; }
        public string? Transactionid { get; set; }
        public string? Keystamp { get; set; }
        public string? Docno { get; set; }
        public bool? Lancadonatesouraria { get; set; }
        public string? MpesaTransactionid { get; set; }
        public string BatchId { get; set; } = null!;
        public bool Checked { get; set; }
        public DateTime ProcessingDateHs { get; set; }
        public string StatusCodeHs { get; set; } = null!;
        public string StatusDescriptionHs { get; set; } = null!;
        public string Origem { get; set; } = null!;
        public string Destino { get; set; } = null!;
        public string? BankReference { get; set; }
        public string Emailf { get; set; } = null!;
    }
}
