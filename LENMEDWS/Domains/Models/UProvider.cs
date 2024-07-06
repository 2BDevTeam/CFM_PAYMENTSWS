using Newtonsoft.Json;

namespace LENMEDWS.Domains.Models
{
    public class UProvider
    {
        public string chave { get; set; }
        public string u_providerstamp { get; set; }
        public string valor { get; set; }
        public decimal? codigo { get; set; }
        public string grupo { get; set; }
        public override string ToString() => JsonConvert.SerializeObject(this);

   
    }
}
