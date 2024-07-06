using Newtonsoft.Json;

namespace LENMEDWS.DTOs
{
    public class ResponseCodesDTO
    {
        public ResponseCodesDTO() // Default constructor
        {
            // Initialize default values here if needed
        }
        public ResponseCodesDTO(string cod, string codDesc, decimal id)
        {
            this.cod = cod;
            this.codDesc = codDesc;
            this.id = id;
        }
        public ResponseCodesDTO(string cod, string codDesc)
        {
            this.cod = cod;
            this.codDesc = codDesc;
         
        }

        public string cod { get; set; }
        public decimal? id { get; set; }

        public string codDesc { get; set; }
        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
