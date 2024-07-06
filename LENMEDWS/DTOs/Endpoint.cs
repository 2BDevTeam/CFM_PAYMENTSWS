using Newtonsoft.Json;

namespace LENMEDWS.DTOs
{
    public class Endpoint
    {

      
        public string operationCode { get; set; }
        public string method { get; set; }
        public string url { get; set; }
        public string contentType { get; set; }
        public Credentials credentials { get; set; }
        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
