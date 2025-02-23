using System;
using Microsoft.Extensions.Configuration;

namespace moneygram_api.Settings
{
    public interface IConfigurations
    {
        string BaseUrl { get; }
        string Resource { get; }
        string AgentId { get; }
        string Token { get; }
        int Sequence { get; }
        int ApiVersion { get; }
        int ClientSoftwareVer { get; }
        string JwtSecretKey { get; }
        string JwtIssuer { get; }
        string JwtAudience { get; }
        string PublicKey { get; }
        string[] WhitelistedIps { get; }
        string[] WhitelistedProxies { get; }
        bool EnableLocalDevelopment { get; }
        string[] LocalDevelopmentIps { get; }
    }

    public class Configurations : IConfigurations
    {
        private readonly IConfiguration _configuration;

        public Configurations(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public string BaseUrl => _configuration.GetSection("AppSettings")["BaseUrl"] ?? string.Empty;
        public string Resource => _configuration.GetSection("AppSettings")["Resource"] ?? string.Empty;
        public string AgentId => _configuration.GetSection("AppSettings")["AgentId"] ?? string.Empty;
        public string Token => _configuration.GetSection("AppSettings")["Token"] ?? string.Empty;
        public int Sequence => int.TryParse(_configuration.GetSection("AppSettings")["Sequence"], out var result) ? result : 0;
        public int ApiVersion => int.TryParse(_configuration.GetSection("AppSettings")["ApiVersion"], out var result) ? result : 0;
        public int ClientSoftwareVer => int.TryParse(_configuration.GetSection("AppSettings")["ClientSoftwareVer"], out var result) ? result : 0;
        public string JwtSecretKey => _configuration.GetSection("JwtSettings")["SecretKey"] ?? string.Empty;
        public string JwtIssuer => _configuration.GetSection("JwtSettings")["Issuer"] ?? string.Empty;
        public string JwtAudience => _configuration.GetSection("JwtSettings")["Audience"] ?? string.Empty;
        public string PublicKey => _configuration.GetSection("WebhookSettings")["PublicKey"] ?? string.Empty;
        public string[] WhitelistedIps => _configuration.GetSection("WebhookSettings")["WhitelistedIps"]?.Split(',') ?? Array.Empty<string>();
        public string[] WhitelistedProxies => _configuration.GetSection("WebhookSettings")["WhitelistedProxies"]?.Split(',') ?? Array.Empty<string>();
        public bool EnableLocalDevelopment => bool.TryParse(_configuration.GetSection("WebhookSettings")["EnableLocalDevelopment"], out var result) && result;
        public string[] LocalDevelopmentIps => _configuration.GetSection("WebhookSettings")["LocalDevelopmentIps"]?.Split(',') ?? Array.Empty<string>();
   
    }
}