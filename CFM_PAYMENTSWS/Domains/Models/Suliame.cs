using System;
using System.Collections.Generic;

namespace CFM_PAYMENTSWS.Domains.Models
{
    public partial class Suliame
    {
        public decimal Userno { get; set; }
        public string Email { get; set; } = null!;
        public string Tlmvl { get; set; } = null!;
    }
}
