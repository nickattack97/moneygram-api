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

        public AuthController(IAuthService authService)
        {
            _authService = authService;
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
                return Unauthorized(ex.Message);
            }
        }
    }
}