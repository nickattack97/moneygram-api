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

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _next = next;
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Invoke(HttpContext context)
        {
            var requestLogId = await LogRequest(context);

            var originalBodyStream = context.Response.Body;
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                try
                {
                    await _next(context);
                    await LogResponse(context, requestLogId);
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
        }

        private async Task<int> LogRequest(HttpContext context)
        {
            context.Request.EnableBuffering();
            var request = context.Request;

            var requestBody = string.Empty;

            if (request.ContentLength > 0 && request.Body.CanSeek)
            {
                using (var reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true))
                {
                    requestBody = await reader.ReadToEndAsync();
                    request.Body.Position = 0;
                }
            }

            var username = context.Items["Username"]?.ToString() ?? "Anonymous";
            var originIp = GetClientIp(context);

            var logDetails = new
            {
                Username = username,
                HttpMethod = request.Method,
                Url = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}",
                Headers = request.Headers,
                Body = requestBody,
                Device = request.Headers["User-Agent"],
                Origin = originIp,
                RequestTime = DateTime.Now
            };

            _logger.LogInformation("Incoming Request: {@LogDetails}", logDetails);

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var loggingService = scope.ServiceProvider.GetRequiredService<ILoggingService>();

                var requestLog = new RequestLog
                {
                    Username = username,
                    HttpMethod = request.Method,
                    Url = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}",
                    Headers = request.Headers.ToString(),
                    RequestBody = requestBody,
                    Device = request.Headers["User-Agent"],
                    Origin = originIp,
                    RequestTime = DateTime.Now
                };

                await loggingService.LogRequestAsync(requestLog);
                return requestLog.Id;
            }
        }

        private async Task LogResponse(HttpContext context, int requestLogId)
        {
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            var username = context.Items["Username"]?.ToString() ?? "Anonymous";
            var originIp = GetClientIp(context);

            var logDetails = new
            {
                Username = username,
                HttpMethod = context.Request.Method,
                Url = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}",
                StatusCode = context.Response.StatusCode,
                ResponseBody = responseBody,
                Device = context.Request.Headers["User-Agent"],
                Origin = originIp,
                ResponseTime = DateTime.Now
            };

            _logger.LogInformation("Outgoing Response: {@LogDetails}", logDetails);

            using (var scope = _serviceScopeFactory.CreateScope())
            {
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
        }

        private async Task LogException(HttpContext context, Exception ex)
        {
            var username = context.Items["Username"]?.ToString() ?? "Anonymous";
            var originIp = GetClientIp(context);

            var logDetails = new
            {
                Username = username,
                Exception = ex.Message,
                InnerExceptionMessage = ex.InnerException?.Message,
                StackTrace = ex.StackTrace,
                HttpMethod = context.Request.Method,
                Url = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}",
                Origin = originIp,
                Timestamp = DateTime.Now
            };

            _logger.LogError("Exception occurred: {@LogDetails}", logDetails);

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var loggingService = scope.ServiceProvider.GetRequiredService<ILoggingService>();

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

                await loggingService.LogExceptionAsync(exceptionLog);
            }
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
