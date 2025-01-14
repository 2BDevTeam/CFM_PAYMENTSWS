using Newtonsoft.Json;

namespace CFM_PAYMENTSWS.DTOs
{
    public class ResponseSmallDTO
    {
        public ResponseSmallDTO()
        {
        }


        public ResponseSmallDTO(string code, string description)
        {
            this.code = code;
            this.description = description;
        }
        public string code { get; set; }
        public string description { get; set; }
        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
