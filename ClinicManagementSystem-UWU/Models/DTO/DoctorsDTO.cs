using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSystem_UWU.Models.DTO
{
    public class DoctorsDTO
    {
        public string? FullName { get; set; }
        
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Specialization { get; set; }
    }
}
