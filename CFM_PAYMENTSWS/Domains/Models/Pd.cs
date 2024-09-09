using System;
using System.Collections.Generic;

namespace CFM_PAYMENTSWS.Domains.Models
{
    public partial class Pd
    {
        public string Pdstamp { get; set; } = null!;
        public decimal Rno { get; set; }
        public DateTime Rdata { get; set; }
        public string Nome { get; set; } = null!;
        public decimal Total { get; set; }
        public decimal Etotal { get; set; }
        public decimal No { get; set; }
        public string Morada { get; set; } = null!;
        public string Local { get; set; } = null!;
        public string Codpost { get; set; } = null!;
        public string Ncont { get; set; } = null!;
        public decimal Pdano { get; set; }
        public string Olcodigo { get; set; } = null!;
        public string Telocal { get; set; } = null!;
        public decimal Totalmoeda { get; set; }
        public string Moeda { get; set; } = null!;
        public string Descricao { get; set; } = null!;
        public string Fref { get; set; } = null!;
        public string Ccusto { get; set; } = null!;
        public string Ncusto { get; set; } = null!;
        public decimal Contado { get; set; }
        public string Nib { get; set; } = null!;
        public decimal Base { get; set; }
        public decimal Basemoeda { get; set; }
        public string Zona { get; set; } = null!;
        public string Ollocal { get; set; } = null!;
        public bool Plano { get; set; }
        public bool Integrado { get; set; }
        public string Segmento { get; set; } = null!;
        public decimal Ebase { get; set; }
        public string Tipo { get; set; } = null!;
        public decimal Pais { get; set; }
        public decimal Estab { get; set; }
        public string Memissao { get; set; } = null!;
        public decimal Iva { get; set; }
        public decimal Tabiva { get; set; }
        public decimal Ivav { get; set; }
        public decimal Ivavmoeda { get; set; }
        public decimal Eivav { get; set; }
        public decimal Cm { get; set; }
        public string Cmdesc { get; set; } = null!;
        public string Adoc { get; set; } = null!;
        public bool Incerto { get; set; }
        public decimal Tipoad { get; set; }
        public decimal Pdid { get; set; }
        public string Intid { get; set; } = null!;
        public DateTime Dplano { get; set; }
        public string Dinoplano { get; set; } = null!;
        public decimal Dilnoplano { get; set; }
        public decimal Diaplano { get; set; }
        public bool Planoonline { get; set; }
        public string Dostamp { get; set; } = null!;
        public string Moeda2 { get; set; } = null!;
        public decimal Valorm2 { get; set; }
        public string Processo { get; set; } = null!;
        public string Subproc { get; set; } = null!;
        public string Crend { get; set; } = null!;
        public decimal Txirs { get; set; }
        public decimal Evirs { get; set; }
        public decimal Virs { get; set; }
        public bool Sujirsisen { get; set; }
        public string Czonag { get; set; } = null!;
        public bool Sujirs { get; set; }
        public bool Bempr { get; set; }
        public bool Exportado { get; set; }
        public string Site { get; set; } = null!;
        public string Pnome { get; set; } = null!;
        public decimal Pno { get; set; }
        public string Cxstamp { get; set; } = null!;
        public string Cxusername { get; set; } = null!;
        public string Ssstamp { get; set; } = null!;
        public string Ssusername { get; set; } = null!;
        public string Identdecexp { get; set; } = null!;
        public bool Jatrfonline { get; set; }
        public bool Cambiofixo { get; set; }
        public decimal Cambio { get; set; }
        public string Formapag { get; set; } = null!;
        public decimal UPdcam { get; set; }
        public decimal UPdtot { get; set; }
        public decimal UMcam { get; set; }
        public string UAtt { get; set; } = null!;
        public string UBopstamp { get; set; } = null!;
        public string UBostamp { get; set; } = null!;
        public decimal UCm { get; set; }
        public decimal UDevoluc { get; set; }
        public decimal UNocolabo { get; set; }
        public string UColaborr { get; set; } = null!;
        public string UFollocal { get; set; } = null!;
        public string UNref { get; set; } = null!;
        public string UMpag { get; set; } = null!;
        public bool UAnulado { get; set; }
        public decimal UTotal { get; set; }
        public decimal UTotalmoe { get; set; }
        public string Ousrinis { get; set; } = null!;
        public DateTime Ousrdata { get; set; }
        public string Ousrhora { get; set; } = null!;
        public string Usrinis { get; set; } = null!;
        public DateTime Usrdata { get; set; }
        public string Usrhora { get; set; } = null!;
        public bool Marcada { get; set; }
        public string Cecope { get; set; } = null!;
        public bool Operext { get; set; }
        public bool UImpresso { get; set; }
        public string UDesc { get; set; } = null!;
        public string Modalidade { get; set; } = null!;
        public bool UOwirps { get; set; }
        public string Cheque { get; set; } = null!;
        public DateTime Dataven { get; set; }
        public string Chfollocal { get; set; } = null!;
        public string Chfstamp { get; set; } = null!;
        public decimal Chfvalor { get; set; }
        public decimal Echfvalor { get; set; }
        public string UMotivtrf { get; set; } = null!;
        public bool UIntgrok { get; set; }
        public string UNomecheq { get; set; } = null!;
        public string UContacto { get; set; } = null!;
        public bool UCheqentr { get; set; }
        public DateTime UDtcheque { get; set; }
        public string UBenef { get; set; } = null!;
        public string UFicheiro { get; set; } = null!;
        public string UNome { get; set; } = null!;
        public string UBalcao { get; set; } = null!;
        public string UNib { get; set; } = null!;
        public string UBanco { get; set; } = null!;
        public string UConta { get; set; } = null!;
        public bool USendpay { get; set; }
        public string UUsrtrf { get; set; } = null!;
        public DateTime UDatatrf { get; set; }
        public string URefbanco { get; set; } = null!;
    }
}
