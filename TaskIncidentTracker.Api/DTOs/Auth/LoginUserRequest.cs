using System.ComponentModel.DataAnnotations;

namespace TaskIncidentTracker.Api.DTOs.Auth
{
    public class LoginUserRequest
    {
        [Required, MinLength(3)]
        public string Username {  get; set; }
        [Required, MinLength(6)]
        public string Password { get; set; }
    }
}
