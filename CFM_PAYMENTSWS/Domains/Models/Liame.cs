using System;
using System.Collections.Generic;

namespace CFM_PAYMENTSWS.Domains.Models
{
    public partial class Liame
    {
        public string Liamestamp { get; set; } = null!;
        public string Para { get; set; } = null!;
        public string Assunto { get; set; } = null!;
        public string Corpo { get; set; } = null!;
        public bool Processado { get; set; }
        public DateTime Ousrdata { get; set; }
        public string Keystamp { get; set; } = null!;
    }
}
