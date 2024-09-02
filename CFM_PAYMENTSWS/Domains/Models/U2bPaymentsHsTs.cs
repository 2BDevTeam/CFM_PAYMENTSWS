using System;
using System.Collections.Generic;

namespace CFM_PAYMENTSWS.Domains.Models;

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


    public U2bPaymentsHsTs(string transactionId, string creditAccount, string beneficiaryName, string transactionDescription, string currency, decimal amount, string bankReference, string statusCode, string statusDescription, string batchId, DateTime processingDate, string statusCodeHs, string statusDescriptionHs,
    string u2BPaymentsHsStamp,
    string debitAccount,
    DateTime ousrData)
    {
        TransactionId = transactionId;
        CreditAccount = creditAccount;
        BeneficiaryName = beneficiaryName;
        TransactionDescription = transactionDescription;
        Currency = currency;
        Amount = amount;
        BankReference = bankReference;
        StatusCode = statusCode;
        StatusDescription = statusDescription;
        BatchId = batchId;
        ProcessingDate = processingDate;
        StatusCodeHs = statusCodeHs;
        StatusDescriptionHs = statusDescriptionHs;
        U2bPaymentsHsTsstamp = u2BPaymentsHsStamp;
        DebitAccount = debitAccount;
        Ousrdata = ousrData;
    }
}

