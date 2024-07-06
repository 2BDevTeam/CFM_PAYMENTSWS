namespace LENMEDWS.DTOs
{
    public class ActiveBedCountDTO
    {
        public DateTime Date { get; set; }
        public string HospitalCode { get; set; }
        public string WardCode { get; set; }
        public string WardName { get; set; }
        public int ActiveBeds { get; set; }

    }
}
