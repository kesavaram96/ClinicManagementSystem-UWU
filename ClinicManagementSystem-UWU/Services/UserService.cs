using ClinicManagementSystem_UWU.Interfaces;
using ClinicManagementSystem_UWU.Models.Auth;
using ClinicManagementSystem_UWU.Models.Data;
using ClinicManagementSystem_UWU.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementSystem_UWU.Services
{
    public class UserService : IUserService
    {
        private readonly ClinicDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        public UserService(ClinicDbContext context, 
            IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
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
            user.PasswordHash=_passwordHasher.HashPassword(user,userDto.PasswordHash);
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

        public async Task<List<Role>> GetUserRolesAsync(int userId)
        {
            return await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.Role).ToListAsync();
                
        }
    }

}
