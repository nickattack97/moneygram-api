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
        private readonly IWebHostEnvironment _environment;

        public IpValidationService(
            IConfigurations configurations, 
            ILogger<IpValidationService> logger,
            IWebHostEnvironment environment)
        {
            _configurations = configurations;
            _logger = logger;
            _environment = environment;
        }

        public bool IsIpWhitelisted(HttpContext context)
        {
            var (clientIp, proxyIp) = ExtractIpAddresses(context);

            if (string.IsNullOrEmpty(clientIp))
            {
                var remoteIp = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                _logger.LogWarning($"Unable to determine client IP address. Remote IP: {remoteIp}");
                return false;
            }

            _logger.LogInformation($"Validating - Client IP: {clientIp}, Proxy IP: {proxyIp}, Environment: {_environment.EnvironmentName}");

            // Handle Development Environment
            if (_environment.IsDevelopment() && _configurations.EnableLocalDevelopment)
            {
                if (IsLocalDevelopmentIp(clientIp))
                {
                    _logger.LogInformation($"Allowing development IP: {clientIp}");
                    return true;
                }
                _logger.LogWarning($"IP {clientIp} is not in local development whitelist");
            }

            // Check Direct Access (no proxy)
            if (_configurations.WhitelistedIps.Any(ip => IsIpMatch(clientIp, ip)))
            {
                _logger.LogInformation($"Direct IP {clientIp} is whitelisted");
                return true;
            }

            // Check Proxy Access
            if (!string.IsNullOrEmpty(proxyIp))
            {
                if (_configurations.WhitelistedProxies.Any(ip => IsIpMatch(proxyIp, ip)))
                {
                    _logger.LogInformation($"Request through whitelisted proxy: {proxyIp} for client: {clientIp}");
                    return true;
                }
                _logger.LogWarning($"Proxy IP {proxyIp} is not whitelisted for client: {clientIp}");
            }

            _logger.LogWarning($"Access denied - Client IP: {clientIp}, Proxy IP: {proxyIp}");
            return false;
        }

        private (string clientIp, string proxyIp) ExtractIpAddresses(HttpContext context)
        {
            string clientIp = null;
            string proxyIp = context.Connection.RemoteIpAddress?.ToString();

            // Try to get client IP from headers
            var forwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(forwardedFor))
            {
                var ips = forwardedFor.Split(',', StringSplitOptions.RemoveEmptyEntries);
                clientIp = ips.FirstOrDefault()?.Trim();
                // If there are multiple IPs, the last one is usually the proxy
                if (ips.Length > 1)
                {
                    proxyIp = ips.Last().Trim();
                }
            }

            // Fallback to X-Real-IP if X-Forwarded-For is not present
            if (string.IsNullOrEmpty(clientIp))
            {
                clientIp = context.Request.Headers["X-Real-IP"].FirstOrDefault();
            }

            // If no headers are present, use the remote IP as client IP
            if (string.IsNullOrEmpty(clientIp))
            {
                clientIp = proxyIp;
                proxyIp = null;
            }

            return (clientIp, proxyIp);
        }

        private bool IsLocalDevelopmentIp(string ipAddress)
        {
            var localIps = new[]
            {
                "127.0.0.1",
                "::1",
                "localhost"
            }.Concat(_configurations.LocalDevelopmentIps ?? Array.Empty<string>());

            return localIps.Any(ip => IsIpMatch(ipAddress, ip));
        }

        private bool IsIpMatch(string ipAddress, string whitelist)
        {
            // Handle CIDR notation
            if (whitelist.Contains("/"))
            {
                return IsCidrMatch(ipAddress, whitelist);
            }

            // Handle exact IP match
            return ipAddress.Equals(whitelist, StringComparison.OrdinalIgnoreCase);
        }

        private bool IsCidrMatch(string ipAddress, string cidrNotation)
        {
            try
            {
                var network = IPNetwork.Parse(cidrNotation);
                var address = IPAddress.Parse(ipAddress);
                return network.Contains(address);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking CIDR match for {ipAddress} against {cidrNotation}");
                return false;
            }
        }
    }
}