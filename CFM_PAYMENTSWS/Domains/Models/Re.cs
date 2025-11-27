using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CFM_PAYMENTSWS.Domains.Models;

public partial class Re
{
    public Re(string restamp, string ccusto, DateTime chdata, decimal contado, decimal etotal, decimal etotow, string fref, string local, string memissao, string morada, string ncont, decimal ndoc, string nmdoc, decimal no, string nome, string olcodigo, string ollocal, DateTime ousrdata, DateTime usrdata, string ousrhora, string usrhora, string ousrinis, string usrinis, bool process, DateTime rdata, int reano, decimal rno, string segmento, string telocal, decimal total, decimal totow, DateTime procdata, string moeda, string UTransid, decimal UEntps, string URefps)
    {
        Restamp = restamp ?? "";
        Ccusto = ccusto ?? "";
        Chdata = chdata;
        Contado = contado;
        Etotal = etotal;
        Etotow = etotow;
        Fref = fref ?? "";
        Local = local ?? "";
        Memissao = memissao ?? "";
        Morada = morada ?? "";
        Ncont = ncont ?? "";
        Ndoc = ndoc;
        Nmdoc = nmdoc ?? "";
        No = no;
        Nome = nome ?? "";
        Olcodigo = olcodigo ?? "";
        Ollocal = ollocal ?? "";
        Ousrdata = ousrdata;
        Usrdata = usrdata;
        Ousrhora = ousrhora ?? "";
        Usrhora = usrhora ?? "";
        Ousrinis = ousrinis ?? "";
        Usrinis = usrinis ?? "";
        Process = process;
        Rdata = rdata;
        Reano = reano;
        Rno = rno;
        Segmento = segmento ?? "";
        Telocal = telocal ?? "";
        Total = total;
        Totow = totow;
        Procdata = procdata;
        Moeda = moeda ?? "";
        this.UTransid = UTransid ?? "";
        this.UEntps= UEntps;
        this.URefps = URefps ?? "";
    }

    public string Restamp { get; set; }
    public string Ccusto { get; set; }
    public DateTime Chdata { get; set; }
    public decimal Contado { get; set; }
    public decimal Etotal { get; set; }
    public decimal Etotow { get; set; }
    public string Fref { get; set; }
    public string Local { get; set; }
    public string Memissao { get; set; }
    public string Morada { get; set; }
    public string Ncont { get; set; }
    public decimal Ndoc { get; set; }
    public string Nmdoc { get; set; }
    public decimal No { get; set; }
    public string Nome { get; set; }
    public string Olcodigo { get; set; }
    public string Ollocal { get; set; }
    public DateTime Ousrdata { get; set; }
    public DateTime Usrdata { get; set; }
    public string Ousrhora { get; set; }
    public string Usrhora { get; set; }
    public string Ousrinis { get; set; }
    public string Usrinis { get; set; }
    public bool Process { get; set; }
    public DateTime Rdata { get; set; }
    public int Reano { get; set; }
    public decimal Rno { get; set; }
    public string Segmento { get; set; }
    public string Telocal { get; set; }
    public decimal Total { get; set; }
    public decimal Totow { get; set; }
    public DateTime Procdata { get; set; }
    public string Moeda { get; set; }
    public string UTransid { get; set; }
    public decimal UEntps { get; set; }
    public string URefps { get; set; }

    public override string ToString() => JsonConvert.SerializeObject(this);
}
