using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using moneygram_api.Settings;

namespace moneygram_api.HealthChecks
{
    public class MoneyGramHealthCheck : IHealthCheck
    {
        private readonly IConfigurations _configurations;
        private readonly HttpClient _httpClient;

        public MoneyGramHealthCheck(IConfigurations configurations, HttpClient httpClient)
        {
            _configurations = configurations;
            _httpClient = httpClient;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var baseUrl = _configurations.BaseUrl;
            var resource = _configurations.Resource;
            var url = $"{baseUrl}{resource}";

            try
            {
                var response = await _httpClient.GetAsync(url, cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    return HealthCheckResult.Healthy("MoneyGram endpoint is reachable.");
                }
                return HealthCheckResult.Unhealthy("MoneyGram endpoint is unavailable.");
            }
            catch (HttpRequestException ex)
            {
                return HealthCheckResult.Unhealthy("MoneyGram endpoint is unavailable.", ex);
            }
        }
    }
}