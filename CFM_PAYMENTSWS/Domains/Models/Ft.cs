using System;
using System.Collections.Generic;

namespace CFM_PAYMENTSWS.Domains.Models;

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
    public DateTime Bidata { get; set; }
    public string Bilocal { get; set; } = null!;
    public string Telefone { get; set; } = null!;
    public string Zona { get; set; } = null!;
    public decimal Vendedor { get; set; }
    public string Vendnm { get; set; } = null!;
    public DateTime Fdata { get; set; }
    public decimal Ftano { get; set; }
    public decimal Ndoc { get; set; }
    public decimal Tipodoc { get; set; }
    public decimal Total { get; set; }
    public decimal Ttiliq { get; set; }
    public decimal Tmiliq { get; set; }
    public decimal Ttiva { get; set; }
    public decimal Descc { get; set; }
    public decimal Descm { get; set; }
    public bool Anulado { get; set; }
    public decimal Ftid { get; set; }
    public string Ousrinis { get; set; } = null!;
    public DateTime Ousrdata { get; set; }
    public string Ousrhora { get; set; } = null!;
    public string Usrinis { get; set; } = null!;
    public DateTime Usrdata { get; set; }
    public string Usrhora { get; set; } = null!;
}