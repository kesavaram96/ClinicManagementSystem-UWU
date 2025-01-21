namespace ClinicManagementSystem_UWU.Models.DTO
{
    public class AppointmentRequestDto
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int clinicId { get; set; }
        public string CLinicName { get; set; }
        public string PatientName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public DateTime RequestedDate { get; set; }
        public string ApproveUser { get; set; }
        public string Status { get; set; }
        public string RequestingReason { get; set; }
        public string ApprovedReason { get; set; }
        public int? DoctorId { get; set; }
        public string DoctorName { get; set; }
    }
}
