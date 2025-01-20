using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using moneygram_api.Settings;

namespace moneygram_api.Middleware
{
    public class JwtAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfigurations _configurations;

        public JwtAuthenticationMiddleware(RequestDelegate next, IConfigurations configurations)
        {
            _next = next;
            _configurations = configurations;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Allow requests to Swagger and Auth endpoints without authentication
            if (context.Request.Path.StartsWithSegments("/swagger") || context.Request.Path.StartsWithSegments("/api/auth"))
            {
                await _next(context);
                return;
            }

            // Extract token from Authorization header
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

            if (authHeader != null && authHeader.StartsWith("Bearer "))
            {
                var token = authHeader.Substring("Bearer ".Length);

                try
                {
                    var claimsPrincipal = ValidateToken(token);
                    if (claimsPrincipal != null)
                    {
                        context.User = claimsPrincipal; // Set the user context

                        // Extract username from the token and store it in HttpContext.Items
                        var username = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "userName")?.Value;
                        if (!string.IsNullOrEmpty(username))
                        {
                            context.Items["Username"] = username;
                        }
                    }
                    else
                    {
                        context.Response.StatusCode = 401; // Unauthorized
                        return;
                    }
                }
                catch
                {
                    context.Response.StatusCode = 401; // Unauthorized
                    return;
                }
            }
            else
            {
                context.Response.StatusCode = 401; // Unauthorized
                return;
            }

            // Continue processing the request
            await _next(context);
        }

        private ClaimsPrincipal ValidateToken(string jwtToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configurations.JwtSecretKey);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _configurations.JwtIssuer,
                ValidAudience = _configurations.JwtAudience
            };

            try
            {
                return tokenHandler.ValidateToken(jwtToken, validationParameters, out var validatedToken);
            }
            catch
            {
                return null; // Token validation failed
            }
        }
    }
}