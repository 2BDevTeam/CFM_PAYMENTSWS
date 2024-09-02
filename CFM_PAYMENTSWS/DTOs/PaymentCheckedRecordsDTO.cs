using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace CFM_PAYMENTSWS.DTOs
{
    public class PaymentCheckedRecordsDTO
    {
        [Required(ErrorMessage = "TransactionId is required.")]
        public string TransactionId { get; set; }

       
        public string BankReference { get; set; }

        [Required(ErrorMessage = "StatusCode is required.")]
        [RegularExpression("^[0-9]{4}$", ErrorMessage = "StatusCode must be a 4-digit number.")]
        [Range(0, 9999, ErrorMessage = "StatusCode must be between 0000 and 9999.")]
        public string StatusCode { get; set; }

        [Required(ErrorMessage = "StatusDescription is required.")]
        public string StatusDescription { get; set; }

        public override string ToString() => JsonConvert.SerializeObject(this);

    }
}
