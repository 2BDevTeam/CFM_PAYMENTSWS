using Newtonsoft.Json;

namespace LENMEDWS.DTOs
{
    public class ApiKey
    {
        public string empresa { get; set; }
        public string key { get; set; }
        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
