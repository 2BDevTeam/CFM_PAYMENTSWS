using Newtonsoft.Json;

namespace LENMEDWS.DTOs
{
    public class GetBillingOfLadingRequestDTO
    {
        public string voyageNumber { get; set; }
        public string billOfLadingNumber { get; set; }
        public string operation { get; set; }
        public string type { get; set; }
        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
