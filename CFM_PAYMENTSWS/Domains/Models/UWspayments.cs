using System;
using System.Collections.Generic;

namespace CFM_PAYMENTSWS.Domains.Models
{
    public partial class UWspayments
    {
        public string UWspaymentsstamp { get; set; } = null!;
        public string Estado { get; set; } = null!;
        public string Bankreference { get; set; } = null!;
        public string Oristamp { get; set; } = null!;
        public string Origem { get; set; } = null!;
        public string Destino { get; set; } = null!;
        public string Descricao { get; set; } = null!;
        public DateTime Dataprocessado { get; set; }
        public string Batchid { get; set; } = null!;
        public string Ousrinis { get; set; } = null!;
        public DateTime Ousrdata { get; set; }
        public string Ousrhora { get; set; } = null!;
        public string Usrinis { get; set; } = null!;
        public DateTime Usrdata { get; set; }
        public string Usrhora { get; set; } = null!;
        public bool Marcada { get; set; }
    }
}
