﻿namespace ClinicManagementSystem_UWU.Models.DTO
{
    public class CreateUserDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
        public PersonalInformationInputDto PersonalDetails { get; set; }
    }
}