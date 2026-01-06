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

        public AuthService(ApplicationDbContext appDbContext, IPasswordHasher passwordHasher, IJwtTokenService jwtTokenService)
        {
            _context = appDbContext;
            _passwordHasher = passwordHasher;
            _jwtTokenService = jwtTokenService;
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
