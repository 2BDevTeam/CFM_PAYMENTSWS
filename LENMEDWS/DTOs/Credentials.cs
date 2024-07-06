using Newtonsoft.Json;

namespace LENMEDWS
{
    public class Credentials
    {
 
        public string username { get; set; }
        public string password { get; set; }
        public string apiKey { get; set; }
        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
