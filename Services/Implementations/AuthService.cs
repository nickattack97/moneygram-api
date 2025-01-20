using moneygram_api.DTOs;
using moneygram_api.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace moneygram_api.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> AuthenticateAsync(LoginRequestDTO loginRequest)
        {
            // Validate the user credentials (this is just a placeholder, replace with actual validation logic)
            if (loginRequest.Username != "testuser" || loginRequest.Password != "password")
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            // Read the token from the text file
            var tokenFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Media", "bearer_token.txt");
            if (!File.Exists(tokenFilePath))
            {
                throw new FileNotFoundException("Bearer token file not found");
            }

            var tokenJson = await File.ReadAllTextAsync(tokenFilePath);
            var tokenObject = JsonSerializer.Deserialize<TokenObject>(tokenJson);

            if (tokenObject == null || string.IsNullOrEmpty(tokenObject.Token))
            {
                throw new InvalidOperationException("Invalid token in the file");
            }

            return tokenObject.Token;
        }

        private class TokenObject
        {
            public string Token { get; set; }
        }
    }
}