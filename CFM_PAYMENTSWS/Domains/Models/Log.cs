using CFM_PAYMENTSWS.Extensions;

namespace CFM_PAYMENTSWS.Domains.Models
{
    public class Log
    {
        public string u_logstamp { get; set; } = 25.UseThisSizeForStamp();
        public string? requestId { get; set; }
        public DateTime? data { get; set; }
        public string? code { get; set; }
        public string? content { get; set; }
        public string? responseDesc { get; set; }
        public string? operation { get; set; }
    }
}
