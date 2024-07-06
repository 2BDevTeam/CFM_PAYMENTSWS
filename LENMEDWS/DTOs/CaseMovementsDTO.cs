namespace LENMEDWS.DTOs
{
    public class CaseMovementsDTO
    {
        public string HospitalCode { get; set; }
        public int CaseNumber { get; set; }
        public int MovementCategory { get; set; }
        public string MovementType { get; set; }
        public DateTime StartDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public DateTime EndDate { get; set; }

    }
}
