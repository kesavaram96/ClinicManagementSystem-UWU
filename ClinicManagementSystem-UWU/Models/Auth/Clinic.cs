namespace ClinicManagementSystem_UWU.Models.Auth
{
    public class Clinic
    {
        public int ClinicId { get; set; }
        public string ClinicName { get; set; }
        public string Location { get; set; }
        public ICollection<Room> Rooms { get; set; }
    }
}
