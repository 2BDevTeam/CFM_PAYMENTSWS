using System;
using System.Collections.Generic;

namespace LENMEDWS.Domains.Models
{
    public partial class Cu
    {
        public string Custamp { get; set; } = null!;
        public string Cct { get; set; } = null!;
        public string Descricao { get; set; } = null!;
        public bool Inactivo { get; set; }
        public bool Integracao { get; set; }
        public string Ousrinis { get; set; } = null!;
        public DateTime Ousrdata { get; set; }
        public string Ousrhora { get; set; } = null!;
        public string Usrinis { get; set; } = null!;
        public DateTime Usrdata { get; set; }
        public string Usrhora { get; set; } = null!;
        public bool Marcada { get; set; }
    }
}
