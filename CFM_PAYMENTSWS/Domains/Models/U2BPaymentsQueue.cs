using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace CFM_PAYMENTSWS.Domains.Models
{
    public class U2BPaymentsQueue
    {
		public string u_2b_paymentsQueuestamp { get; set; }
		public decimal valor { get; set; }
		public string moeda { get; set; }
        public string BatchId { get; set; }
		public string description { get; set; }
        public string beneficiaryName { get; set; }
        public string transactionDescription { get; set; }
        public int lordem { get; set; }
		public string transactionId { get; set; }
		public string origem { get; set; }
		public string destino { get; set; }
		public int canal { get; set; }
		public string estado { get; set; }
		public DateTime usrdata { get; set; }
		public string descricao { get; set; }

		public string keystamp;
		public DateTime processingDate { get; set; }
		public string BeneficiaryEmail { get; set; }
		public override string ToString() => JsonConvert.SerializeObject(this);

	}
}
