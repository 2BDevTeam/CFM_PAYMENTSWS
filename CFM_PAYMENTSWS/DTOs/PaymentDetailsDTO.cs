using System.ComponentModel.DataAnnotations;

namespace CFM_PAYMENTSWS.DTOs
{
    public class PaymentDetailsDTO
    {
        [Required(ErrorMessage = "O Campo Referencia é obrigatório")]
        public string Referencia { get; set; }

        [Required(ErrorMessage = "O Campo Valor é obrigatório")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "O Campo MSISDN é obrigatório")]
        public string MSISDN { get; set; }

        public string Tabela { get; set; }
        public string Oristamp { get; set; }
        public decimal Canal { get; set; }
    }
}
