using Newtonsoft.Json;
namespace CFM_PAYMENTSWS.Providers.Bim.DTOs
{

    public class BimAuthResponse
    {
        public string token { get; set; }
        public string message { get; set; }
        public int statusCode { get; set; }
        public override string ToString() => JsonConvert.SerializeObject(this);
    }

}

