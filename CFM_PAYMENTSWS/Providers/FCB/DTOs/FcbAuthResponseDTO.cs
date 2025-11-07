using Newtonsoft.Json;

namespace CFM_PAYMENTSWS.Providers.FCB.DTOs
{
    public class FcbAuthResponseDTO
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; } = string.Empty;

        [JsonProperty("token_type")]
        public string TokenType { get; set; } = string.Empty;

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; } = string.Empty;

        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
