using System;
using System.Collections.Generic;

namespace LENMEDWS.Domains.Models
{
    public partial class Para1
    {
        public string Para1stamp { get; set; } = null!;
        public string Descricao { get; set; } = null!;
        public string Valor { get; set; } = null!;
        public string Tipo { get; set; } = null!;
        public decimal Dec { get; set; }
        public decimal Tam { get; set; }
        public string Memvalor { get; set; } = null!;
        public byte[] Imgvalor { get; set; } = null!;
        public decimal Conf { get; set; }
        public string Ousrinis { get; set; } = null!;
        public DateTime Ousrdata { get; set; }
        public string Ousrhora { get; set; } = null!;
        public string Usrinis { get; set; } = null!;
        public DateTime Usrdata { get; set; }
        public string Usrhora { get; set; } = null!;
        public bool Marcada { get; set; }
        public decimal Tacesso { get; set; }
        public decimal Userno { get; set; }
        public string Usernm { get; set; } = null!;
        public string Acesso { get; set; } = null!;
        public string Tabela { get; set; } = null!;
        public decimal Pfcod { get; set; }
        public string Pfnm { get; set; } = null!;
        public string Ecran { get; set; } = null!;
        public string Ecrannm { get; set; } = null!;
        public bool Apenasecran { get; set; }
        public bool Actanexos { get; set; }
    }
}
