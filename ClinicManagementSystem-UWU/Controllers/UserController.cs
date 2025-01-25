using ClinicManagementSystem_UWU.Interfaces;
using ClinicManagementSystem_UWU.Models.Auth;
using ClinicManagementSystem_UWU.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem_UWU.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        
        public UserController(IUserService userService)
        {
            _userService = userService;
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
    }
}