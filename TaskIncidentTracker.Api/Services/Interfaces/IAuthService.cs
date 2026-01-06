using TaskIncidentTracker.Api.DTOs.Auth;
using TaskIncidentTracker.Api.Models;

namespace TaskIncidentTracker.Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<bool> RegisterUser(string username, string password, UserRole role);
        Task<(string, UserResponse?)> LoginUser(string username, string password);
    }
}
