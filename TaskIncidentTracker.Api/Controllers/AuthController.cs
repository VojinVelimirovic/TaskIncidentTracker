using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Linq.Dynamic.Core.Tokenizer;
using System.Security.Claims;
using TaskIncidentTracker.Api.DTOs.Auth;
using TaskIncidentTracker.Api.Models;
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
            var token = await _authService.RegisterUser(request.Username, request.Password);
            if (!token.IsNullOrEmpty())
            {
                return Ok(new {accessToken = token, message = "Registration successful." });   
            }
            return Conflict( new { message = "Username taken." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserRequest request)
        {
            var (token, userResponse) = await _authService.LoginUser(request.Username, request.Password);
            if (userResponse != null)
            {
                return Ok( new { accessToken = token, message = "Login successful.", data = userResponse });
            }
            return Unauthorized(new { message = "Incorrect username or password." });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("change-role")]
        public async Task<IActionResult> ChangeRole(string username, UserRole role)
        {
            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _authService.ChangeUserRole(adminId, username, role);
            if(result)
                return Ok(new { message = $"{username}'s role successfully changed to {role.ToString()}." });
            return BadRequest(new { message = "Role change failed."});
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers() 
        {
            var users = await _authService.GetAllUsers();
            return Ok(new { message = "User list fetched.", data = users});
        }
    }
}
