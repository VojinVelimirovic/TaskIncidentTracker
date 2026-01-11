using System.ComponentModel.DataAnnotations;
using TaskIncidentTracker.Api.Models;

namespace TaskIncidentTracker.Api.DTOs.Auth
{
    public class RegisterUserRequest
    {
        [Required, MinLength(3)]
        public string Username {  get; set; }
        [Required, MinLength(5)]
        public string Password { get; set; }
        [Required]
        public UserRole Role { get; set; }
    }
}
