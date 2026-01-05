using TaskIncidentTracker.Api.Models;

namespace TaskIncidentTracker.Api.DTOs.Auth
{
    public class UserResponse
    {
        public int Id {  get; set; }
        public string Username { get; set; }
        public UserRole Role {  get; set; }
    }
}
