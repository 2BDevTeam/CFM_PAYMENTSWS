using Newtonsoft.Json;

namespace CFM_PAYMENTSWS.DTOs
{
    public class RespostaDTO
    {


        public RespostaDTO(decimal responseId,string codigo, string estado, string descricao)
        {
            Estado = estado;
            Codigo = codigo;
            Descricao = descricao;
            Id = responseId;
        }
        public RespostaDTO(string codigo, string estado, string descricao)
        {
            Estado = estado;
            Codigo = codigo;
            Descricao = descricao;
        }
        public RespostaDTO()
        {
        }


        public decimal Id { get; set; }
        public string Codigo { get; set; }
        public string Estado { get; set; }
        public string Descricao { get; set; }
        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
