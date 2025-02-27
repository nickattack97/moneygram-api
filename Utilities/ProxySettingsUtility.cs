using RestSharp;
using System;
using System.Net;
using moneygram_api.Settings;

namespace moneygram_api.Utilities
{
    public static class ProxySettingsUtility
    {
        public static void ApplyProxySettings(RestClientOptions options, IConfigurations configurations)
        {
            if (options == null || configurations == null)
            {
                throw new ArgumentNullException("Options or configurations cannot be null.");
            }

            if (configurations.UseProxy)
            {
                if (string.IsNullOrEmpty(configurations.ProxyAddress) || configurations.ProxyPort <= 0)
                {
                    throw new InvalidOperationException("Proxy address and port must be provided when UseProxy is enabled.");
                }

                // Create proxy instance
                var proxy = new WebProxy
                {
                    Address = new Uri($"{configurations.ProxyAddress}:{configurations.ProxyPort}"),
                    BypassProxyOnLocal = false, // Adjust based on your needs
                };

                // Add credentials if provided
                if (!string.IsNullOrEmpty(configurations.ProxyUsername) && !string.IsNullOrEmpty(configurations.ProxyPassword))
                {
                    proxy.Credentials = new NetworkCredential(
                        configurations.ProxyUsername,
                        configurations.ProxyPassword
                    );
                }

                // Assign proxy to RestClientOptions
                options.Proxy = proxy;
            }
        }
    }
}