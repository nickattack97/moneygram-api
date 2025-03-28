using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using moneygram_api.Models;
using moneygram_api.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace moneygram_api.Middleware
{
    public class MoneyGramXmlLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<MoneyGramXmlLoggingMiddleware> _logger;

        public MoneyGramXmlLoggingMiddleware(RequestDelegate next, ILogger<MoneyGramXmlLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, IServiceScopeFactory serviceScopeFactory, SoapContext soapContext)
        {
            // Skip OPTIONS and non-MoneyGram requests
            if (context.Request.Method == HttpMethods.Options || !IsMoneyGramRequest(context))
            {
                await _next(context);
                return;
            }

            string operation = GetOperationFromRequest(context);
            
            try
            {
                // Process request
                await _next(context);

                // Log using the SOAP context values if available
                await LogXmlInteractionIfAvailable(
                    context, 
                    serviceScopeFactory,
                    operation,
                    soapContext.RequestXml,
                    soapContext.ResponseXml
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in MoneyGram XML logging");
                
                // Log partial information if request XML is available
                if (soapContext.RequestXml != null)
                {
                    await LogXmlInteractionIfAvailable(
                        context,
                        serviceScopeFactory,
                        operation,
                        soapContext.RequestXml,
                        $"<ErrorResponse>{ex.Message}</ErrorResponse>"
                    );
                }
                throw;
            }
        }

        private async Task LogXmlInteractionIfAvailable(
            HttpContext context,
            IServiceScopeFactory serviceScopeFactory,
            string operation,
            string requestXml,
            string responseXml)
        {
            // Only log if both RequestXml and ResponseXml are not null or empty
            if (!string.IsNullOrWhiteSpace(requestXml) && !string.IsNullOrWhiteSpace(responseXml))
            {
                using var scope = serviceScopeFactory.CreateScope();
                var loggingService = scope.ServiceProvider.GetRequiredService<ILoggingService>();
                var username = context.Items["Username"]?.ToString() ?? "Anonymous";

                var xmlLog = new MoneyGramXmlLog
                {
                    Operation = operation,
                    RequestXml = requestXml,
                    ResponseXml = responseXml,
                    LogTime = DateTime.UtcNow,
                    Username = username,
                    HttpMethod = context.Request.Method,
                    Url = $"{context.Request.Path}{context.Request.QueryString}"
                };

                await loggingService.LogMoneyGramXmlAsync(xmlLog);
            }
        }

        private bool IsMoneyGramRequest(HttpContext context)
        {  
            if (context.Request.Method == HttpMethods.Options)
                return false;

            var path = context.Request.Path.Value?.ToLower();
            return path != null && GetOperationFromRequest(context) != "Unknown";
        }

        private string GetOperationFromRequest(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower();
            if (path == null) return "Unknown";

            return path switch
            {
                string p when p.EndsWith("/api/sends/consumer-lookup") => "ConsumerLookup",
                string p when p.EndsWith("/api/sends/send-validation") => "SendValidation",
                string p when p.EndsWith("/api/sends/save-rewards") => "SaveRewards",
                string p when p.EndsWith("/api/sends/detail-lookup") => "DetailLookup",
                string p when p.EndsWith("/api/sends/commit-transaction") => "CommitTransaction",
                string p when p.EndsWith("/api/sends/amend-transaction") => "AmendTransaction",
                string p when p.EndsWith("/api/sends/send-reversal") => "SendReversal",
                string p when p.EndsWith("/api/sends/filtered-fee-lookup") => "FeeLookup",
                string p when p.EndsWith("/api/sends/gffp") => "GFFP",
                string p when p.EndsWith("/api/sends/code-table") => "CodeTable",
                string p when p.EndsWith("/api/sends/country-info") => "CountryInfo",
                string p when p.EndsWith("/api/sends/currency-info") => "CurrencyInfo",
                _ => "Unknown"
            };
        }
    }

    public static class MoneyGramXmlLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseMoneyGramXmlLogging(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MoneyGramXmlLoggingMiddleware>();
        }
    }
}