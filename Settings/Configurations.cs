using System;
using Microsoft.Extensions.Configuration;

namespace moneygram_api.Settings
{
    public interface IConfigurations
    {
        string BaseUrl { get; }
        string Resource { get; }
        string AuthBaseUrl { get; }
        string AuthResource { get; }
        string ForgotPasswordResource { get; }
        string ResendForgotPasswordOtpResource { get; }
        string ChangeForgottenPasswordResource { get; }
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
        int UpdateFrequencyInDays { get; }
        bool UseProxy { get; }          
        string ProxyAddress { get; }    
        int ProxyPort { get; }           
        string ProxyUsername { get; }    
        string ProxyPassword { get; } 
    }

    public class Configurations : IConfigurations
    {
        private readonly IConfiguration _configuration;

        public Configurations(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public string BaseUrl => _configuration.GetSection("AppSettings")["BaseUrl"] ?? string.Empty;
        public string AuthBaseUrl => _configuration.GetSection("UserConnectSettings")["BaseUrl"] ?? string.Empty;
        public string AuthResource => _configuration.GetSection("UserConnectSettings")["LoginResource"] ?? string.Empty;
        public string ForgotPasswordResource => _configuration.GetSection("UserConnectSettings")["ForgotPasswordResource"] ?? string.Empty;
        public string ResendForgotPasswordOtpResource => _configuration.GetSection("UserConnectSettings")["ResendForgotPasswordOtpResource"] ?? string.Empty;
        public string ChangeForgottenPasswordResource => _configuration.GetSection("UserConnectSettings")["ChangeForgottenPasswordResource"] ?? string.Empty;
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
        public string[] WhitelistedIps => _configuration.GetSection("WebhookSettings:WhitelistedIps").Get<string[]>() ?? Array.Empty<string>();
        public int UpdateFrequencyInDays => int.TryParse(_configuration.GetSection("AppSettings")["UpdateFrequencyInDays"], out var result) ? result : 14; // Default is 14 days
        public bool UseProxy => bool.TryParse(_configuration.GetSection("ProxySettings")["UseProxy"], out var result) && result;
        public string ProxyAddress => _configuration.GetSection("ProxySettings")["ProxyAddress"] ?? string.Empty;
        public string ProxyUsername => _configuration.GetSection("ProxySettings")["ProxyUsername"] ?? string.Empty;
        public string ProxyPassword => _configuration.GetSection("ProxySettings")["ProxyPassword"] ?? string.Empty;
        public int ProxyPort => int.TryParse(_configuration.GetSection("ProxySettings")["ProxyPort"], out var result) ? result : 0;
    }
}