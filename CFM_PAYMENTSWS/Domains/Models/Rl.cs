using System;
using System.Collections.Generic;

namespace CFM_PAYMENTSWS.Domains.Models;

public partial class Rl
{
    public Rl(string restamp, string rlstamp, string ccstamp, string cdesc, decimal cm, DateTime datalc, DateTime dataven, decimal enaval, decimal eval, decimal escrec, decimal escval, decimal erec, decimal evori, string moeda, decimal val, decimal rec, decimal ndoc, decimal nrdoc, bool process, decimal rno, DateTime rdata, DateTime ousrdata, DateTime usrdata, string ousrhora, string usrhora, string ousrinis, string usrinis)
    {
        Restamp = restamp;
        Rlstamp = rlstamp;
        Ccstamp = ccstamp;
        Cdesc = cdesc;
        Cm = cm;
        Datalc = datalc;
        Dataven = dataven;
        Enaval = enaval;
        Eval = eval;
        Escrec = escrec;
        Escval = escval;
        Erec = erec;
        Evori = evori;
        Moeda = moeda;
        Val = val;
        Rec = rec;
        Ndoc = ndoc;
        Nrdoc = nrdoc;
        Process = process;
        Rno = rno;
        Rdata = rdata;
        Ousrdata = ousrdata;
        Usrdata = usrdata;
        Ousrhora = ousrhora;
        Usrhora = usrhora;
        Ousrinis = ousrinis;
        Usrinis = usrinis;
    }

    public string Rlstamp { get; set; } = null!;

    public decimal Ndoc { get; set; }

    public decimal Rno { get; set; }

    public string Cdesc { get; set; } = null!;

    public decimal Nrdoc { get; set; }

    public decimal Val { get; set; }

    public decimal Rec { get; set; }

    public DateTime Datalc { get; set; }

    public DateTime Dataven { get; set; }

    public string Restamp { get; set; } = null!;

    public string Ccstamp { get; set; } = null!;

    public decimal Cm { get; set; }

    public decimal Cambio { get; set; }

    public decimal Eval { get; set; }

    public decimal Erec { get; set; }

    public bool Process { get; set; }

    public string Moeda { get; set; } = null!;

    public DateTime Rdata { get; set; }

    public bool Cambiofixo { get; set; }

    public decimal Escval { get; set; }

    public decimal Escrec { get; set; }

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

    public decimal Lordem { get; set; }

    public decimal Earred { get; set; }

    public decimal Arred { get; set; }

    public decimal Enaval { get; set; }

    public decimal Naval { get; set; }

    public bool Cheque { get; set; }

    public decimal Desconto { get; set; }

    public decimal Vori { get; set; }

    public decimal Evori { get; set; }

    public decimal Mvori { get; set; }

    public decimal Ivavori1 { get; set; }

    public decimal Eivavori1 { get; set; }

    public decimal Ivavori2 { get; set; }

    public decimal Eivavori2 { get; set; }

    public decimal Ivavori3 { get; set; }

    public decimal Eivavori3 { get; set; }

    public decimal Ivavori4 { get; set; }

    public decimal Eivavori4 { get; set; }

    public decimal Ivavori5 { get; set; }

    public decimal Eivavori5 { get; set; }

    public decimal Ivavori6 { get; set; }

    public decimal Eivavori6 { get; set; }

    public bool Naoutiliza { get; set; }

    public string Moedoc { get; set; } = null!;

    public decimal Ecambio { get; set; }

    public decimal Cambiore { get; set; }

    public decimal Virs { get; set; }

    public decimal Evirs { get; set; }

    public string Ousrinis { get; set; } = null!;

    public DateTime Ousrdata { get; set; }

    public string Ousrhora { get; set; } = null!;

    public string Usrinis { get; set; } = null!;

    public DateTime Usrdata { get; set; }

    public string Usrhora { get; set; } = null!;

    public bool Marcada { get; set; }

    public decimal Ivav7 { get; set; }

    public decimal Eivav7 { get; set; }

    public decimal Ivav8 { get; set; }

    public decimal Eivav8 { get; set; }

    public decimal Ivav9 { get; set; }

    public decimal Eivav9 { get; set; }

    public decimal Ivavori7 { get; set; }

    public decimal Eivavori7 { get; set; }

    public decimal Ivavori8 { get; set; }

    public decimal Eivavori8 { get; set; }

    public decimal Ivavori9 { get; set; }

    public decimal Eivavori9 { get; set; }

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

    public decimal Vsujirs { get; set; }

    public decimal Evsujirs { get; set; }

    public string Cessao { get; set; } = null!;

    public string Faccstamp { get; set; } = null!;

    public decimal Vpercinc { get; set; }

    public decimal Virsori { get; set; }

    public decimal Evirsori { get; set; }

    public decimal Esirca { get; set; }

    public decimal Eivacativado { get; set; }

    public decimal Ivacativado { get; set; }

    public decimal Mivacativado { get; set; }
}
