using System;
using System.Collections.Generic;

namespace LENMEDWS.Domains.Models
{
    public partial class Ft3
    {
        public string Ft3stamp { get; set; } = null!;
        public string Codpais { get; set; } = null!;
        public string Descpais { get; set; } = null!;
        public string C2codpais { get; set; } = null!;
        public string C2descpais { get; set; } = null!;
        public decimal Ppperiodo { get; set; }
        public string Ppperiodicidade { get; set; } = null!;
        public string Ppbillingagreement { get; set; } = null!;
        public bool Ppallowsubscription { get; set; }
        public bool Ppallowpaynow { get; set; }
        public bool Subscritovpaypal { get; set; }
        public string Ppprofileid { get; set; } = null!;
        public string Pptransactionid { get; set; } = null!;
        public string Ppsbstamp { get; set; } = null!;
        public bool Tpapurchase { get; set; }
        public string Tpaposid { get; set; } = null!;
        public string Tpatype { get; set; } = null!;
        public string Tpaaccountperiodnum { get; set; } = null!;
        public string Tpatransactionseqnum { get; set; } = null!;
        public string Tpamessagenum { get; set; } = null!;
        public string Tparesponsedatetime { get; set; } = null!;
        public string Tpaamountconverted { get; set; } = null!;
        public string Tpapan { get; set; } = null!;
        public string Tpasan { get; set; } = null!;
        public string Tpaissuername { get; set; } = null!;
        public decimal Tpareceiptformat { get; set; }
        public string Tpacommissionsignal { get; set; } = null!;
        public string Tpacommissionvalue { get; set; } = null!;
        public string Tpatextforclientreceipt { get; set; } = null!;
        public string Tpareceipt { get; set; } = null!;
        public string Tpamreceipt { get; set; } = null!;
        public string Walletreceiptid { get; set; } = null!;
        public string Motanul { get; set; } = null!;
        public string Invoicenoori { get; set; } = null!;
        public string Anexo40 { get; set; } = null!;
        public string Anexo41 { get; set; } = null!;
        public string Distrito { get; set; } = null!;
        public string C2distrito { get; set; } = null!;
        public DateTime Taxpointdt { get; set; }
        public string Motretif { get; set; } = null!;
        public bool Naoisenta { get; set; }
        public string Cadmintipo1 { get; set; } = null!;
        public string Cadmintipo1stamp { get; set; } = null!;
        public string Cadmintipo2 { get; set; } = null!;
        public string Cadmintipo2stamp { get; set; } = null!;
        public string Cadmintipo3 { get; set; } = null!;
        public string Cadmintipo3stamp { get; set; } = null!;
        public string Cadmintipo4 { get; set; } = null!;
        public string Cadmintipo4stamp { get; set; } = null!;
        public string Motorista { get; set; } = null!;
        public decimal Fttxirs { get; set; }
        public string Barcode { get; set; } = null!;
        public decimal Totrc { get; set; }
        public decimal Etotrc { get; set; }
        public decimal Mtotrc { get; set; }
        public decimal Totgroj { get; set; }
        public decimal Etotgroj { get; set; }
        public decimal Mtotgroj { get; set; }
        public string Ousrinis { get; set; } = null!;
        public DateTime Ousrdata { get; set; }
        public string Ousrhora { get; set; } = null!;
        public string Usrinis { get; set; } = null!;
        public DateTime Usrdata { get; set; }
        public string Usrhora { get; set; } = null!;
        public bool Marcada { get; set; }
        public string UAnomal { get; set; } = null!;
        public string UCalibre { get; set; } = null!;
        public string UCalpstp { get; set; } = null!;
        public string UContador { get; set; } = null!;
        public decimal UFactu { get; set; }
        public decimal UFactura { get; set; }
        public decimal ULeiact { get; set; }
        public decimal ULeiant { get; set; }
        public bool UMultado { get; set; }
        public string UOristamp { get; set; } = null!;
        public string UPeriodo { get; set; } = null!;
        public decimal UReal { get; set; }
        public string UTipofac { get; set; } = null!;
        public decimal UTx1 { get; set; }
        public bool Cobradovmbway { get; set; }
        public decimal Latitudecarga { get; set; }
        public decimal Longitudecarga { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal Tfivatx1 { get; set; }
        public decimal Tfivatx2 { get; set; }
        public decimal Tfivatx3 { get; set; }
        public decimal Tfivatx4 { get; set; }
        public decimal Tfivatx5 { get; set; }
        public decimal Tfivatx6 { get; set; }
        public decimal Tfivatx7 { get; set; }
        public decimal Tfivatx8 { get; set; }
        public decimal Tfivatx9 { get; set; }
        public decimal Tfeivav1 { get; set; }
        public decimal Tfeivav2 { get; set; }
        public decimal Tfeivav3 { get; set; }
        public decimal Tfeivav4 { get; set; }
        public decimal Tfeivav5 { get; set; }
        public decimal Tfeivav6 { get; set; }
        public decimal Tfeivav7 { get; set; }
        public decimal Tfeivav8 { get; set; }
        public decimal Tfeivav9 { get; set; }
        public decimal Tfeivain1 { get; set; }
        public decimal Tfeivain2 { get; set; }
        public decimal Tfeivain3 { get; set; }
        public decimal Tfeivain4 { get; set; }
        public decimal Tfeivain5 { get; set; }
        public decimal Tfeivain6 { get; set; }
        public decimal Tfeivain7 { get; set; }
        public decimal Tfeivain8 { get; set; }
        public decimal Tfeivain9 { get; set; }
        public string Tfdoctipo { get; set; } = null!;
        public string Tfdocnum { get; set; } = null!;
        public string Tfdocpais { get; set; } = null!;
        public string Tfdocpaisdesc { get; set; } = null!;
        public DateTime Tfdatanasc { get; set; }
        public bool Taxfree { get; set; }
        public decimal Tfpaisori { get; set; }
        public decimal Tfrefund { get; set; }
        public decimal Tfserviceid { get; set; }
        public string Tfdocid { get; set; } = null!;
        public decimal Invoiceyear { get; set; }
        public decimal Tfgrossamount { get; set; }
        public string Anulinis { get; set; } = null!;
        public DateTime Anuldata { get; set; }
        public string Anulhora { get; set; } = null!;
        public DateTime Dplano { get; set; }
        public string Dinoplano { get; set; } = null!;
        public decimal Dilnoplano { get; set; }
        public decimal Diaplano { get; set; }
        public bool Planoonline { get; set; }
        public string Dostamp { get; set; } = null!;
        public bool Plano { get; set; }
        public DateTime Refmbdtvalidade { get; set; }
        public string Oriinis { get; set; } = null!;
        public DateTime Oridata { get; set; }
        public string Orihora { get; set; } = null!;
        public string Oricae { get; set; } = null!;
        public decimal Cativaperc { get; set; }
        public decimal Eivacativado { get; set; }
        public decimal Ivacativado { get; set; }
        public decimal Mivacativado { get; set; }
        public string Region { get; set; } = null!;
        public string Anularetif { get; set; } = null!;
        public string Fiscalcode { get; set; } = null!;
        public decimal Seriecode { get; set; }
        public decimal Docno { get; set; }
        public string Atcud { get; set; } = null!;
        public decimal Contingencia { get; set; }
        public decimal Codmotivreg { get; set; }
        public string Motivreg { get; set; } = null!;
        public string Motivregoutro { get; set; } = null!;
        public string Codendereco { get; set; } = null!;
        public decimal Meiotranscv { get; set; }
        public decimal UEtotliq { get; set; }
        public decimal UTotliq { get; set; }
        public decimal UTotliqm { get; set; }
        public string UGestcont { get; set; } = null!;
        public string UGestnome { get; set; } = null!;
        public decimal UWwfid { get; set; }
        public string UWwfstamp { get; set; } = null!;
        public string UOftstamp { get; set; } = null!;
        public DateTime UOfdata { get; set; }
    }
}
