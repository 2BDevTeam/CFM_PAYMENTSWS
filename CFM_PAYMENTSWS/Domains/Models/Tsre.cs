using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CFM_PAYMENTSWS.Domains.Models;

public partial class Tsre
{
    public string Tsrestamp { get; set; } = null!;

    public string Nmdoc { get; set; } = null!;

    public decimal Ndoc { get; set; }

    public decimal Cmcc { get; set; }

    public string Cmccn { get; set; } = null!;

    public string Oldoc { get; set; } = null!;

    public string Xddesc { get; set; } = null!;

    public string Xdstamp { get; set; } = null!;

    public string Ousrinis { get; set; } = null!;

    public DateTime Ousrdata { get; set; }

    public string Ousrhora { get; set; } = null!;

    public string Usrinis { get; set; } = null!;

    public DateTime Usrdata { get; set; }

    public string Usrhora { get; set; } = null!;

    public bool Marcada { get; set; }

    public decimal Ndino { get; set; }

    public string Ndidesc { get; set; } = null!;

    public decimal Ndcno { get; set; }

    public string Ndcdesc { get; set; } = null!;

    public bool Automl { get; set; }

    public bool Intrord { get; set; }

    public string Serierd { get; set; } = null!;

    public decimal Nserierd { get; set; }

    public bool Introfac { get; set; }

    public bool Intropag { get; set; }

    public bool Movinc { get; set; }

    public bool Serieox { get; set; }

    public bool Docsimport { get; set; }

    public bool Ivacaixa { get; set; }

    public bool Clivacaixa { get; set; }

    public bool Manternumero { get; set; }

    public decimal Led { get; set; }

    public bool UOnlinep { get; set; }

    public override string ToString() => JsonConvert.SerializeObject(this);

}
