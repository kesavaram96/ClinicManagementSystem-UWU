using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicManagementSystem_UWU.Models.Auth
{
    public class Room
    {
        [Key]
        public int RoomId { get; set; }

        [ForeignKey("Clinic")]
        public int ClinicId { get; set; }
        public Clinic Clinic { get; set; }

        public string RoomNumber { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}
