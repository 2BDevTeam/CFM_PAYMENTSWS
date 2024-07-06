using System;
using System.Collections.Generic;

namespace LENMEDWS.Domains.Models
{
    public partial class Do
    {
        public string Dostamp { get; set; } = null!;
        public string Dinome { get; set; } = null!;
        public decimal Dilno { get; set; }
        public string Docnome { get; set; } = null!;
        public string Adoc { get; set; } = null!;
        public DateTime Data { get; set; }
        public decimal Mes { get; set; }
        public decimal Dia { get; set; }
        public decimal Dino { get; set; }
        public decimal Doctipo { get; set; }
        public decimal Debana { get; set; }
        public decimal Debord { get; set; }
        public decimal Debfin { get; set; }
        public decimal Creana { get; set; }
        public decimal Creord { get; set; }
        public decimal Crefin { get; set; }
        public decimal Edebana { get; set; }
        public decimal Edebord { get; set; }
        public decimal Edebfin { get; set; }
        public decimal Ecreana { get; set; }
        public decimal Ecreord { get; set; }
        public decimal Ecrefin { get; set; }
        public string Apstamp { get; set; } = null!;
        public bool Conf1 { get; set; }
        public bool Conf2 { get; set; }
        public string Memissao { get; set; } = null!;
        public string Pncont { get; set; } = null!;
        public string Ncont { get; set; } = null!;
        public string Identdecexp { get; set; } = null!;
        public bool Einvsuj { get; set; }
        public decimal Ano { get; set; }
        public DateTime Datadiva { get; set; }
        public bool Rectranimov { get; set; }
        public string Tipoop { get; set; } = null!;
        public string Nomeop { get; set; } = null!;
        public decimal Cambio { get; set; }
        public string Tabori { get; set; } = null!;
        public DateTime Datarect { get; set; }
        public string Ousrinis { get; set; } = null!;
        public DateTime Ousrdata { get; set; }
        public string Ousrhora { get; set; } = null!;
        public string Usrinis { get; set; } = null!;
        public DateTime Usrdata { get; set; }
        public string Usrhora { get; set; } = null!;
        public bool Marcada { get; set; }
        public bool Meiost { get; set; }
        public bool Prov { get; set; }
        public string Tiporeg { get; set; } = null!;
        public decimal Basesp { get; set; }
        public decimal Ebasesp { get; set; }
        public decimal Basees { get; set; }
        public decimal Ebasees { get; set; }
        public decimal Ivasp { get; set; }
        public decimal Eivasp { get; set; }
        public decimal Ivaes { get; set; }
        public decimal Eivaes { get; set; }
        public decimal Npedido { get; set; }
        public string Anexo41 { get; set; } = null!;
        public bool Saldoinicial { get; set; }
        public DateTime Docdata { get; set; }
        public bool Criadoimp { get; set; }
        public bool UltApuraCev { get; set; }
        public decimal Dilnoid { get; set; }
        public string Strqrcode { get; set; } = null!;
    }
}
