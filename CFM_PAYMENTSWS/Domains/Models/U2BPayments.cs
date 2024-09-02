using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace CFM_PAYMENTSWS.Domains.Models
{
    public class U2BPayments
    {
	
			public string u_2b_paymentsstamp { get; set; }
			public decimal valor { get; set; }
			public string destino { get; set; }
		    public string transactionId { get; set; }
			public string BatchId { get; set; }	
			public int lordem { get; set; }
		    public string moeda { get; set; }
		    public string origem { get; set; }
		    public string tabela { get; set; }
		    public string oristamp { get; set; }
			public bool Processado { get; set; }
            public string estado { get; set; }
			public string descricao { get; set; }
			public DateTime dataprocessado { get; set; }
			public string horaprocessado { get; set; }
			public DateTime ousrdata { get; set; }
			public string ousrinis { get; set; }
			public string ousrhora { get; set; }
			public DateTime usrdata { get; set; }
			public string usrinis { get; set; }
			public string usrhora { get; set; }
			public int canal { get; set; }
			public string bankReference { get; set; }
			public string BeneficiaryEmail { get; set; }

		    public override string ToString() => JsonConvert.SerializeObject(this);


	}
}
