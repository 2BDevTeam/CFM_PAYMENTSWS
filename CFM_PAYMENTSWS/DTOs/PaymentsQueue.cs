using Newtonsoft.Json;
using CFM_PAYMENTSWS.Domains.Models;

namespace CFM_PAYMENTSWS.DTOs
{
    public class PaymentsQueue
    {
      public  int canal { get; set; }
       public Payment payment { get; set; }
        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
