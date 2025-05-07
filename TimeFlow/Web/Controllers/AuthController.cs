using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Services.Abstractions.DTO;
using System.IdentityModel.Tokens.Jwt;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;
        private readonly IUserService _userService;

        public AuthController(IAuthService auth, IUserService userService)
        {
            _auth = auth;
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var user = await _auth.RegisterAsync(dto);
            return Ok(user);
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var token = await _auth.LoginAsync(dto);
            return Ok(token);
        }
        
        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var sub = User.FindFirstValue(ClaimTypes.NameIdentifier)
                   ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            if (string.IsNullOrEmpty(sub))
                return Unauthorized();

            if (!Guid.TryParse(sub, out var userId))
                return Unauthorized();

            var userDto = await _userService.GetByIdAsync(userId);
            return Ok(userDto);
        }
    }
}
