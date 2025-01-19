using ClinicManagementSystem_UWU.Interfaces;
using ClinicManagementSystem_UWU.Models.Auth;
using ClinicManagementSystem_UWU.Models.Data;
using ClinicManagementSystem_UWU.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementSystem_UWU.Services
{
    public class ClinicService : IClinicService
    {
        private readonly ClinicDbContext _context;

        public ClinicService(ClinicDbContext context)
        {
            _context = context;
        }

        // 📌 Create a new clinic
        public async Task<string> CreateClinicAsync(ClinicDTO clinicDto)
        {
            var clinic = new Clinic
            {
                ClinicName = clinicDto.ClinicName,
                Location = clinicDto.Location,
                PatientCapability = clinicDto.PatientCapability
            };

            _context.Clinics.Add(clinic);
            await _context.SaveChangesAsync();
            return "Clinic created successfully!";
        }

        // 📌 Update an existing clinic
        public async Task<string> UpdateClinicAsync(ClinicDTO clinicDto)
        {
            var clinic = await _context.Clinics.FindAsync(clinicDto.ClinicId);

            if (clinic == null)
                return "Clinic not found!";

            clinic.ClinicName = clinicDto.ClinicName;
            clinic.Location = clinicDto.Location;
            clinic.PatientCapability = clinicDto.PatientCapability;

            await _context.SaveChangesAsync();
            return "Clinic updated successfully!";
        }

        // 📌 Delete a clinic
        public async Task<string> DeleteClinicAsync(int clinicId)
        {
            var clinic = await _context.Clinics.FindAsync(clinicId);

            if (clinic == null)
                return "Clinic not found!";

            _context.Clinics.Remove(clinic);
            await _context.SaveChangesAsync();
            return "Clinic deleted successfully!";
        }

        // 📌 Get all clinics
        public async Task<List<ClinicDTO>> GetAllClinicsAsync()
        {
            return await _context.Clinics
                .Select(c => new ClinicDTO
                {
                    ClinicId = c.ClinicId,
                    ClinicName = c.ClinicName,
                    Location = c.Location,
                    PatientCapability = c.PatientCapability
                }).ToListAsync();
        }

        // 📌 Get a single clinic by ID
        public async Task<ClinicDTO> GetClinicByIdAsync(int clinicId)
        {
            var clinic = await _context.Clinics.FindAsync(clinicId);

            if (clinic == null)
                return null;

            return new ClinicDTO
            {
                ClinicId = clinic.ClinicId,
                ClinicName = clinic.ClinicName,
                Location = clinic.Location,
                PatientCapability = clinic.PatientCapability
            };
        }
    }

}
