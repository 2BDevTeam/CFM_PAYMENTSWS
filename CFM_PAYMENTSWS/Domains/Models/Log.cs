using CFM_PAYMENTSWS.Extensions;

namespace CFM_PAYMENTSWS.Domains.Models
{
    public class Log
    {
        public string u_logstamp { get; set; } = 25.UseThisSizeForStamp();
        public string? RequestId { get; set; }
        public DateTime? Data { get; set; }
        public string? Code { get; set; }
        public string? Content { get; set; }
        public string? ResponseDesc { get; set; }
        public string? Operation { get; set; }
    }
}
