using Newtonsoft.Json;

namespace LENMEDWS.DTOs
{
    public class ResponseDTO
    {
        public ResponseDTO()
        {
        }

        public ResponseDTO(HeaderDTO headerDTO, ResponseCodesDTO response, object? data, object? content)
        {
            this.response = response;
            Header = headerDTO;
            Data = data;
            Content = content;
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
