using System;
using System.Collections.Generic;

namespace CFM_PAYMENTSWS.Domains.Models;

public partial class Cc
{
    public string Ccstamp { get; set; } = null!;

    public DateTime Datalc { get; set; }

    public DateTime Dataven { get; set; }

    public string Cmdesc { get; set; } = null!;

    public decimal Nrdoc { get; set; }

    public decimal Deb { get; set; }

    public decimal Cred { get; set; }

    public decimal Edeb { get; set; }

    public decimal Ecred { get; set; }

    public string Nome { get; set; } = null!;

    public string Moeda { get; set; } = null!;

    public decimal No { get; set; }

    public decimal Cm { get; set; }

    public decimal Cr { get; set; }

    public decimal Docref { get; set; }

    public decimal Debf { get; set; }

    public decimal Edebf { get; set; }

    public decimal Credf { get; set; }

    public decimal Ecredf { get; set; }

    public string Fref { get; set; } = null!;

    public string Ccusto { get; set; } = null!;

    public string Ncusto { get; set; } = null!;

    public string Origem { get; set; } = null!;

    public decimal Debm { get; set; }

    public decimal Credm { get; set; }

    public decimal Debfm { get; set; }

    public decimal Credfm { get; set; }

    public string Zona { get; set; } = null!;

    public bool Recibado { get; set; }

    public decimal Vendedor { get; set; }

    public string Vendnm { get; set; } = null!;

    public string Crdesc { get; set; } = null!;

    public bool Incobra { get; set; }

    public decimal Ftndoc { get; set; }

    public decimal Ftfno { get; set; }

    public string Obs { get; set; } = null!;

    public string Obscob { get; set; } = null!;

    public bool Fmarcada { get; set; }

    public string Segmento { get; set; } = null!;

    public decimal Recino { get; set; }

    public decimal Recian { get; set; }

    public string Odstamp { get; set; } = null!;

    public decimal Escvtmp { get; set; }

    public decimal Vtmp { get; set; }

    public decimal Evtmp { get; set; }

    public string Cobranca { get; set; } = null!;

    public string Ftstamp { get; set; } = null!;

    public string Restamp { get; set; } = null!;

    public bool Cambiofixo { get; set; }

    public decimal Edifcambio { get; set; }

    public decimal Difcambio { get; set; }

    public string Tipo { get; set; } = null!;

    public decimal Pais { get; set; }

    public decimal Estab { get; set; }

    public decimal Ivav1 { get; set; }

    public decimal Ivav2 { get; set; }

    public decimal Ivav3 { get; set; }

    public decimal Ivav4 { get; set; }

    public decimal Ivav5 { get; set; }

    public decimal Ivav6 { get; set; }

    public decimal Eivav1 { get; set; }

    public decimal Eivav2 { get; set; }

    public decimal Eivav3 { get; set; }

    public decimal Eivav4 { get; set; }

    public decimal Eivav5 { get; set; }

    public decimal Eivav6 { get; set; }

    public string Ftccstamp { get; set; } = null!;

    public string Rdstamp { get; set; } = null!;

    public string Cobrador { get; set; } = null!;

    public string Rota { get; set; } = null!;

    public string Clbanco { get; set; } = null!;

    public string Clcheque { get; set; } = null!;

    public decimal Valch { get; set; }

    public decimal Evalch { get; set; }

    public decimal Valre { get; set; }

    public decimal Evalre { get; set; }

    public decimal Mvalre { get; set; }

    public string Intid { get; set; } = null!;

    public string Lestamp { get; set; } = null!;

    public string Lrstamp { get; set; } = null!;

    public string Lmstamp { get; set; } = null!;

    public string Tpstamp { get; set; } = null!;

    public string Tpdesc { get; set; } = null!;

    public string Chstamp { get; set; } = null!;

    public decimal Covezes { get; set; }

    public decimal Virs { get; set; }

    public decimal Evirs { get; set; }

    public decimal Virsreg { get; set; }

    public decimal Evirsreg { get; set; }

    public decimal Irsdif { get; set; }

    public decimal Eirsdif { get; set; }

    public decimal Difccont { get; set; }

    public decimal Edifccont { get; set; }

    public string Ousrinis { get; set; } = null!;

    public DateTime Ousrdata { get; set; }

    public string Ousrhora { get; set; } = null!;

    public string Usrinis { get; set; } = null!;

    public DateTime Usrdata { get; set; }

    public string Usrhora { get; set; } = null!;

    public bool Marcada { get; set; }

    public decimal Ivav7 { get; set; }

    public decimal Ivav8 { get; set; }

    public decimal Ivav9 { get; set; }

    public decimal Eivav7 { get; set; }

    public decimal Eivav8 { get; set; }

    public decimal Eivav9 { get; set; }

    public string Faccstamp { get; set; } = null!;

    public string Faclstamp { get; set; } = null!;


    public string Occstamp { get; set; } = null!;


    public bool Dispcbb { get; set; }

    public decimal Cbbno { get; set; }

    public decimal Ivatx1 { get; set; }

    public decimal Ivatx2 { get; set; }

    public decimal Ivatx3 { get; set; }

    public decimal Ivatx4 { get; set; }

    public decimal Ivatx5 { get; set; }

    public decimal Ivatx6 { get; set; }

    public decimal Ivatx7 { get; set; }

    public decimal Ivatx8 { get; set; }

    public decimal Ivatx9 { get; set; }

    public bool Reexgiva { get; set; }

    public string Formapag { get; set; } = null!;

    public string Situacao { get; set; } = null!;

    public string Facstamp { get; set; } = null!;

    public bool Cobradovunicre { get; set; }

    public decimal Recindoc { get; set; }

    public decimal Ccndoc { get; set; }

    public string Ccnmdoc { get; set; } = null!;

    public bool Cobradovpaypal { get; set; }

    public DateTime Datasup12 { get; set; }

    public string Cecope { get; set; } = null!;

    public bool Operext { get; set; }

    public string Rerevstamp { get; set; } = null!;

    public decimal Eivacativado { get; set; }

    public decimal Ivacativado { get; set; }

    public decimal Eivacatreg { get; set; }

    public decimal Ivacatreg { get; set; }

    public decimal Eivacatdif { get; set; }

    public decimal Ivacatdif { get; set; }

    public decimal Mivacativado { get; set; }

    public decimal Mivacatreg { get; set; }

    public decimal Mivacatdif { get; set; }

}
