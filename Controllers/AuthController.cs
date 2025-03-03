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
        private readonly IUserService _userService;

        public AuthController(IAuthService authService, ITokenService tokenService, IUserService userService)
        {
            _authService = authService;
            _tokenService = tokenService;
            _userService = userService;
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

        [HttpPut("change-forgotten-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ChangeForgottenPassword([FromBody] ChangeForgottenPasswordRequestDTO request)
        {
            try
            {
                var result = await _authService.ChangeForgottenPasswordAsync(request);
                return Ok(new { Success = result, Message = "Password changed successfully." });
            }
            catch (BaseCustomException ex)
            {
                return StatusCode(ex.ErrorCode, new ErrorResponseDTO
                {
                    ErrorCode = ex.ErrorCode.ToString(),
                    Message = ex.ErrorMessage,
                    OffendingField = ex.OffendingField,
                    TimeStamp = ex.TimeStamp.ToString("o")
                });
            }
        }

        [HttpPost("send-forgot-password-otp/{username}")]
        [AllowAnonymous]
        public async Task<IActionResult> SendForgotPasswordOtp(string username)
        {
            try
            {
                var result = await _authService.SendForgotPasswordOtpAsync(username);
                return Ok(result);
            }
            catch (BaseCustomException ex)
            {
                return StatusCode(ex.ErrorCode, new ErrorResponseDTO
                {
                    ErrorCode = ex.ErrorCode.ToString(),
                    Message = ex.ErrorMessage,
                    OffendingField = ex.OffendingField,
                    TimeStamp = ex.TimeStamp.ToString("o")
                });
            }
        }

        [HttpPost("resend-forgot-password-otp/{username}")]
        [AllowAnonymous]
        public async Task<IActionResult> ResendForgotPasswordOtp(string username)
        {
            try
            {
                var result = await _authService.ResendForgotPasswordOtpAsync(username);
                return Ok(result);
            }
            catch (BaseCustomException ex)
            {
                return StatusCode(ex.ErrorCode, new ErrorResponseDTO
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

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetUserProfile()
        {
            try
            {
                var userIdClaim = User.FindFirst("id")?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                    return BadRequest("Invalid user ID in token.");

                if (!long.TryParse(userIdClaim, out var userId))
                    return BadRequest("User ID is not in a valid format.");

                var user = await _userService.GetUserProfileAsync(userId);
                if (user == null)
                    return NotFound("User not found.");

                return Ok(user);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error fetching user profile.");
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