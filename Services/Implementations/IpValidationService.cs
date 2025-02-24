using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using System.Net;
using moneygram_api.Settings;
using moneygram_api.Services.Interfaces;

namespace moneygram_api.Services.Implementations
{
    public class IpValidationService : IIpValidationService
    {
        private readonly IConfigurations _configurations;
        private readonly ILogger<IpValidationService> _logger;

        public IpValidationService(
            IConfigurations configurations, 
            ILogger<IpValidationService> logger)
        {
            _configurations = configurations;
            _logger = logger;
        }

        public bool IsIpWhitelisted(HttpContext context)
        {
            var clientIp = GetClientIp(context);

            if (string.IsNullOrEmpty(clientIp))
            {
                _logger.LogWarning("Unable to determine client IP address");
                return false;
            }

            _logger.LogInformation($"Validating IP: {clientIp}");

            if (_configurations.WhitelistedIps.Any(ip => IsIpMatch(clientIp, ip)))
            {
                _logger.LogInformation($"IP {clientIp} is whitelisted");
                return true;
            }

            _logger.LogWarning($"Access denied - IP: {clientIp}");
            return false;
        }

        private string GetClientIp(HttpContext context)
        {
            return context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        }

        private bool IsIpMatch(string ipAddress, string whitelist)
        {
            return ipAddress.Equals(whitelist, StringComparison.OrdinalIgnoreCase);
        }
    }
}