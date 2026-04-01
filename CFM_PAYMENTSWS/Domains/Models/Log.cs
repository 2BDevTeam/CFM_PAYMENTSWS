using CFM_PAYMENTSWS.Extensions;

namespace CFM_PAYMENTSWS.Domains.Models
{
    public class Log
    {
        public string u_logstamp { get; set; } = 25.UseThisSizeForStamp();
        public string? Code { get; set; }
        public string Ip { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string Responsetext { get; set; } = null!;
        public DateTime? Data { get; set; }
        public string Operation { get; set; } = null!;
        public string? RequestId { get; set; }
        public string? ResponseDesc { get; set; }
        
        // Novos campos adicionados
        public string? LogLevel { get; set; }
        public string? SourceBank { get; set; }
        public string? HttpMethod { get; set; }
        public int? HttpStatusCode { get; set; }
        public int? DurationMs { get; set; }
        public string? EndpointUrl { get; set; }
        public string? ProcessingStep { get; set; }
        public string? ErroCompleto { get; set; }
    }
}
