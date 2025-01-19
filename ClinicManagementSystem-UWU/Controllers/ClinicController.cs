using ClinicManagementSystem_UWU.Interfaces;
using ClinicManagementSystem_UWU.Models.DTO;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ClinicController : ControllerBase
{
    private readonly IClinicService _clinicService;

    public ClinicController(IClinicService clinicService)
    {
        _clinicService = clinicService;
    }

    // 📌 Create Clinic
    [HttpPost("create")]
    public async Task<IActionResult> CreateClinic([FromBody] ClinicDTO clinicDto)
    {
        var result = await _clinicService.CreateClinicAsync(clinicDto);
        return Ok(new { Message = result });
    }

    // 📌 Update Clinic
    [HttpPut("update")]
    public async Task<IActionResult> UpdateClinic([FromBody] ClinicDTO clinicDto)
    {
        var result = await _clinicService.UpdateClinicAsync(clinicDto);
        return Ok(new { Message = result });
    }

    // 📌 Delete Clinic
    [HttpDelete("delete/{clinicId}")]
    public async Task<IActionResult> DeleteClinic(int clinicId)
    {
        var result = await _clinicService.DeleteClinicAsync(clinicId);
        return Ok(new { Message = result });
    }

    // 📌 Get All Clinics
    [HttpGet("getAll")]
    public async Task<IActionResult> GetAllClinics()
    {
        var clinics = await _clinicService.GetAllClinicsAsync();
        return Ok(clinics);
    }

    // 📌 Get Clinic by ID
    [HttpGet("get/{clinicId}")]
    public async Task<IActionResult> GetClinicById(int clinicId)
    {
        var clinic = await _clinicService.GetClinicByIdAsync(clinicId);
        if (clinic == null)
            return NotFound(new { Message = "Clinic not found!" });

        return Ok(clinic);
    }
}
