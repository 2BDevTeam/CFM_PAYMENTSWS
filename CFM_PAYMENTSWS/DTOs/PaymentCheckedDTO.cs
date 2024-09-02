using Newtonsoft.Json;
using CFM_PAYMENTSWS.Providers.Nedbank.DTOs;
using System.ComponentModel.DataAnnotations;

namespace CFM_PAYMENTSWS.DTOs
{
    public class PaymentCheckedDTO
    {
        [Required(ErrorMessage = "BatchId is required.")]
        public string BatchId { get; set; }

        [Required(ErrorMessage = "ProcessingDate is required.")]
        public DateTime ProcessingDate { get; set; }

        [Required(ErrorMessage = "StatusCode is required.")]
        [RegularExpression("^[0-9]{4}$", ErrorMessage = "StatusCode must be a 4-digit number.")]
        [Range(0, 9999, ErrorMessage = "StatusCode must be between 0000 and 9999.")]
        public string StatusCode { get; set; }

        [Required(ErrorMessage = "StatusDescription is required.")]
        public string StatusDescription { get; set; }

        [Required(ErrorMessage = "PaymentCheckedRecords is required.")]
        public List<PaymentCheckedRecordsDTO> PaymentCheckedRecords { get; set; }

        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
