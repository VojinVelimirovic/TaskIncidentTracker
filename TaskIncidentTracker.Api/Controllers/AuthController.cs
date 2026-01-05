using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskIncidentTracker.Api.DTOs.Auth;
using TaskIncidentTracker.Api.Services.Interfaces;

namespace TaskIncidentTracker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserRequest request)
        {
            var result = await _authService.RegisterUser(request.Username, request.Password, request.Role);
            if (result)
            {
                return Ok(new { message = "Registration successful." });   
            }
            return Conflict( new { message = "Username taken." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserRequest request)
        {
            var user = await _authService.LoginUser(request.Username, request.Password);
            if (user != null)
            {
                return Ok( new UserResponse { Id = user.Id, Username = user.Username, Role = user.Role });
            }
            return Unauthorized(new { message = "Incorrect username or password." });
        }
    }
}
