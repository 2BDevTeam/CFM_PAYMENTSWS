using System;
using System.Collections.Generic;

namespace CFM_PAYMENTSWS.Domains.Models
{
    public partial class Tb
    {
        public string Tbstamp { get; set; } = null!;
        public DateTime Data { get; set; }
        public string Documento { get; set; } = null!;
        public string Cheque { get; set; } = null!;
        public string Descricao { get; set; } = null!;
        public decimal Valor { get; set; }
        public decimal Evalor { get; set; }
        public string Ollocal { get; set; } = null!;
        public string Local { get; set; } = null!;
        public string Sgrupo { get; set; } = null!;
        public string Grupo { get; set; } = null!;
        public decimal Contado { get; set; }
        public string Olcodigo { get; set; } = null!;
        public string Fref { get; set; } = null!;
        public string Ccusto { get; set; } = null!;
        public string Ncusto { get; set; } = null!;
        public bool Olbancos { get; set; }
        public bool Plano { get; set; }
        public decimal Tbno { get; set; }
        public string Intid { get; set; } = null!;
        public decimal Tbid { get; set; }
        public DateTime Dplano { get; set; }
        public string Dinoplano { get; set; } = null!;
        public decimal Dilnoplano { get; set; }
        public string Ousrinis { get; set; } = null!;
        public DateTime Ousrdata { get; set; }
        public string Ousrhora { get; set; } = null!;
        public string Usrinis { get; set; } = null!;
        public DateTime Usrdata { get; set; }
        public string Usrhora { get; set; } = null!;
        public bool Marcada { get; set; }
        public decimal Diaplano { get; set; }
        public string Prreford { get; set; } = null!;
        public string Frreford { get; set; } = null!;
        public string Vrreford { get; set; } = null!;
        public decimal UOriid { get; set; }
        public string Cbbstamp { get; set; } = null!;
        public decimal Formatoexp { get; set; }
        public bool Sr { get; set; }
        public DateTime UDatatrf { get; set; }
        public bool USendpay { get; set; }
        public string UUsrtrf { get; set; } = null!;
        public decimal Viabanco { get; set; }
        public string Idficheiro { get; set; } = null!;
        public string Filestatus { get; set; } = null!;
        public DateTime Filestatusdate { get; set; }
    }
}
