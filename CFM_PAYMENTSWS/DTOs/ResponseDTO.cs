using Newtonsoft.Json;

namespace CFM_PAYMENTSWS.DTOs
{
    public class ResponseDTO
    {
        public ResponseDTO()
        {
        }


        public ResponseDTO(ResponseCodesDTO response, object? data, object? content)
        {
            this.response = response;
            Data = data;
            Content = content;
        }
        public ResponseCodesDTO response { get; set; }
        public HeaderDTO Header { get; set; }
        public object? Data { get; set; }
        public object? Content { get; set; }
        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
