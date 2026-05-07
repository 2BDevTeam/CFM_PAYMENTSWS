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

        [JsonProperty("error")]
        public string Error { get; set; } = string.Empty;

        [JsonProperty("error_description")]
        public string ErrorDescription { get; set; } = string.Empty;

        public int? HttpStatusCode { get; set; }
        public string RawPayload { get; set; } = string.Empty;
        public int AttemptCount { get; set; } = 0;

        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
