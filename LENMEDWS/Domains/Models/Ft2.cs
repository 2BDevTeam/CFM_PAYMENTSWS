using System;
using System.Collections.Generic;

namespace LENMEDWS.Domains.Models
{
    public partial class Ft2
    {
        public string Ft2stamp { get; set; } = null!;
        public string Ousrinis { get; set; } = null!;
        public DateTime Ousrdata { get; set; }
        public string Usrinis { get; set; } = null!;
        public string Usrhora { get; set; } = null!;
        public DateTime Usrdata { get; set; }
        public bool Marcada { get; set; }
        public string Ousrhora { get; set; } = null!;
        public bool UKobosync { get; set; }
    }
}
