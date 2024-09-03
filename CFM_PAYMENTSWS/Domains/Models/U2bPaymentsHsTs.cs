using System;
using System.Collections.Generic;

namespace CFM_PAYMENTSWS.Domains.Models
{
    public partial class U2bPaymentsHsTs
    {
        public string TransactionId { get; set; } = null!;
        public string CreditAccount { get; set; } = null!;
        public string BeneficiaryName { get; set; } = null!;
        public string TransactionDescription { get; set; } = null!;
        public string Currency { get; set; } = null!;
        public decimal Amount { get; set; }
        public string BankReference { get; set; } = null!;
        public string StatusCode { get; set; } = null!;
        public string StatusDescription { get; set; } = null!;
        public string BatchId { get; set; } = null!;
        public DateTime ProcessingDate { get; set; }
        public string StatusCodeHs { get; set; } = null!;
        public string StatusDescriptionHs { get; set; } = null!;
        public string U2bPaymentsHsTsstamp { get; set; } = null!;
        public string DebitAccount { get; set; } = null!;
        public DateTime Ousrdata { get; set; }
        public string Oristamp { get; set; } = null!;
    }
}
