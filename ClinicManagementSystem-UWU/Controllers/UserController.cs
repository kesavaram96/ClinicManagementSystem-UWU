using ClinicManagementSystem_UWU.Interfaces;
using ClinicManagementSystem_UWU.Models.Auth;
using ClinicManagementSystem_UWU.Models.Data;
using ClinicManagementSystem_UWU.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementSystem_UWU.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClinicDbContext _context;


        public UserController(IUserService userService, IHttpContextAccessor httpContextAccessor,ClinicDbContext context)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser(UserCreationDto userDto)
        {
            var user = await _userService.CreateUserAsync(userDto);
            return Ok(new { Message = "User created successfully", UserId = user.UserId });
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole(int userId, int roleId)
        {
            await _userService.AssignRoleAsync(userId, roleId);
            return Ok("Role assigned successfully");
        }
        [Authorize]
        [HttpGet("{userId}/roles")]
        public async Task<IActionResult> GetUserRoles(int userId)
        {
            var roles = await _userService.GetUserRolesAsync(userId);
            return Ok(roles);
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var token = await _userService.LoginAsync(loginDto);
            if (token == null)
            {
                return Unauthorized(new { Message = "Invalid credentials", Status = false });
            }

            return Ok(new { Message = "Success", Token = token, Status = true });
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto createUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _userService.CreateUserAsync(createUserDto);
                if (result)
                {
                    return Ok(new { Message = "User created successfully." });
                }
                return BadRequest(new { Message = "Failed to create user." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("Users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetUsersList();


            var userDtos = users.Select(u => new UserListDTO
            {
                UserId = u.UserId,
                Username = u.Username,
                Email = u.Email,
                RoleNames = u.UserRoles.Select(ur => ur.Role.RoleName).ToList() // Include role names only
            }).ToList();

            return Ok(userDtos);
        }


        [HttpGet("User")]
        public async Task<IActionResult> GetUser(int userId)
        {
           var user= await _userService.GetUser(userId);

            return Ok(user);
        }

        [HttpPut("EditUser")]
        public async Task<IActionResult> EditUser(int userId, [FromBody] EditUserDTO userDto)
        {
            try
            {
                var result = await _userService.EditUser(userId, userDto);
                if (result == "User not found.")
                {
                    return NotFound(new { message = result });
                }
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the user.", error = ex.Message });
            }
        }

        // Delete User API
        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            try
            {
                var result = await _userService.DeleteUser(userId);
                if (result == "User not found.")
                {
                    return NotFound(new { message = result });
                }
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the user.", error = ex.Message });
            }
        }
        [HttpGet("{patientId}")]
        public async Task<ActionResult<PatientDetailsDTO>> GetPatient(int patientId)
        {
            var patientDetails = await _userService.GetPatient(patientId);

            if (patientDetails == null)
            {
                return NotFound(); // Return 404 if patient is not found
            }

            return Ok(patientDetails); // Return 200 with patient details
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] PasswordDTO request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Password) || string.IsNullOrWhiteSpace(request.ConfirmPassword))
            {
                return BadRequest("Password fields cannot be empty.");
            }

            if (request.Password != request.ConfirmPassword)
            {
                return BadRequest("Passwords do not match.");
            }

            var result = await _userService.ChangePassword(request);

            if (!result)
            {
                return BadRequest("Failed to change password. Please try again.");
            }

            return Ok(new { Message = "Password changed successfully!" });
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdatePatientDetails([FromBody] PatientPersonalInfo patientDetails)
        {
            try
            {
                // Get the logged-in username (userId)
                var username = _httpContextAccessor.HttpContext?.User.Identity.Name;

                if (string.IsNullOrEmpty(username))
                {
                    return Unauthorized("User not authenticated.");
                }

                // You can get the userId by querying the User table with the username.
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

                if (user == null)
                {
                    return NotFound("User not found.");
                }

                // Use the patient service to update the patient details.
                var updatedPatient = await _userService.UpdatePatientDetails(user.UserId, patientDetails);

                return Ok(updatedPatient);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error updating patient details: {ex.Message}");
            }
        }

        [HttpGet("Doctors")]
        public async Task<IActionResult> GetDoctors()
        {
            try
            {
                // Fetching the list of doctors using the service method
                List<DoctorsDTO> doctorList = await _userService.GetDoctor();

                // Returning a successful response with the list of doctors
                if (doctorList == null || doctorList.Count == 0)
                {
                    return NotFound(new { message = "No doctors found." });
                }

                return Ok(doctorList);
            }
            catch (Exception ex)
            {
                // Handling any exceptions that occur during the process
                return StatusCode(500, new { message = "An error occurred while fetching the doctors.", error = ex.Message });
            }
        }
    }
}
