using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSystem_UWU.Models.DTO
{
    public class AppointmentDTO
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }

        [Required]
        public int ClinicId { get; set; }
    }

}
