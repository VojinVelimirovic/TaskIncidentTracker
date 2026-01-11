using Microsoft.EntityFrameworkCore;
using TaskIncidentTracker.Api.Data;
using TaskIncidentTracker.Api.DTOs.Auth;
using TaskIncidentTracker.Api.Models;
using TaskIncidentTracker.Api.Services.Interfaces;

namespace TaskIncidentTracker.Api.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly ILogger<AuthService> _logger;

        public AuthService(ApplicationDbContext appDbContext, IPasswordHasher passwordHasher, IJwtTokenService jwtTokenService, ILogger<AuthService> logger)
        {
            _context = appDbContext;
            _passwordHasher = passwordHasher;
            _jwtTokenService = jwtTokenService;
            _logger = logger;
        }

        public async Task<bool> ChangeUserRole(string adminId, string username, UserRole role)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                return false;
            }
            user.Role = role;
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Admin {adminId} has changed user {username}'s role to {role.ToString()}");
            return true;
        }

        public async Task<List<UserResponse>> GetAllUsers()
        {
            return await _context.Users
            .Select(u => new UserResponse
            {
                Id = u.Id,
                Username = u.Username,
                Role = u.Role
            })
            .ToListAsync();
        }

        public async Task<(string, UserResponse?)> LoginUser(string username, string password)
        {
            var normalized = username.Trim().ToLower();
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == normalized);
            if (user == null || !_passwordHasher.VerifyPassword(user.PasswordHash, password))
            {
                return (String.Empty, null);
            }
            var userResponse = new UserResponse { Id = user.Id , Username = user.Username, Role = user.Role};
            var token = _jwtTokenService.GenerateJwtToken(user);
            _logger.LogInformation($"User {user.Id} - {user.Username} has logged in.");
            return (token, userResponse);
        }

        public async Task<bool> RegisterUser(string username, string password, UserRole role)
        {
            var normalized = username.Trim().ToLower();
            if (await _context.Users.AnyAsync(u => u.Username == normalized))
            {
                return false;
            }

            var user = new User { Username = username.Trim().ToLower(), PasswordHash = _passwordHasher.HashPassword(password), Role = role, CreatedAt = DateTime.UtcNow };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
