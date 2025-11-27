using System;
using System.Collections.Generic;

namespace CFM_PAYMENTSWS.Domains.Models
{
    public partial class Rech
    {

        public string Rechstamp { get; set; } = null!;
        public string Clbanco { get; set; } = null!;
        public string Clcheque { get; set; } = null!;
        public DateTime Chdata { get; set; }
        public decimal Chvalor { get; set; }
        public decimal Echvalor { get; set; }
        public decimal Chvalorm { get; set; }
        public string Tptit { get; set; } = null!;
        public string Restamp { get; set; } = null!;
        public string Ousrinis { get; set; } = null!;
        public DateTime Ousrdata { get; set; }
        public string Ousrhora { get; set; } = null!;
        public string Usrinis { get; set; } = null!;
        public DateTime Usrdata { get; set; }
        public string Usrhora { get; set; } = null!;
        public bool Marcada { get; set; }
    }
}
