using CFM_PAYMENTSWS.Extensions;

namespace CFM_PAYMENTSWS.Domains.Models
{
    public class ApiKey
    {

        public string u_apikeystamp { get; set; } = 25.UseThisSizeForStamp();
        public string? apikey { get; set; }
        public DateTime? createdAt { get; set; } = DateTime.Now;
        public string? entity { get; set; }

    }
}
