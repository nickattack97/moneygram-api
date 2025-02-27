using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using moneygram_api.DTOs;
using moneygram_api.Services.Interfaces;
using System.Threading.Tasks;
using moneygram_api.Exceptions;
using moneygram_api.Utilities;
using moneygram_api.DTOs;

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
            try
            {
                var token = await _authService.AuthenticateAsync(loginRequest);
                return Ok(new AuthResponseDTO { Token = token });
            }
            catch (BaseCustomException ex)
            {
                return StatusCode(ex.ErrorCode switch
                {
                    400 => 400, // Bad Request
                    401 => 401, // Unauthorized
                    503 => 503, // Service Unavailable
                    _ => 500    // Internal Server Error
                }, new ErrorResponseDTO
                {
                    ErrorCode = ex.ErrorCode.ToString(),
                    Message = ex.ErrorMessage,
                    OffendingField = ex.OffendingField,
                    TimeStamp = ex.TimeStamp.ToString("o")
                });
            }
        }

        [HttpGet("validate-token")]
        [Authorize]
        public async Task<IActionResult> ValidateToken()
        {
            var authHeader = Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return BadRequest(new ErrorResponseDTO 
                { 
                    ErrorCode = "INVALID_TOKEN", 
                    Message = "No valid Bearer token provided." 
                });
            }

            try
            {
                var token = authHeader.Substring("Bearer ".Length).Trim();
                var result = await _tokenService.ValidateTokenAsync(token);
                return result; 
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponseDTO 
                { 
                    ErrorCode = "INTERNAL_SERVER_ERROR", 
                    Message = $"Token validation failed: {ex.Message}" 
                });
            }
        }


        public class AuthResponseDTO
        {
            public string Token { get; set; } = string.Empty;
        }

        public class ErrorResponseDTO
        {
            public string ErrorCode { get; set; } = string.Empty;
            public string Message { get; set; } = string.Empty;
            public string OffendingField { get; set; } = string.Empty;
            public string TimeStamp { get; set; } = string.Empty;
        }       
    }
}