using ClinicManagementSystem_UWU.Interfaces;
using ClinicManagementSystem_UWU.Models.DTO;
using ClinicManagementSystem_UWU.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementSystem_UWU.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentRequestController : ControllerBase
    {
        private readonly IAppointmentRequestService _service;
        
        public AppointmentRequestController(IAppointmentRequestService service)
        {
            _service = service;
         
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] string? status)
        {
            var result = await _service.GetAllAsync(status);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAppointmentRequestDto request)
        {
            var result = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpPut("UpdateAppointmentRequestStatus/{requestId}")]
        public async Task<IActionResult> UpdateAppointmentRequestStatus(int requestId, [FromBody] UpdateAppointmentRequestDTO requestDto)
        {
            if (requestDto == null || string.IsNullOrEmpty(requestDto.Status))
            {
                return BadRequest(new { Message = "Invalid request data." });
            }

            var response = await _service.UpdateAppointmentRequestStatusAsync(requestId, requestDto.Status, requestDto.ApprovedReason);

            if (response.Message.Contains("not found") || response.Message.Contains("failed"))
            {
                return NotFound(response);
            }

            return Ok(response);
        }

    }
}
