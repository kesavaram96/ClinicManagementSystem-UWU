﻿namespace ClinicManagementSystem_UWU.Models.DTO
{
    public class CreateAppointmentRequestDto
    {
        public int clinicId{ get; set; }
        public DateTime AppointmentDate { get; set; }
                
        public string RequestingReason { get; set; }
        public int DoctorId { get; set; }

    }
}
