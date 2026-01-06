using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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
            var (token, userResponse) = await _authService.LoginUser(request.Username, request.Password);
            if (userResponse != null)
            {
                return Ok( new { token, userResponse });
            }
            return Unauthorized(new { message = "Incorrect username or password." });
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var name = User.FindFirstValue(ClaimTypes.Name);

            var role = User.FindFirstValue(ClaimTypes.Role);

            return Ok( new { id, name, role });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            return Ok(new { message = "You are admin" });
        }
    }
}
