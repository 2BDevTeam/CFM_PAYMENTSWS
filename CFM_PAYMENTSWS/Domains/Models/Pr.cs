using System;
using System.Collections.Generic;

namespace CFM_PAYMENTSWS.Domains.Models
{
    public partial class Pr
    {
        public string Prstamp { get; set; } = null!;
        public string Nome { get; set; } = null!;
        public decimal Recibo { get; set; }
        public DateTime Data { get; set; }
        public decimal Liquido { get; set; }
        public decimal Eliquido { get; set; }
        public decimal Dias { get; set; }
        public decimal Vencacum { get; set; }
        public decimal No { get; set; }
        public string Seccao { get; set; } = null!;
        public string Ncont { get; set; } = null!;
        public string Nbenef { get; set; } = null!;
        public decimal Basedia { get; set; }
        public decimal Basemes { get; set; }
        public decimal Ttsuj { get; set; }
        public decimal Ttnsuj { get; set; }
        public decimal Ttdesc { get; set; }
        public decimal Subsidio { get; set; }
        public decimal Bonus { get; set; }
        public decimal Acidente { get; set; }
        public DateTime Nasc { get; set; }
        public decimal Ttsujcx { get; set; }
        public decimal Horasmes { get; set; }
        public decimal Horasfalta { get; set; }
        public decimal Horasextra { get; set; }
        public string Obsss { get; set; } = null!;
        public bool Primeiro { get; set; }
        public decimal Txssp { get; set; }
        public decimal Txsse { get; set; }
        public bool Txirs { get; set; }
        public string Obsss2 { get; set; } = null!;
        public decimal Ssevalor { get; set; }
        public decimal Sshextra { get; set; }
        public decimal Codigo { get; set; }
        public string Mesref { get; set; } = null!;
        public string Ltrab { get; set; } = null!;
        public string Caixa { get; set; } = null!;
        public bool Contabil { get; set; }
        public decimal Recno { get; set; }
        public bool Plano { get; set; }
        public string Ccusto { get; set; } = null!;
        public string Fref { get; set; } = null!;
        public string Ncusto { get; set; } = null!;
        public string Sind { get; set; } = null!;
        public string Nosind { get; set; } = null!;
        public string Bairro { get; set; } = null!;
        public string Cval1 { get; set; } = null!;
        public string Cval2 { get; set; } = null!;
        public string Cval3 { get; set; } = null!;
        public string Cval4 { get; set; } = null!;
        public string Banco { get; set; } = null!;
        public string Noconta { get; set; } = null!;
        public decimal Status { get; set; }
        public decimal Sscodigo { get; set; }
        public decimal Hett { get; set; }
        public decimal Heqt { get; set; }
        public bool Notrf { get; set; }
        public decimal Irsacum { get; set; }
        public decimal Eirsacum { get; set; }
        public decimal Ehett { get; set; }
        public decimal Evencacum { get; set; }
        public decimal Ebasedia { get; set; }
        public decimal Ebasemes { get; set; }
        public decimal Ettsuj { get; set; }
        public decimal Ettnsuj { get; set; }
        public decimal Ettdesc { get; set; }
        public decimal Esubsidio { get; set; }
        public decimal Ebonus { get; set; }
        public decimal Eacidente { get; set; }
        public decimal Ettsujcx { get; set; }
        public decimal Essevalor { get; set; }
        public decimal Esshextra { get; set; }
        public decimal Estab { get; set; }
        public string Nomecomp { get; set; } = null!;
        public string Tbcheque { get; set; } = null!;
        public bool Tbok { get; set; }
        public string Tbstamp { get; set; } = null!;
        public string Gpccheque { get; set; } = null!;
        public bool Gpcok { get; set; }
        public string Gpcstamp { get; set; } = null!;
        public string Ccategoria { get; set; } = null!;
        public string Seguro { get; set; } = null!;
        public bool Jaajirs { get; set; }
        public string Col9 { get; set; } = null!;
        public string Escala { get; set; } = null!;
        public decimal Sitprof { get; set; }
        public decimal Ncategoria { get; set; }
        public string Cprofiss { get; set; } = null!;
        public decimal Nhabil { get; set; }
        public string Nivel { get; set; } = null!;
        public string Ccprofiss { get; set; } = null!;
        public string Curso { get; set; } = null!;
        public string Abrsit { get; set; } = null!;
        public string Codtcontr { get; set; } = null!;
        public string Codrdtrab { get; set; } = null!;
        public DateTime Oldata { get; set; }
        public string Ollocal { get; set; } = null!;
        public decimal Telanno { get; set; }
        public string Telocal { get; set; } = null!;
        public decimal Contado { get; set; }
        public string Cheque { get; set; } = null!;
        public bool Lol { get; set; }
        public string Olcodigo { get; set; } = null!;
        public string Sgrupo { get; set; } = null!;
        public string Grupo { get; set; } = null!;
        public decimal Prid { get; set; }
        public string Intid { get; set; } = null!;
        public string Docnome { get; set; } = null!;
        public string Owdescricao { get; set; } = null!;
        public decimal Docno { get; set; }
        public bool Nada { get; set; }
        public string Area { get; set; } = null!;
        public decimal Ttsujcga { get; set; }
        public decimal Ettsujcga { get; set; }
        public decimal Ttsujadse { get; set; }
        public decimal Ettsujadse { get; set; }
        public bool Efuncp { get; set; }
        public bool Cgass { get; set; }
        public decimal Cgaevalor { get; set; }
        public decimal Ecgaevalor { get; set; }
        public string Moeda { get; set; } = null!;
        public decimal Mliquido { get; set; }
        public string Czonag { get; set; } = null!;
        public DateTime Dplano { get; set; }
        public string Dinoplano { get; set; } = null!;
        public decimal Dilnoplano { get; set; }
        public decimal Diaplano { get; set; }
        public decimal Ano { get; set; }
        public bool UDespcalc { get; set; }
        public string UCaixa { get; set; } = null!;
        public decimal UHrecibo { get; set; }
        public bool UImport { get; set; }
        public string UTcontr { get; set; } = null!;
        public decimal UCastsuj { get; set; }
        public decimal UCastxp { get; set; }
        public bool UNocas { get; set; }
        public string UEscalao { get; set; } = null!;
        public string UAltpr { get; set; } = null!;
        public string Ousrinis { get; set; } = null!;
        public DateTime Ousrdata { get; set; }
        public string Ousrhora { get; set; } = null!;
        public string Usrinis { get; set; } = null!;
        public DateTime Usrdata { get; set; }
        public string Usrhora { get; set; } = null!;
        public bool Marcada { get; set; }
        public decimal UMtliq { get; set; }
        public decimal UUsdliq { get; set; }
        public string UProcpor { get; set; } = null!;
        public bool ULastproc { get; set; }
        public string UClsfcfm { get; set; } = null!;
        public decimal UCambio { get; set; }
        public decimal UMinnac { get; set; }
        public decimal UNumproc { get; set; }
        public decimal UMintrib { get; set; }
        public bool UBonproc { get; set; }
        public decimal Adseevalor { get; set; }
        public decimal Eadseevalor { get; set; }
        public bool Pago { get; set; }
        public DateTime Dpago { get; set; }
        public string Nib { get; set; } = null!;
        public decimal Valorjaemtb { get; set; }
        public decimal Evalorjaemtb { get; set; }
        public decimal Valorol { get; set; }
        public decimal Evalorol { get; set; }
        public decimal Mvalorol { get; set; }
        public string UEvent { get; set; } = null!;
        public DateTime UDta { get; set; }
        public decimal Mvalorjaemtb { get; set; }
        public string Fgctmesstamp { get; set; } = null!;
        public decimal Efctvalor { get; set; }
        public decimal Fctvalor { get; set; }
        public decimal Efgctvalor { get; set; }
        public decimal Fgctvalor { get; set; }
        public decimal Efgctvalreemb { get; set; }
        public decimal Fgctvalreemb { get; set; }
        public DateTime Datafgctreemb { get; set; }
        public bool Fgctreemb { get; set; }
        public string Sepagh { get; set; } = null!;
        public string Sepapi { get; set; } = null!;
        public string EventoSupinfSs { get; set; } = null!;
        public DateTime DteventoSupinfSs { get; set; }
        public string UDistres { get; set; } = null!;
        public bool Resestr { get; set; }
        public decimal UNrantigo { get; set; }
        public decimal USstvalor { get; set; }
        public decimal Viansujirs { get; set; }
        public decimal Eviansujirs { get; set; }
        public decimal Ttnotliquido { get; set; }
        public decimal Ettnotliquido { get; set; }
        public decimal UTxxpe { get; set; }
        public decimal UValorxpe { get; set; }
        public string UNrcard { get; set; } = null!;
        public string UFicheiro { get; set; } = null!;
        public string UNome { get; set; } = null!;
        public string UCcusto { get; set; } = null!;
        public bool UExclinss { get; set; }
        public decimal UVlxpeatr { get; set; }
        public bool Jovemisentoirs { get; set; }
        public DateTime Datajovemisentoirs { get; set; }
        public decimal Ettsujart2b { get; set; }
        public decimal Ttsujart2b { get; set; }
    }
}
