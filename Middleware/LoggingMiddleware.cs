using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using moneygram_api.Services.Interfaces;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using moneygram_api.Models;
using System.Linq;

namespace moneygram_api.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private const string WebhookPath = "/sandbox/cbz-bank/moneygram/webhook_status_events";

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _next = next;
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Invoke(HttpContext context)
        {
            var isWebhookRequest = context.Request.Path.Equals(WebhookPath, StringComparison.OrdinalIgnoreCase);
            var requestLogId = isWebhookRequest ? await LogWebhookRequest(context) : await LogRequest(context);

            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            try
            {
                await _next(context);
                if (isWebhookRequest)
                {
                    await LogWebhookResponse(context, requestLogId);
                }
                else
                {
                    await LogResponse(context, requestLogId);
                }
            }
            catch (Exception ex)
            {
                await LogException(context, ex);
                throw;
            }
            finally
            {
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        private async Task<string> ReadRequestBody(HttpContext context)
        {
            context.Request.EnableBuffering();
            if (context.Request.ContentLength > 0 && context.Request.Body.CanSeek)
            {
                using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true);
                var body = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;
                return body;
            }
            return string.Empty;
        }

        private string SerializeHeaders(IHeaderDictionary headers)
        {
            var headerStrings = headers.Select(h => $"{h.Key}: {string.Join(", ", h.Value)}");
            return string.Join("; ", headerStrings);
        }

        private async Task<int> LogRequest(HttpContext context)
        {
            var requestBody = await ReadRequestBody(context);
            var username = context.Items["Username"]?.ToString() ?? "Anonymous";
            var originIp = GetClientIp(context);
            var request = context.Request;

            var requestLog = new RequestLog
            {
                Username = username,
                HttpMethod = request.Method,
                Url = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}",
                Headers = SerializeHeaders(request.Headers), // Properly serialize headers
                RequestBody = requestBody,
                Device = request.Headers["User-Agent"],
                Origin = originIp,
                RequestTime = DateTime.Now
            };

            _logger.LogInformation("Incoming Request: Username={Username}, Method={HttpMethod}, Url={Url}, Origin={Origin}",
                username, request.Method, requestLog.Url, originIp);

            using var scope = _serviceScopeFactory.CreateScope();
            var loggingService = scope.ServiceProvider.GetRequiredService<ILoggingService>();
            await loggingService.LogRequestAsync(requestLog);
            return requestLog.Id;
        }

        private async Task<int> LogWebhookRequest(HttpContext context)
        {
            var requestBody = await ReadRequestBody(context);
            var originIp = GetClientIp(context);
            var request = context.Request;

            // Extract the Signature header
            var signatureHeader = request.Headers["Signature"].ToString();
            if (string.IsNullOrEmpty(signatureHeader))
            {
                _logger.LogWarning("Signature header is missing for webhook request");
            }

            var webhookLog = new WebhookLog
            {
                HttpMethod = request.Method,
                Url = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}",
                Headers = SerializeHeaders(request.Headers), // Properly serialize headers
                RequestBody = requestBody,
                Device = request.Headers["User-Agent"],
                Origin = originIp,
                RequestTime = DateTime.Now
            };

            _logger.LogInformation("Incoming Webhook Request: Method={HttpMethod}, Url={Url}, Origin={Origin}, SignatureHeader={SignatureHeader}",
                request.Method, webhookLog.Url, originIp, signatureHeader);

            using var scope = _serviceScopeFactory.CreateScope();
            var loggingService = scope.ServiceProvider.GetRequiredService<ILoggingService>();
            await loggingService.LogWebhookRequestAsync(webhookLog);
            return webhookLog.Id;
        }

        private async Task LogResponse(HttpContext context, int requestLogId)
        {
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            var username = context.Items["Username"]?.ToString() ?? "Anonymous";
            var originIp = GetClientIp(context);

            _logger.LogInformation("Outgoing Response: Username={Username}, Method={HttpMethod}, Url={Url}, StatusCode={StatusCode}, Origin={Origin}",
                username, context.Request.Method, $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}",
                context.Response.StatusCode, originIp);

            using var scope = _serviceScopeFactory.CreateScope();
            var loggingService = scope.ServiceProvider.GetRequiredService<ILoggingService>();
            var requestLog = await loggingService.GetRequestLogByIdAsync(requestLogId);
            if (requestLog != null)
            {
                requestLog.ResponseBody = responseBody ?? string.Empty;
                requestLog.StatusCode = context.Response.StatusCode;
                requestLog.ResponseTime = DateTime.Now;
                await loggingService.UpdateRequestLogAsync(requestLog);
            }
        }

        private async Task LogWebhookResponse(HttpContext context, int webhookLogId)
        {
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            var originIp = GetClientIp(context);

            _logger.LogInformation("Outgoing Webhook Response: Method={HttpMethod}, Url={Url}, StatusCode={StatusCode}, Origin={Origin}",
                context.Request.Method, $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}",
                context.Response.StatusCode, originIp);

            using var scope = _serviceScopeFactory.CreateScope();
            var loggingService = scope.ServiceProvider.GetRequiredService<ILoggingService>();
            var webhookLog = await loggingService.GetWebhookLogByIdAsync(webhookLogId);
            if (webhookLog != null)
            {
                webhookLog.ResponseBody = responseBody ?? string.Empty;
                webhookLog.StatusCode = context.Response.StatusCode;
                webhookLog.ResponseTime = DateTime.Now;
                await loggingService.UpdateWebhookLogAsync(webhookLog);
            }
        }

        private async Task LogException(HttpContext context, Exception ex)
        {
            var username = context.Items["Username"]?.ToString() ?? "Anonymous";
            var originIp = GetClientIp(context);

            var exceptionLog = new ExceptionLog
            {
                Username = username,
                ExceptionMessage = ex.Message,
                InnerExceptionMessage = ex.InnerException?.Message,
                StackTrace = ex.StackTrace,
                HttpMethod = context.Request.Method,
                Url = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}",
                Origin = originIp,
                Timestamp = DateTime.Now
            };

            _logger.LogError(ex, "Exception occurred: Username={Username}, Method={HttpMethod}, Url={Url}, Origin={Origin}",
                username, context.Request.Method, exceptionLog.Url, originIp);

            using var scope = _serviceScopeFactory.CreateScope();
            var loggingService = scope.ServiceProvider.GetRequiredService<ILoggingService>();
            await loggingService.LogExceptionAsync(exceptionLog);
        }

        private string GetClientIp(HttpContext context)
        {
            return context.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                ?? context.Request.Headers["X-Real-IP"].FirstOrDefault()
                ?? context.Connection.RemoteIpAddress?.ToString()
                ?? "unknown";
        }
    }
}