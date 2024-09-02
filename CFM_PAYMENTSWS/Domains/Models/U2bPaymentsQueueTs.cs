using System;
using System.Collections.Generic;

namespace CFM_PAYMENTSWS.Domains.Models;

public partial class U2bPaymentsQueueTs
{
    public string U2bPaymentsQueueTsstamp { get; set; } = null!;

    public string? origem { get; set; }

    public string destino { get; set; } = null!;

    public decimal valor { get; set; }

    public string? moeda { get; set; }

    public int canal { get; set; }

    public int lordem { get; set; }

    public string Usrinis { get; set; } = null!;

    public DateTime usrdata { get; set; }

    public string Usrhora { get; set; } = null!;

    public DateTime? Ousrdata { get; set; }

    public string? Ousrinis { get; set; }

    public string? Ousrhora { get; set; }

    public bool Marcada { get; set; }

    public string? transactionId { get; set; }

    public string? keystamp { get; set; }

    public string BatchId { get; set; } = null!;

    public string description { get; set; } = null!;

    public string transactionDescription { get; set; } = null!;

    public string? Oristamp { get; set; }

    public string? docno { get; set; }

    public string descricao { get; set; } = null!;

    public string estado { get; set; } = null!;

    public DateTime processingDate { get; set; }

    public string? beneficiaryName { get; set; }

    public string BeneficiaryEmail { get; set; } = null!;
}
