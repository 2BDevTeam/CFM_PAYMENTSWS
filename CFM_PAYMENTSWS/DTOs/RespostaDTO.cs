using System.Text.Json.Serialization;

namespace CFM_PAYMENTSWS.DTOs
{
    public class RespostaDTO
    {


        public RespostaDTO(string responseId,string codigo, string estado, string descricao)
        {
            Estado = estado;
            Codigo = codigo;
            Descricao = descricao;
            Id = responseId;
        }
        public RespostaDTO(string responseId, string codigo, string descricao)
        {
            Codigo = codigo;
            Descricao = descricao;
            Id = responseId;
        }

        public RespostaDTO(string id, ResponseCodesDTO responseCodes)
        {
            Codigo = responseCodes.cod;
            Descricao = responseCodes.codDesc;
            Id = id;
        }
        public RespostaDTO(string id, ResponseCodesDTO responseCodes, string param1)
        {
            Codigo = responseCodes.cod;
            Descricao = responseCodes.codDesc.Replace("{0}", param1);
            Id = id;
        }

        public RespostaDTO(string id, ResponseCodesDTO responseCodes, string param1, string param2)
        {
            Codigo = responseCodes.cod;
            Descricao = responseCodes.codDesc.Replace("{0}", param1).Replace("{1}", param2);
            Id = id;
        }

        public RespostaDTO()
        {
        }


        public string Id { get; set; }
        public string Codigo { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Estado { get; set; }
        public string Descricao { get; set; }
        public override string ToString() => Newtonsoft.Json.JsonConvert.SerializeObject(this);
    }
}
