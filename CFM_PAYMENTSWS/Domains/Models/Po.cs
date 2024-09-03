using System;
using System.Collections.Generic;

namespace CFM_PAYMENTSWS.Domains.Models
{
    public partial class Po
    {
        public string Postamp { get; set; } = null!;
        public decimal Rno { get; set; }
        public DateTime Rdata { get; set; }
        public string Nome { get; set; } = null!;
        public decimal Total { get; set; }
        public decimal Etotal { get; set; }
        public string Tipo { get; set; } = null!;
        public decimal Pais { get; set; }
        public decimal Estab { get; set; }
        public decimal No { get; set; }
        public string Morada { get; set; } = null!;
        public string Local { get; set; } = null!;
        public string Codpost { get; set; } = null!;
        public string Ncont { get; set; } = null!;
        public decimal Poano { get; set; }
        public DateTime Dvalor { get; set; }
        public string Olcodigo { get; set; } = null!;
        public string Telocal { get; set; } = null!;
        public decimal Totalmoeda { get; set; }
        public string Moeda { get; set; } = null!;
        public string Desc1 { get; set; } = null!;
        public string Desc2 { get; set; } = null!;
        public decimal Contado { get; set; }
        public bool Process { get; set; }
        public string Cobranca { get; set; } = null!;
        public string Nib { get; set; } = null!;
        public decimal Fin { get; set; }
        public decimal Finv { get; set; }
        public decimal Finvmoeda { get; set; }
        public bool Impresso { get; set; }
        public string Userimpresso { get; set; } = null!;
        public DateTime Procdata { get; set; }
        public string Zona { get; set; } = null!;
        public string Ollocal { get; set; } = null!;
        public string Descba { get; set; } = null!;
        public string Fref { get; set; } = null!;
        public string Ccusto { get; set; } = null!;
        public string Ncusto { get; set; } = null!;
        public bool Plano { get; set; }
        public string Olstamp { get; set; } = null!;
        public string Fcstamp { get; set; } = null!;
        public string Segmento { get; set; } = null!;
        public string Cc2stamp { get; set; } = null!;
        public decimal Efinv { get; set; }
        public decimal Edifcambio { get; set; }
        public decimal Difcambio { get; set; }
        public decimal Cm { get; set; }
        public string Cmdesc { get; set; } = null!;
        public string Adoc { get; set; } = null!;
        public decimal Ivav1 { get; set; }
        public decimal Eivav1 { get; set; }
        public decimal Ivav2 { get; set; }
        public decimal Eivav2 { get; set; }
        public decimal Ivav3 { get; set; }
        public decimal Eivav3 { get; set; }
        public decimal Ivav4 { get; set; }
        public decimal Eivav4 { get; set; }
        public decimal Ivav5 { get; set; }
        public decimal Eivav5 { get; set; }
        public decimal Ivav6 { get; set; }
        public decimal Eivav6 { get; set; }
        public decimal Ivav7 { get; set; }
        public decimal Eivav7 { get; set; }
        public decimal Ivav8 { get; set; }
        public decimal Eivav8 { get; set; }
        public decimal Ivav9 { get; set; }
        public decimal Eivav9 { get; set; }
        public decimal Earred { get; set; }
        public string Memissao { get; set; } = null!;
        public decimal Arred { get; set; }
        public bool Introfin { get; set; }
        public string Intid { get; set; } = null!;
        public string Nome2 { get; set; } = null!;
        public bool Tbok { get; set; }
        public bool Faztrf { get; set; }
        public string Tbcheque { get; set; } = null!;
        public string Tbstamp { get; set; } = null!;
        public bool Regiva { get; set; }
        public DateTime Dplano { get; set; }
        public string Dinoplano { get; set; } = null!;
        public decimal Dilnoplano { get; set; }
        public decimal Diaplano { get; set; }
        public bool Planoonline { get; set; }
        public string Dostamp { get; set; } = null!;
        public decimal Totol2 { get; set; }
        public decimal Etotol2 { get; set; }
        public string Ollocal2 { get; set; } = null!;
        public string Telocal2 { get; set; } = null!;
        public decimal Contado2 { get; set; }
        public string Site { get; set; } = null!;
        public string Pnome { get; set; } = null!;
        public decimal Pno { get; set; }
        public string Cxstamp { get; set; } = null!;
        public string Cxusername { get; set; } = null!;
        public string Ssstamp { get; set; } = null!;
        public string Ssusername { get; set; } = null!;
        public decimal Vdinheiro { get; set; }
        public decimal Evdinheiro { get; set; }
        public decimal Mvdinheiro { get; set; }
        public decimal Chtotal { get; set; }
        public decimal Echtotal { get; set; }
        public decimal Chtmoeda { get; set; }
        public string Modop1 { get; set; } = null!;
        public decimal Epaga1 { get; set; }
        public decimal Paga1 { get; set; }
        public decimal Mpaga1 { get; set; }
        public string Modop2 { get; set; } = null!;
        public decimal Epaga2 { get; set; }
        public decimal Paga2 { get; set; }
        public decimal Mpaga2 { get; set; }
        public string Modop3 { get; set; } = null!;
        public decimal Epaga3 { get; set; }
        public decimal Paga3 { get; set; }
        public decimal Mpaga3 { get; set; }
        public string Modop4 { get; set; } = null!;
        public decimal Epaga4 { get; set; }
        public decimal Paga4 { get; set; }
        public decimal Mpaga4 { get; set; }
        public string Modop5 { get; set; } = null!;
        public decimal Epaga5 { get; set; }
        public decimal Paga5 { get; set; }
        public decimal Mpaga5 { get; set; }
        public string Modop6 { get; set; } = null!;
        public decimal Epaga6 { get; set; }
        public decimal Paga6 { get; set; }
        public decimal Mpaga6 { get; set; }
        public decimal Acerto { get; set; }
        public decimal Eacerto { get; set; }
        public decimal Macerto { get; set; }
        public decimal Totow { get; set; }
        public decimal Etotow { get; set; }
        public decimal Virs { get; set; }
        public decimal Evirs { get; set; }
        public string Moeda2 { get; set; } = null!;
        public decimal Valorowm2 { get; set; }
        public decimal Valorm2 { get; set; }
        public decimal Valor2m2 { get; set; }
        public decimal Earredm2 { get; set; }
        public string Moeda3 { get; set; } = null!;
        public decimal Valor2m3 { get; set; }
        public bool Exportado { get; set; }
        public bool Jatrfonline { get; set; }
        public decimal Txirs { get; set; }
        public bool Luserfin { get; set; }
        public string Bic { get; set; } = null!;
        public string Iban { get; set; } = null!;
        public bool Processsepa { get; set; }
        public string Sepagh { get; set; } = null!;
        public string Sepapi { get; set; } = null!;
        public bool Operext { get; set; }
        public bool Ivacaixa { get; set; }
        public decimal Poid { get; set; }
        public string UDesc { get; set; } = null!;
        public decimal UVirsm { get; set; }
        public decimal UPocam { get; set; }
        public decimal UPotot { get; set; }
        public decimal UMcam { get; set; }
        public bool UVerif { get; set; }
        public bool UImpresso { get; set; }
        public string UNref { get; set; } = null!;
        public string UFollocal { get; set; } = null!;
        public string Ousrinis { get; set; } = null!;
        public DateTime Ousrdata { get; set; }
        public string Ousrhora { get; set; } = null!;
        public string Usrinis { get; set; } = null!;
        public DateTime Usrdata { get; set; }
        public string Usrhora { get; set; } = null!;
        public bool Marcada { get; set; }
        public bool UOwirps { get; set; }
        public decimal Chftotal { get; set; }
        public decimal Echftotal { get; set; }
        public decimal Ecativa { get; set; }
        public decimal Cativa { get; set; }
        public decimal Mcativa { get; set; }
        public string UMotivtrf { get; set; } = null!;
        public bool UIntgrok { get; set; }
        public string UBenef { get; set; } = null!;
        public string UFicheiro { get; set; } = null!;
        public string UNib { get; set; } = null!;
        public string UBalcao { get; set; } = null!;
        public string UNome { get; set; } = null!;
        public bool UIncluido { get; set; }
        public string UBanco { get; set; } = null!;
        public string UConta { get; set; } = null!;
        public bool USendpay { get; set; }
        public string UUsrtrf { get; set; } = null!;
        public DateTime UDatatrf { get; set; }
        public string URefbanco { get; set; } = null!;
    }
}
