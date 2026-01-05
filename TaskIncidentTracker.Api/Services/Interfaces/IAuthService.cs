using TaskIncidentTracker.Api.Models;

namespace TaskIncidentTracker.Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<bool> RegisterUser(string username, string password, UserRole role);
        Task<User?> LoginUser(string username, string password);
    }
}
