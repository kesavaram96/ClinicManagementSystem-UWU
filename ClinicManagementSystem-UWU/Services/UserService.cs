using ClinicManagementSystem_UWU.Interfaces;
using ClinicManagementSystem_UWU.Models.Auth;
using ClinicManagementSystem_UWU.Models.Data;
using ClinicManagementSystem_UWU.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ClinicManagementSystem_UWU.Services
{
    public class UserService : IUserService
    {
        private readonly ClinicDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _configuration;

        public UserService(ClinicDbContext context,
            IPasswordHasher<User> passwordHasher,
            IConfiguration configuration)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
        }


        public async Task<User> CreateUserAsync(UserCreationDto userDto)
        {
            var user = new User
            {
                Username = userDto.Username,
                //PasswordHash = userDto.PasswordHash,
                FullName = userDto.FullName,
                Email = userDto.Email,
                PhoneNumber = userDto.PhoneNumber,
                Address = userDto.Address,
                IsActive = userDto.IsActive,
                CreatedDate = userDto.CreatedDate
            };
            user.PasswordHash = _passwordHasher.HashPassword(user, userDto.PasswordHash);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }
        public async Task AssignRoleAsync(int userId, int roleId)
        {
            var userRole = new UserRole { UserId = userId, RoleId = roleId };
            _context.UserRoles.Add(userRole);
            await _context.SaveChangesAsync();
        }
        [Authorize]
        public async Task<List<Role>> GetUserRolesAsync(int userId)
        {
            return await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.Role).ToListAsync();

        }
        public async Task<string> LoginAsync(LoginDto loginDto)
        {

            var user = await _context.Users
        .Include(u => u.UserRoles)
        .ThenInclude(ur => ur.Role)
        .SingleOrDefaultAsync(u => u.Username == loginDto.Username);

            if (user == null || _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password) != PasswordVerificationResult.Success)
            {
                return null;
            }

            var roles = user.UserRoles.Select(ur => ur.Role.RoleName).ToList();



            // Generate JWT Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
    }
                .Concat(roles.Select(role => new Claim(ClaimTypes.Role, role)))),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:ExpireMinutes"])),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

        }

        public async Task<bool> CreateUserAsync(CreateUserDto createUserDto)
        {
            // Check if role exists
            var role = await _context.Roles.SingleOrDefaultAsync(r => r.RoleName == createUserDto.RoleName);
            if (role == null) throw new Exception("Role does not exist.");

            var existingUser = await _context.Users.SingleOrDefaultAsync(u => u.Username == createUserDto.Username);
            if (existingUser != null)
            {
                throw new Exception("Username already exists. Please choose a different username.");
            }

            var user = new User
            {
                Username = createUserDto.Username,
                Email = createUserDto.Email,
                IsActive = true,
                PasswordHash = _passwordHasher.HashPassword(null, createUserDto.Password),
                FullName= createUserDto.FullName,
                PhoneNumber=createUserDto.PhoneNumber,
                Address=createUserDto.Address
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var userRole = new UserRole
            {
                UserId = user.UserId,
                RoleId = role.RoleId
            };
            _context.UserRoles.Add(userRole);

            await _context.SaveChangesAsync();


            switch (createUserDto.RoleName.ToLower())
            {
                case "admin":
                    var adminDetails = new AdminDetails
                    {
                        UserId = user.UserId,
                        Department = createUserDto.PersonalDetails.Department,
                        Position = createUserDto.PersonalDetails.Position
                    };
                    _context.AdminDetails.Add(adminDetails);
                    await _context.SaveChangesAsync();
                    break;

                case "doctor":
                    var doctorDetails = new DoctorDetails
                    {
                        UserId = user.UserId,
                        Specialization = createUserDto.PersonalDetails.Specialization,
                        JoiningDate = createUserDto.PersonalDetails.JoiningDate ?? DateTime.Now
                    };
                    _context.DoctorDetails.Add(doctorDetails);
                    await _context.SaveChangesAsync();
                    break;

                case "nurse":
                    var nurseDetails = new NurseDetails
                    {
                        UserId = user.UserId,
                        Shift = createUserDto.PersonalDetails.Shift,
                        Ward = createUserDto.PersonalDetails.Ward
                    };
                    _context.NurseDetails.Add(nurseDetails);
                    await _context.SaveChangesAsync();
                    break;

                case "receptionist":
                    var receptionistDetails = new ReceptionistDetails
                    {
                        UserId = user.UserId,
                        DeskLocation = createUserDto.PersonalDetails.DeskLocation,
                        AssignedDoctor = createUserDto.PersonalDetails.AssignedDoctor,
                        Shift = createUserDto.PersonalDetails.Shift
                    };
                    _context.ReceptionistDetails.Add(receptionistDetails);
                    await _context.SaveChangesAsync();
                    break;

                case "patient":
                    var patientDetails = new PatientDetails
                    {
                        UserId = user.UserId,
                        MedicalHistory = createUserDto.PersonalDetails.MedicalHistory,
                        InsuranceNumber = createUserDto.PersonalDetails.InsuranceNumber,
                        DateOfBirth = createUserDto.PersonalDetails.DateOfBirth ?? DateTime.MinValue,
                        EmergencyContactPerson = createUserDto.PersonalDetails.EmergencyContactPerson,
                        ECNumber = createUserDto.PersonalDetails.ECNumber,
                        ECRelationship = createUserDto.PersonalDetails.ECRelationship,
                        BloodGroup = createUserDto.PersonalDetails.BloodGroup
                    };
                    _context.PatientDetails.Add(patientDetails);
                    await _context.SaveChangesAsync();
                    break;
                default:
                    throw new Exception("Role does not exist.");
            }

            return true;


        }

        public async Task<List<User>> GetUsersList()
        {
            try
            {
                return await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role) // Only include Role, not User inside UserRoles
                .ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine(ex.Message);
                throw;
            }

        }


        public async Task<UserEditDTO> GetUser(int userId)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                    .Where(u => u.UserId == userId) // Corrected comparison
                    .Select(u => new UserEditDTO
                    {
                        userId=u.UserId,
                        Username = u.Username,
                        Password = "",
                        Email = u.Email,
                        RoleName = u.UserRoles.Select(ur => ur.Role.RoleName).FirstOrDefault(),
                        PhoneNumber = u.PhoneNumber,
                        FullName = u.FullName,
                        Address = u.Address,
                        PersonalDetails = null
                    })
                    .FirstOrDefaultAsync();

                return user ?? throw new KeyNotFoundException("User not found.");
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the user.", ex);
            }
        }

        public async Task<string> EditUser(int userId, EditUserDTO userDto)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.UserRoles)
                    .FirstOrDefaultAsync(u => u.UserId == userId);

                if (user == null)
                {
                    return "User not found.";
                }

                // Update user details
                user.Username = userDto.Username;
                user.Email = userDto.Email;
                user.PhoneNumber = userDto.PhoneNumber;
                user.FullName = userDto.FullName;
                user.Address = userDto.Address;
                user.PasswordHash = _passwordHasher.HashPassword(user, userDto.Password);

                // If roles need to be updated, handle them as well
                var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == userDto.RoleName);
                if (role != null)
                {
                    var userRole = await _context.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == userId);
                    if (userRole != null)
                    {
                        userRole.RoleId = role.RoleId;
                    }
                    else
                    {
                        _context.UserRoles.Add(new UserRole
                        {
                            UserId = userId,
                            RoleId = role.RoleId
                        });
                    }
                }

                // Save changes
                await _context.SaveChangesAsync();

                return "User updated successfully.";
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the user.", ex);
            }
        }

        // Method to Delete User
        public async Task<string> DeleteUser(int userId)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.UserRoles)
                    .FirstOrDefaultAsync(u => u.UserId == userId);

                if (user == null)
                {
                    return "User not found.";
                }

                // Remove associated user roles if necessary
                var userRoles = await _context.UserRoles.Where(ur => ur.UserId == userId).ToListAsync();
                _context.UserRoles.RemoveRange(userRoles);

                // Remove the user
                _context.Users.Remove(user);

                // Save changes to the database
                await _context.SaveChangesAsync();

                return "User deleted successfully.";
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the user.", ex);
            }
        }
    }


}
