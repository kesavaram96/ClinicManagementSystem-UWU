namespace ClinicManagementSystem_UWU.Models.DTO
{
    public class CreateAppointmentRequestDto
    {
        public int CliniId { get; set; }
        public DateTime AppointmentDate { get; set; }
                
        public string RequestingReason { get; set; }
        
    }
}
