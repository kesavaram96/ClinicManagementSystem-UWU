using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSystem_UWU.Models.DTO
{
    public class PasswordDTO
    {
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
