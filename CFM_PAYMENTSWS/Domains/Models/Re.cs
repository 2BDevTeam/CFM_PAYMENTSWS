using System;
using System.Collections.Generic;

namespace CFM_PAYMENTSWS.Domains.Models
{
    public partial class Re
    {
        public string Restamp { get; set; } = null!;
        public string Nmdoc { get; set; } = null!;
        public decimal Rno { get; set; }
        public DateTime Rdata { get; set; }
        public string Nome { get; set; } = null!;
        public decimal Total { get; set; }
        public decimal Etotal { get; set; }
        public decimal Ndoc { get; set; }
        public decimal No { get; set; }
        public string Morada { get; set; } = null!;
        public string Local { get; set; } = null!;
        public string Codpost { get; set; } = null!;
        public string Ncont { get; set; } = null!;
        public decimal Reano { get; set; }
        public string Olcodigo { get; set; } = null!;
        public string Telocal { get; set; } = null!;
        public double Totalmoeda { get; set; }
        public string Moeda { get; set; } = null!;
        public string Desc1 { get; set; } = null!;
        public string Desc2 { get; set; } = null!;
        public string Fref { get; set; } = null!;
        public string Ccusto { get; set; } = null!;
        public string Ncusto { get; set; } = null!;
        public decimal Contado { get; set; }
        public bool Process { get; set; }
        public string Cobranca { get; set; } = null!;
        public string Nib { get; set; } = null!;
        public decimal Fin { get; set; }
        public decimal Finv { get; set; }
        public double Finvmoeda { get; set; }
        public bool Impresso { get; set; }
        public string Clbanco { get; set; } = null!;
        public string Clcheque { get; set; } = null!;
        public DateTime Procdata { get; set; }
        public DateTime Vdata { get; set; }
        public string Zona { get; set; } = null!;
        public string Ollocal { get; set; } = null!;
        public string Descba { get; set; } = null!;
        public bool Plano { get; set; }
        public decimal Vendedor { get; set; }
        public string Vendnm { get; set; } = null!;
        public string Olstamp { get; set; } = null!;
        public string Ccstamp { get; set; } = null!;
        public string Segmento { get; set; } = null!;
        public string Cc2stamp { get; set; } = null!;
        public decimal Efinv { get; set; }
        public decimal Edifcambio { get; set; }
        public decimal Difcambio { get; set; }
        public string Tipo { get; set; } = null!;
        public decimal Pais { get; set; }
        public decimal Estab { get; set; }
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
        public decimal Earred { get; set; }
        public string Memissao { get; set; } = null!;
        public decimal Arred { get; set; }
        public string Cobrador { get; set; } = null!;
        public string Rota { get; set; } = null!;
        public bool Introfin { get; set; }
        public bool Procomss { get; set; }
        public bool Cheque { get; set; }
        public DateTime Chdata { get; set; }
        public string Intid { get; set; } = null!;
        public string Nome2 { get; set; } = null!;
        public bool Regiva { get; set; }
        public decimal Reid { get; set; }
        public DateTime Dplano { get; set; }
        public string Dinoplano { get; set; } = null!;
        public decimal Dilnoplano { get; set; }
        public decimal Totol2 { get; set; }
        public decimal Etotol2 { get; set; }
        public string Ollocal2 { get; set; } = null!;
        public string Telocal2 { get; set; } = null!;
        public decimal Contado2 { get; set; }
        public decimal Chtotal { get; set; }
        public decimal Echtotal { get; set; }
        public string Site { get; set; } = null!;
        public string Pnome { get; set; } = null!;
        public decimal Pno { get; set; }
        public string Cxstamp { get; set; } = null!;
        public string Cxusername { get; set; } = null!;
        public string Ssstamp { get; set; } = null!;
        public string Ssusername { get; set; } = null!;
        public decimal Totow { get; set; }
        public decimal Etotow { get; set; }
        public string Ousrinis { get; set; } = null!;
        public DateTime Ousrdata { get; set; }
        public string Ousrhora { get; set; } = null!;
        public string Usrinis { get; set; } = null!;
        public DateTime Usrdata { get; set; }
        public string Usrhora { get; set; } = null!;
        public bool Marcada { get; set; }
        public decimal UMcam { get; set; }
        public decimal URecam { get; set; }
        public decimal URetot { get; set; }
        public bool Tbok { get; set; }
        public bool Faztrf { get; set; }
        public string Tptit { get; set; } = null!;
        public decimal Virs { get; set; }
        public decimal Evirs { get; set; }
        public string Chmoeda { get; set; } = null!;
        public decimal Chtotalm { get; set; }
        public string Moeda2 { get; set; } = null!;
        public decimal Valorowm2 { get; set; }
        public decimal Valorm2 { get; set; }
        public decimal Valor2m2 { get; set; }
        public decimal Earredm2 { get; set; }
        public string Moeda3 { get; set; } = null!;
        public decimal Valor2m3 { get; set; }
        public string Userimpresso { get; set; } = null!;
        public decimal Vdinheiro { get; set; }
        public decimal Evdinheiro { get; set; }
        public decimal Mvdinheiro { get; set; }
        public string Modop1 { get; set; } = null!;
        public decimal Epaga1 { get; set; }
        public decimal Paga1 { get; set; }
        public decimal Mpaga1 { get; set; }
        public decimal Ecompaga1 { get; set; }
        public decimal Compaga1 { get; set; }
        public decimal Mcompaga1 { get; set; }
        public string Modop2 { get; set; } = null!;
        public decimal Epaga2 { get; set; }
        public decimal Paga2 { get; set; }
        public decimal Mpaga2 { get; set; }
        public decimal Ecompaga2 { get; set; }
        public decimal Compaga2 { get; set; }
        public decimal Mcompaga2 { get; set; }
        public string Modop3 { get; set; } = null!;
        public decimal Epaga3 { get; set; }
        public decimal Paga3 { get; set; }
        public decimal Mpaga3 { get; set; }
        public decimal Ecompaga3 { get; set; }
        public decimal Compaga3 { get; set; }
        public decimal Mcompaga3 { get; set; }
        public string Modop4 { get; set; } = null!;
        public decimal Epaga4 { get; set; }
        public decimal Paga4 { get; set; }
        public decimal Mpaga4 { get; set; }
        public decimal Ecompaga4 { get; set; }
        public decimal Compaga4 { get; set; }
        public decimal Mcompaga4 { get; set; }
        public string Modop5 { get; set; } = null!;
        public decimal Epaga5 { get; set; }
        public decimal Paga5 { get; set; }
        public decimal Mpaga5 { get; set; }
        public decimal Ecompaga5 { get; set; }
        public decimal Compaga5 { get; set; }
        public decimal Mcompaga5 { get; set; }
        public string Modop6 { get; set; } = null!;
        public decimal Epaga6 { get; set; }
        public decimal Paga6 { get; set; }
        public decimal Mpaga6 { get; set; }
        public decimal Ecompaga6 { get; set; }
        public decimal Compaga6 { get; set; }
        public decimal Mcompaga6 { get; set; }
        public decimal Acerto { get; set; }
        public decimal Eacerto { get; set; }
        public decimal Macerto { get; set; }
        public decimal Diaplano { get; set; }
        public bool Planoonline { get; set; }
        public string Dostamp { get; set; } = null!;
        public bool Exportado { get; set; }
        public decimal Ivav7 { get; set; }
        public decimal Eivav7 { get; set; }
        public decimal Ivav8 { get; set; }
        public decimal Eivav8 { get; set; }
        public decimal Ivav9 { get; set; }
        public decimal Eivav9 { get; set; }
        public string Contrato { get; set; } = null!;
        public string Cessao { get; set; } = null!;
        public string Facstamp { get; set; } = null!;
        public string Faccstamp { get; set; } = null!;
        public decimal Totoladi { get; set; }
        public decimal Etotoladi { get; set; }
        public Guid Rowid { get; set; }
        public decimal Cbbno { get; set; }
        public bool Luserfin { get; set; }
        public string Bic { get; set; } = null!;
        public string Iban { get; set; } = null!;
        public bool Processsepa { get; set; }
        public decimal Totalimp { get; set; }
        public decimal Etotalimp { get; set; }
        public string Sepagh { get; set; } = null!;
        public string Sepapi { get; set; } = null!;
        public bool Operext { get; set; }
        public decimal Npedido { get; set; }
        public string Tiporeg { get; set; } = null!;
        public bool Temch { get; set; }
        public string Paymentrefnoori { get; set; } = null!;
        public string Upddespacho { get; set; } = null!;
        public bool Revogado { get; set; }
        public DateTime Revdata { get; set; }
        public bool Anulado { get; set; }
        public string Anulinis { get; set; } = null!;
        public DateTime Anuldata { get; set; }
        public string Anulhora { get; set; } = null!;
        public decimal Esirca { get; set; }
        public decimal Ecativa { get; set; }
        public decimal Cativa { get; set; }
        public decimal Mcativa { get; set; }
        public bool UIntgrok { get; set; }
        public string Atcud { get; set; } = null!;
        public decimal Contingencia { get; set; }
        public string Nomeat { get; set; } = null!;
        public decimal UEntps { get; set; }
        public string URefps { get; set; } = null!;
        public string UTransid { get; set; } = null!;
    }
}
