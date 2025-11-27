using System;
using System.Collections.Generic;

namespace CFM_PAYMENTSWS.Domains.Models
{
    public partial class U2bRecPayments
    {
        public string U2bRecPaymentsStamp { get; set; } = null!;
        public string IdPagamento { get; set; } = null!;
        public int Entidade { get; set; }
        public string Referencia { get; set; }
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }
        public bool Enviado { get; set; }
        public string StatusCode { get; set; } = null!;
        public string StatusDescription { get; set; } = null!;
        public string Metodo { get; set; } = null!;
        public string Provider { get; set; } = null!;
        public string Usrinis { get; set; } = null!;
        public DateTime Usrdata { get; set; }
        public string Usrhora { get; set; } = null!;
        public DateTime? Ousrdata { get; set; }
        public string Ousrinis { get; set; } = null!;
        public string? Ousrhora { get; set; }
        public bool Marcada { get; set; }
        public string Moeda { get; set; } = null!;
        public string Descricao { get; set; } = null!;
    }
}
