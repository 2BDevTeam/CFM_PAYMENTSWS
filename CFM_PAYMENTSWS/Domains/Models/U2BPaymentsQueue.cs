using System;
using System.Collections.Generic;

namespace CFM_PAYMENTSWS.Domains.Models
{
    public partial class U2bPaymentsQueue
    {
        public string U2bPaymentsQueuestamp { get; set; } = null!;
        public string? Origem { get; set; }
        public string Destino { get; set; } = null!;
        public decimal Valor { get; set; }
        public string? Moeda { get; set; }
        public int Canal { get; set; }
        public int Lordem { get; set; }
        public string Usrinis { get; set; } = null!;
        public DateTime Usrdata { get; set; }
        public string Usrhora { get; set; } = null!;
        public DateTime? Ousrdata { get; set; }
        public string? Ousrinis { get; set; }
        public string? Ousrhora { get; set; }
        public bool Marcada { get; set; }
        public string? TransactionId { get; set; }
        public string? Keystamp { get; set; }
        public string BatchId { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string TransactionDescription { get; set; } = null!;
        public string? Oristamp { get; set; }
        public string? Docno { get; set; }
        public string Descricao { get; set; } = null!;
        public string Estado { get; set; } = null!;
        public DateTime? ProcessingDate { get; set; }
        public string? BeneficiaryName { get; set; }
        public string Emailf { get; set; } = null!;
        public string Tabela { get; set; } = null!;
        public string Ccusto { get; set; } = null!;
    }
}
