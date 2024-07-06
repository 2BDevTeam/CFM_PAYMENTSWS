using System;
using System.Collections.Generic;

namespace LENMEDWS.Domains.Models
{
    public partial class Ft
    {
        public string Ftstamp { get; set; } = null!;
        public decimal Pais { get; set; }
        public string Nmdoc { get; set; } = null!;
        public decimal Fno { get; set; }
        public decimal No { get; set; }
        public string Nome { get; set; } = null!;
        public string Morada { get; set; } = null!;
        public string Local { get; set; } = null!;
        public string Codpost { get; set; } = null!;
        public string Ncont { get; set; } = null!;
        public string Bino { get; set; } = null!;
        public DateTime Bidata { get; set; }
        public string Bilocal { get; set; } = null!;
        public string Telefone { get; set; } = null!;
        public string Zona { get; set; } = null!;
        public decimal Vendedor { get; set; }
        public string Vendnm { get; set; } = null!;
        public DateTime Fdata { get; set; }
        public decimal Ftano { get; set; }
        public string Encomenda { get; set; } = null!;
        public string Pagamento { get; set; } = null!;
        public DateTime Pdata { get; set; }
        public string Expedicao { get; set; } = null!;
        public string Carga { get; set; } = null!;
        public string Descar { get; set; } = null!;
        public string Saida { get; set; } = null!;
        public decimal Ivatx1 { get; set; }
        public decimal Ivatx2 { get; set; }
        public decimal Ivatx3 { get; set; }
        public decimal Fin { get; set; }
        public string Final { get; set; } = null!;
        public decimal Ndoc { get; set; }
        public decimal Ftpos { get; set; }
        public string Moeda { get; set; } = null!;
        public string Fref { get; set; } = null!;
        public string Ccusto { get; set; } = null!;
        public string Ncusto { get; set; } = null!;
        public bool Facturada { get; set; }
        public decimal Fnoft { get; set; }
        public string Nmdocft { get; set; } = null!;
        public decimal Estab { get; set; }
        public DateTime Cdata { get; set; }
        public decimal Ivatx4 { get; set; }
        public decimal Peso { get; set; }
        public bool Plano { get; set; }
        public string Segmento { get; set; } = null!;
        public decimal Totqtt { get; set; }
        public decimal Qtt1 { get; set; }
        public decimal Qtt2 { get; set; }
        public decimal Qtt3 { get; set; }
        public decimal Qtt4 { get; set; }
        public string Tipo { get; set; } = null!;
        public bool Impresso { get; set; }
        public string Userimpresso { get; set; } = null!;
        public bool Cobrado { get; set; }
        public string Cobranca { get; set; } = null!;
        public bool Lifref { get; set; }
        public decimal Tipodoc { get; set; }
        public string Matricula { get; set; } = null!;
        public string Chora { get; set; } = null!;
        public decimal Ivatx5 { get; set; }
        public decimal Ivatx6 { get; set; }
        public decimal Ivatx7 { get; set; }
        public decimal Ivatx8 { get; set; }
        public decimal Ivatx9 { get; set; }
        public bool Cambiofixo { get; set; }
        public string Memissao { get; set; } = null!;
        public string Cobrador { get; set; } = null!;
        public string Rota { get; set; } = null!;
        public bool Multi { get; set; }
        public bool Introfin { get; set; }
        public bool Cheque { get; set; }
        public string Clbanco { get; set; } = null!;
        public string Clcheque { get; set; } = null!;
        public decimal Chtotal { get; set; }
        public decimal Echtotal { get; set; }
        public decimal Chtmoeda { get; set; }
        public string Chmoeda { get; set; } = null!;
        public bool Jaexpedi { get; set; }
        public decimal Tot1 { get; set; }
        public decimal Tot2 { get; set; }
        public decimal Tot3 { get; set; }
        public decimal Tot4 { get; set; }
        public decimal Portes { get; set; }
        public decimal Custo { get; set; }
        public decimal Diferido { get; set; }
        public decimal Eivain1 { get; set; }
        public decimal Eivain2 { get; set; }
        public decimal Eivain3 { get; set; }
        public decimal Eivav1 { get; set; }
        public decimal Eivav2 { get; set; }
        public decimal Eivav3 { get; set; }
        public decimal Ettiliq { get; set; }
        public decimal Edescc { get; set; }
        public decimal Ettiva { get; set; }
        public decimal Etotal { get; set; }
        public decimal Eivain4 { get; set; }
        public decimal Eivav4 { get; set; }
        public decimal Ediferido { get; set; }
        public decimal Etot1 { get; set; }
        public decimal Etot2 { get; set; }
        public decimal Etot3 { get; set; }
        public decimal Etot4 { get; set; }
        public decimal Efinv { get; set; }
        public decimal Eportes { get; set; }
        public decimal Ecusto { get; set; }
        public decimal Eivain5 { get; set; }
        public decimal Eivav5 { get; set; }
        public decimal Edebreg { get; set; }
        public decimal Eivain6 { get; set; }
        public decimal Eivav6 { get; set; }
        public decimal Eivain7 { get; set; }
        public decimal Eivav7 { get; set; }
        public decimal Eivain8 { get; set; }
        public decimal Eivav8 { get; set; }
        public decimal Eivain9 { get; set; }
        public decimal Eivav9 { get; set; }
        public decimal Total { get; set; }
        public decimal Totalmoeda { get; set; }
        public decimal Finv { get; set; }
        public decimal Finvm { get; set; }
        public decimal Ivain1 { get; set; }
        public decimal Ivamin1 { get; set; }
        public decimal Ivain2 { get; set; }
        public decimal Ivamin2 { get; set; }
        public decimal Ivain3 { get; set; }
        public decimal Ivamin3 { get; set; }
        public decimal Ivain4 { get; set; }
        public decimal Ivamin4 { get; set; }
        public decimal Ivain5 { get; set; }
        public decimal Ivamin5 { get; set; }
        public decimal Ivain6 { get; set; }
        public decimal Ivamin6 { get; set; }
        public decimal Ivain7 { get; set; }
        public decimal Ivamin7 { get; set; }
        public decimal Ivain8 { get; set; }
        public decimal Ivamin8 { get; set; }
        public decimal Ivain9 { get; set; }
        public decimal Ivamin9 { get; set; }
        public decimal Ivav1 { get; set; }
        public decimal Ivamv1 { get; set; }
        public decimal Ivav2 { get; set; }
        public decimal Ivamv2 { get; set; }
        public decimal Ivav3 { get; set; }
        public decimal Ivamv3 { get; set; }
        public decimal Ivav4 { get; set; }
        public decimal Ivamv4 { get; set; }
        public decimal Ivav5 { get; set; }
        public decimal Ivamv5 { get; set; }
        public decimal Ivav6 { get; set; }
        public decimal Ivamv6 { get; set; }
        public decimal Ivav7 { get; set; }
        public decimal Ivamv7 { get; set; }
        public decimal Ivav8 { get; set; }
        public decimal Ivamv8 { get; set; }
        public decimal Ivav9 { get; set; }
        public decimal Ivamv9 { get; set; }
        public decimal Ttiliq { get; set; }
        public decimal Tmiliq { get; set; }
        public decimal Ttiva { get; set; }
        public decimal Tmiva { get; set; }
        public decimal Descc { get; set; }
        public decimal Descm { get; set; }
        public decimal Debreg { get; set; }
        public decimal Debregm { get; set; }
        public string Intid { get; set; } = null!;
        public string Nome2 { get; set; } = null!;
        public string Lrstamp { get; set; } = null!;
        public string Lpstamp { get; set; } = null!;
        public string Tpstamp { get; set; } = null!;
        public string Tpdesc { get; set; } = null!;
        public string Snstamp { get; set; } = null!;
        public decimal Rdtotal { get; set; }
        public decimal Erdtotal { get; set; }
        public decimal Rdtotalm { get; set; }
        public string Mhstamp { get; set; } = null!;
        public DateTime Dplano { get; set; }
        public string Dinoplano { get; set; } = null!;
        public decimal Dilnoplano { get; set; }
        public decimal Diaplano { get; set; }
        public bool Planoonline { get; set; }
        public string Dostamp { get; set; } = null!;
        public bool Optri { get; set; }
        public bool Meiost { get; set; }
        public DateTime Chdata { get; set; }
        public string Pscm { get; set; } = null!;
        public decimal Zncm { get; set; }
        public decimal Excm { get; set; }
        public string Ptcm { get; set; } = null!;
        public string Encm { get; set; } = null!;
        public decimal Ntcm { get; set; }
        public string Pscmdesc { get; set; } = null!;
        public string Znregiao { get; set; } = null!;
        public string Excmdesc { get; set; } = null!;
        public string Ptcmdesc { get; set; } = null!;
        public string Encmdesc { get; set; } = null!;
        public bool Ncin { get; set; }
        public bool Ncout { get; set; }
        public bool Usaintra { get; set; }
        public bool Iectisento { get; set; }
        public string Series { get; set; } = null!;
        public string Series2 { get; set; } = null!;
        public decimal Cambio { get; set; }
        public string Site { get; set; } = null!;
        public string Pnome { get; set; } = null!;
        public decimal Pno { get; set; }
        public string Cxstamp { get; set; } = null!;
        public string Cxusername { get; set; } = null!;
        public string Ssstamp { get; set; } = null!;
        public string Ssusername { get; set; } = null!;
        public bool Anulado { get; set; }
        public string Rpclstamp { get; set; } = null!;
        public string Rpclnome { get; set; } = null!;
        public DateTime Rpcldini { get; set; }
        public DateTime Rpcldfim { get; set; }
        public string Classe { get; set; } = null!;
        public bool Procomss { get; set; }
        public string Eanft { get; set; } = null!;
        public string Eancl { get; set; } = null!;
        public string Lang { get; set; } = null!;
        public string Tptit { get; set; } = null!;
        public decimal Virs { get; set; }
        public decimal Evirs { get; set; }
        public decimal Valorm2 { get; set; }
        public string Arstamp { get; set; } = null!;
        public decimal Arno { get; set; }
        public string Iecacodisen { get; set; } = null!;
        public string Niec { get; set; } = null!;
        public string Iecadoccod { get; set; } = null!;
        public bool Aprovado { get; set; }
        public bool Nprotri { get; set; }
        public decimal Ftid { get; set; }
        public string UAltft { get; set; } = null!;
        public decimal UCambio { get; set; }
        public string Ousrinis { get; set; } = null!;
        public DateTime Ousrdata { get; set; }
        public string Ousrhora { get; set; } = null!;
        public string Usrinis { get; set; } = null!;
        public DateTime Usrdata { get; set; }
        public string Usrhora { get; set; } = null!;
        public bool Marcada { get; set; }
        public string Mrstamp { get; set; } = null!;
        public bool Idft { get; set; }
        public string UNomepac { get; set; } = null!;
        public string UCodaut { get; set; } = null!;
        public decimal UIdpac { get; set; }
        public DateTime UDnp { get; set; }
        public string UNbf { get; set; } = null!;
    }
}
