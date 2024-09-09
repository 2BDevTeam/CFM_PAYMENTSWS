using System;
using System.Collections.Generic;

namespace CFM_PAYMENTSWS.Domains.Models
{
    public partial class UTrfb
    {
        public string UTrfbstamp { get; set; } = null!;
        public DateTime Dti { get; set; }
        public DateTime Dtf { get; set; }
        public string Banco { get; set; } = null!;
        public string Ficheiro { get; set; } = null!;
        public bool Corrente { get; set; }
        public string Id { get; set; } = null!;
        public DateTime Dt { get; set; }
        public decimal No { get; set; }
        public string Ccusto { get; set; } = null!;
        public DateTime Rdata { get; set; }
        public decimal Rno { get; set; }
        public string Cct { get; set; } = null!;
        public string Bc { get; set; } = null!;
        public string Ousrinis { get; set; } = null!;
        public DateTime Ousrdata { get; set; }
        public string Ousrhora { get; set; } = null!;
        public string Usrinis { get; set; } = null!;
        public DateTime Usrdata { get; set; }
        public string Usrhora { get; set; } = null!;
        public bool Marcada { get; set; }
        public decimal Valor { get; set; }
        public decimal Qtd { get; set; }
        public bool Pagto { get; set; }
        public bool Adito { get; set; }
        public bool Docta { get; set; }
        public string Stamp { get; set; } = null!;
        public bool Sendpay { get; set; }
        public decimal Valortrf { get; set; }
        public DateTime Datatrf { get; set; }
        public string Formatrf { get; set; } = null!;
        public string Usrtrf { get; set; } = null!;
    }
}
