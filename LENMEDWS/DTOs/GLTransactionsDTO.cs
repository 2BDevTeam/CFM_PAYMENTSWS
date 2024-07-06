namespace LENMEDWS.DTOs
{
    public class GLTransactionsDTO
    {
        public int LedgerId{ get; set; }
        public string HospitalCode { get; set; }
        public int CaseNumber { get; set; }
        public String PostingDate { get; set; }
        public string GLAccount { get; set; }
        public string GLAccountDesc { get; set; }
        public string CostCenter { get; set; }
        public string CostCenterDesc { get; set; }
        public string DCIndicator { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
    }
}
