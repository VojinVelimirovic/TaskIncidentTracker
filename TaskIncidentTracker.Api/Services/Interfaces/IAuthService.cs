using TaskIncidentTracker.Api.DTOs.Auth;
using TaskIncidentTracker.Api.Models;

namespace TaskIncidentTracker.Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterUser(string username, string password);
        Task<(string, UserResponse?)> LoginUser(string username, string password);
        Task<bool> ChangeUserRole(string adminId, string username, UserRole role);
        Task<List<UserResponse>> GetAllUsers();
    }
}
