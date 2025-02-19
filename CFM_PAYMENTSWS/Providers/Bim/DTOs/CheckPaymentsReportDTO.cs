using Newtonsoft.Json;

namespace CFM_PAYMENTSWS.Providers.Bim.DTOs
{
    public class CheckPaymentsReportDTO
    {
        public string BatchId { get; set; }
        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
