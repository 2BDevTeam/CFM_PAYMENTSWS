using Newtonsoft.Json;

namespace LENMEDWS.DTOs
{
    public class ErrorDTO
    {
        public string message { get; set; }
        public string stack { get; set; }
        public string inner { get; set; }
        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
