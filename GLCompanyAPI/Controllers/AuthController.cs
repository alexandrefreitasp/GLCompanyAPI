using GLCompanyAPI.Configurations;
using GLCompanyAPI.Models;
using GLCompanyAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GLCompanyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly JwtSettings _jwtSettings;
        private readonly IConfiguration _configuration;

        public AuthController(IAuthService authService, IOptions<JwtSettings> jwtSettings, IConfiguration configuration)
        {
            _authService = authService;
            _jwtSettings = jwtSettings.Value;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            var success = await _authService.RegisterUserAsync(user.Username, user.PasswordHash);
            if (!success) return BadRequest("Username already exists");

            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            var authenticatedUser = await _authService.AuthenticateAsync(user.Username, user.PasswordHash);
            if (authenticatedUser == null) return Unauthorized("Invalid credentials");

            var token = GenerateJwtToken(authenticatedUser.Username);
            return Ok(new { Token = token });
        }

        [HttpPost("auto-login")]
        public IActionResult AutoLogin([FromBody] string appSecret)
        {
            var storedSecret = _configuration["ApplicationSettings:AppSecret"];

            if (appSecret != storedSecret)
            {
                return Unauthorized("Invalid application secret.");
            }

            var token = GenerateJwtToken("System");
            return Ok(new { Token = token });
        }

        private string GenerateJwtToken(string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
