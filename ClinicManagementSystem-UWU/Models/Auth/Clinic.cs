﻿using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSystem_UWU.Models.Auth
{
    public class Clinic
    {
        [Key]
        public int ClinicId { get; set; }
        public string ClinicName { get; set; }
        public string Location { get; set; }
        public int PatientCapability { get; set; }
        
    }
}
