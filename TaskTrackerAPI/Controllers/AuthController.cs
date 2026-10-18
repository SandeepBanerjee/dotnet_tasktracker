using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskTrackerAPI.Models.Dtos;
using TaskTrackerAPI.Services;

namespace TaskTrackerAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController: ControllerBase 
    {
        private readonly AuthService _authService;
        private readonly ResponseDto _response;

        public AuthController(AuthService authService)
        {
            _authService = authService;
            _response = new ResponseDto();
        }

        [HttpPost("register")]
        public async Task <IActionResult> Register([FromBody] RegistrationRequestDto request)
        {
            var registerResponse = await _authService.Register(request);

            if (registerResponse == null)
            {
                _response.IsSuccess = false;
                return BadRequest(_response);
            }

            return Ok(registerResponse);
        }

        [HttpPost("login")]
        public async Task <IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var loginResponse = await _authService.Login(request);

            if (loginResponse == null)
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = "Username or password is incorrect";
                return BadRequest(_response);
            }

            _response.Result = loginResponse;
            return Ok(_response);
        }

        [HttpPost("assign-role")]
        public async Task <IActionResult> AssignRole([FromBody] AssignRoleRequestDto request)
        {
            var assignRoleToUser = await _authService.AssignRole(request.Email, request.Role.ToLower());
            if (assignRoleToUser == null)
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = "Error encountered";
                return BadRequest(_response);
            }

            _response.IsSuccess = true;
            _response.Result = assignRoleToUser;

            return Ok(_response);
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult LoggedInUserDetails()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var username = User.Identity?.Name;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            _response.IsSuccess = true;
            _response.Result = new
            {
                UserId = userId,
                Email = email,
                Role = role,
                Username = username
            };

            return Ok(_response);
        }
    }
}
