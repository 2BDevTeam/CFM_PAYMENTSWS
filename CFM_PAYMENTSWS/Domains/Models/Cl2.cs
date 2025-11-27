using System;
using System.Collections.Generic;

namespace CFM_PAYMENTSWS.Domains.Models;

public partial class Cl2
{
    public string Cl2stamp { get; set; } = null!;

    public string Codpais { get; set; } = null!;

    public string Descpais { get; set; } = null!;

    public bool Adcsepaativa { get; set; }

    public bool Isb2b { get; set; }

    public bool Monitignios { get; set; }

    public string Nomemod { get; set; } = null!;

    public decimal Nomod { get; set; }

    public decimal Estabmod { get; set; }

    public string Cadmintipo1 { get; set; } = null!;

    public string Cadmintipo1stamp { get; set; } = null!;

    public string Cadmintipo2 { get; set; } = null!;

    public string Cadmintipo2stamp { get; set; } = null!;

    public string Cadmintipo3 { get; set; } = null!;

    public string Cadmintipo3stamp { get; set; } = null!;

    public string Cadmintipo4 { get; set; } = null!;

    public string Cadmintipo4stamp { get; set; } = null!;

    public bool Clivacaixa { get; set; }

    public string Tdocidcod { get; set; } = null!;

    public bool Tdocidenif { get; set; }

    public string Retarrat { get; set; } = null!;

    public string Pwportal { get; set; } = null!;

    public string Userid { get; set; } = null!;

    public DateTime Dlogin { get; set; }

    public string Hlogin { get; set; } = null!;

    public bool Dgrelhas { get; set; }

    public bool Dbpm { get; set; }

    public bool Doceletronicos { get; set; }

    public decimal Latitude { get; set; }

    public decimal Longitude { get; set; }

    public bool Cobrecsede { get; set; }

    public string Forgotid { get; set; } = null!;

    public DateTime Forgotdate { get; set; }

    public bool Termsconditions { get; set; }

    public string Egarprod { get; set; } = null!;

    public string Egarapa { get; set; } = null!;

    public string Egarpgl { get; set; } = null!;

    public string Egargrupo { get; set; } = null!;

    public string Egaropera { get; set; } = null!;

    public string Egaropdes { get; set; } = null!;

    public string Validadecartao { get; set; } = null!;

    public string Passpais { get; set; } = null!;

    public string Passpaisdesc { get; set; } = null!;

    

    public string Ousrinis { get; set; } = null!;

    public DateTime Ousrdata { get; set; }

    public string Ousrhora { get; set; } = null!;

    public string Usrinis { get; set; } = null!;

    public DateTime Usrdata { get; set; }

    public string Usrhora { get; set; } = null!;

    public bool Marcada { get; set; }

    public bool Refmbusavalidade { get; set; }

    public decimal Refmbtpdata { get; set; }

    public decimal Refmbndias { get; set; }

    public decimal Cativaperc { get; set; }

    public bool Nifvalidado { get; set; }


    public string Codendereco { get; set; } = null!;

}
