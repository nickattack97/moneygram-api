using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using moneygram_api.Services.Interfaces;
using moneygram_api.Settings;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;

namespace moneygram_api.Services.Implementations
{
    public class TokenService : ITokenService
    {
        private readonly IConfigurations _configurations;

        public TokenService(IConfigurations configurations)
        {
            _configurations = configurations;
        }

        public async Task<IActionResult> ValidateTokenAsync(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return new BadRequestObjectResult("No valid token provided");
            }

            try
            {
                var jwtSecretKey = _configurations.JwtSecretKey;
                var jwtIssuer = _configurations.JwtIssuer;
                var jwtAudience = _configurations.JwtAudience;

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(jwtSecretKey);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtAudience,
                    ClockSkew = TimeSpan.Zero
                };

                try
                {
                    var claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                    var jwtToken = validatedToken as JwtSecurityToken;

                    if (jwtToken == null)
                    {
                        return new UnauthorizedObjectResult("Invalid token format");
                    }

                    var expirationDate = jwtToken.ValidTo;
                    var currentDate = DateTime.UtcNow;

                    return new OkObjectResult(new
                    {
                        IsValid = true,
                        ExpiresAt = expirationDate,
                        TimeRemainingSeconds = (expirationDate - currentDate).TotalSeconds,
                        IsExpired = currentDate > expirationDate
                    });
                }
                catch (SecurityTokenExpiredException)
                {
                    return new OkObjectResult(new
                    {
                        IsValid = false,
                        IsExpired = true,
                        Message = "Token has expired"
                    });
                }
                catch (SecurityTokenException ex)
                {
                    return new BadRequestObjectResult(new
                    {
                        IsValid = false,
                        IsExpired = false,
                        Message = $"Token validation failed: {ex.Message}"
                    });
                }
            }
            catch (Exception ex)
            {
                return new ObjectResult(new
                {
                    IsValid = false,
                    Message = $"An error occurred while validating the token: {ex.Message}"
                })
                {
                    StatusCode = 500
                };
            }
        }
    }
}