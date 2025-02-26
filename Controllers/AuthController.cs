using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using moneygram_api.DTOs;
using moneygram_api.Services.Interfaces;
using System.Threading.Tasks;

namespace moneygram_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;
        public AuthController(IAuthService authService, ITokenService tokenService)
        {
            _authService = authService;
            _tokenService = tokenService;
        }
        

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequest)
        {
            if (loginRequest == null)
            {
                return BadRequest("Invalid client request");
            }

            try
            {
                var token = await _authService.AuthenticateAsync(loginRequest);
                return Ok(new { Token = token });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { ex.Message });
            }
        }

        [HttpGet("validate-token")]
        [Authorize]
        public async Task<IActionResult> ValidateToken()
        {
            var authHeader = Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return BadRequest("No valid token provided");
            }

            var token = authHeader.Substring("Bearer ".Length);
            return await _tokenService.ValidateTokenAsync(token);
        }
    }
}