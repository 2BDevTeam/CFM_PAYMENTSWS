namespace LENMEDWS.DTOs
{
    public class CaseMasterDataDTO
    {
        public string HospitalCode { get; set; }
        public int CaseNumber { get; set; }
        public int PatientNumber { get; set; }
        public string MedicalAidAdmin { get; set; }
        public string MedicalAid { get; set; }
        public string AdmittingDoctor { get; set; }
        public string DoctorSpeciality { get; set; }
        public DateTime AdmissionDate { get; set; }
        public DateTime DischargeDate { get; set; }
        public DateTime DeceasedDate { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Nationality { get; set; }
        public string CaseType { get; set; }
        public string ChargeType { get; set; }
        public bool EDAdmission { get; set; }

    }
}
