using System;
using System.Collections.Generic;

namespace CFM_PAYMENTSWS.Domains.Models
{
    public partial class JobLocks
    {
        public string JobId { get; set; } = null!;
        public bool IsRunning { get; set; }
    }
}
