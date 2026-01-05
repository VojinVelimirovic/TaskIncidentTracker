using Microsoft.EntityFrameworkCore;
using TaskIncidentTracker.Api.Data;
using TaskIncidentTracker.Api.Models;
using TaskIncidentTracker.Api.Services.Interfaces;

namespace TaskIncidentTracker.Api.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher _passwordHasher;

        public AuthService(ApplicationDbContext appDbContext, IPasswordHasher passwordHasher)
        {
            _context = appDbContext;
            _passwordHasher = passwordHasher;
        }
        public async Task<User?> LoginUser(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username.ToLower());
            if (user == null || !_passwordHasher.VerifyPassword(user.PasswordHash, password))
            {
                return null;
            }
            return user;
        }

        public async Task<bool> RegisterUser(string username, string password, UserRole role)
        {
            if (await _context.Users.AnyAsync(u => u.Username == username))
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
