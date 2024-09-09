using System;
using System.Collections.Generic;

namespace CFM_PAYMENTSWS.Domains.Models
{
    public partial class Ow
    {
        public string Owstamp { get; set; } = null!;
        public DateTime Data { get; set; }
        public string Docnome { get; set; } = null!;
        public string Descricao { get; set; } = null!;
        public decimal Entr { get; set; }
        public decimal Said { get; set; }
        public decimal Eentr { get; set; }
        public decimal Esaid { get; set; }
        public string Local { get; set; } = null!;
        public string Sgrupo { get; set; } = null!;
        public string Grupo { get; set; } = null!;
        public string Origem { get; set; } = null!;
        public decimal Contado { get; set; }
        public string Ollocal { get; set; } = null!;
        public string Olcodigo { get; set; } = null!;
        public string Cheque { get; set; } = null!;
        public string Fref { get; set; } = null!;
        public string Ccusto { get; set; } = null!;
        public string Ncusto { get; set; } = null!;
        public DateTime Dvalor { get; set; }
        public string Olstamp { get; set; } = null!;
        public decimal Docno { get; set; }
        public decimal Owid { get; set; }
        public string Intid { get; set; } = null!;
        public bool Plano { get; set; }
        public string Adoc { get; set; } = null!;
        public bool Ollinhas { get; set; }
        public string Ozstamp { get; set; } = null!;
        public string Postamp { get; set; } = null!;
        public string Restamp { get; set; } = null!;
        public decimal Lordem { get; set; }
        public string Site { get; set; } = null!;
        public string Pnome { get; set; } = null!;
        public decimal Pno { get; set; }
        public string Cxstamp { get; set; } = null!;
        public string Cxusername { get; set; } = null!;
        public string Ssstamp { get; set; } = null!;
        public string Ssusername { get; set; } = null!;
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
        public decimal UMcam { get; set; }
        public string UMoe { get; set; } = null!;
        public decimal UOwcam { get; set; }
        public decimal UOwent { get; set; }
        public decimal UOwsaid { get; set; }
        public decimal UOwtot { get; set; }
        public string Moeda1 { get; set; } = null!;
        public string Moeda2 { get; set; } = null!;
        public decimal Entrm1 { get; set; }
        public decimal Entrm2 { get; set; }
        public decimal Saidm1 { get; set; }
        public decimal Saidm2 { get; set; }
        public string Prstamp { get; set; } = null!;
        public string Frstamp { get; set; } = null!;
        public string Vrstamp { get; set; } = null!;
        public decimal Diaplano { get; set; }
        public bool Owliolcod { get; set; }
        public string Processo { get; set; } = null!;
        public string Subproc { get; set; } = null!;
        public string UContab { get; set; } = null!;
        public string UDebita { get; set; } = null!;
        public string UFactor { get; set; } = null!;
        public bool Planoonline { get; set; }
        public string Dostamp { get; set; } = null!;
        public bool Exportado { get; set; }
        public string UCaixa { get; set; } = null!;
        public string UBennome { get; set; } = null!;
        public decimal UBenno { get; set; }
        public bool UImpresso { get; set; }
        public bool UIndexada { get; set; }
        public decimal UNfunc { get; set; }
        public string UNomefunc { get; set; } = null!;
        public bool Operext { get; set; }
        public string Cecope { get; set; } = null!;
        public bool Lancancont { get; set; }
        public string Ncont { get; set; } = null!;
        public string Oristamp { get; set; } = null!;
        public decimal Recibo { get; set; }
        public bool Lancapen { get; set; }
        public decimal UEstabfl { get; set; }
        public decimal UNofl { get; set; }
        public string UNomefl { get; set; } = null!;
        public bool UIntgrok { get; set; }
        public decimal UNope { get; set; }
        public bool UXiporo { get; set; }
        public string UFicheiro { get; set; } = null!;
        public string UNome { get; set; } = null!;
        public string UBalcao { get; set; } = null!;
        public string UNib { get; set; } = null!;
        public string UNoconta { get; set; } = null!;
        public string UNobanco { get; set; } = null!;
        public string UFdconta { get; set; } = null!;
        public string UFdbanco { get; set; } = null!;
        public string UFdnib { get; set; } = null!;
        public string UBanco { get; set; } = null!;
        public string UConta { get; set; } = null!;
        public bool USendpay { get; set; }
        public string UUsrtrf { get; set; } = null!;
        public DateTime UDatatrf { get; set; }
    }
}
