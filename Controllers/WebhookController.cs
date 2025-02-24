using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using moneygram_api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using moneygram_api.Models;
using moneygram_api.Settings;
using Newtonsoft.Json;

namespace moneygram_api.Controllers
{
    [ApiController]
    [Route("sandbox/cbz-bank/moneygram")]
    [AllowAnonymous]
    public class WebhookController : ControllerBase
    {
        private readonly ILogger<WebhookController> _logger;
        private readonly IConfigurations _configurations;
        private readonly ISignatureVerificationService _signatureVerificationService;
        private readonly IIpValidationService _ipValidationService;
        private readonly ILoggingService _loggingService;

        public WebhookController(
            ILogger<WebhookController> logger,
            IConfigurations configurations,
            ISignatureVerificationService signatureVerificationService,
            IIpValidationService ipValidationService,
            ILoggingService loggingService)
        {
            _logger = logger;
            _configurations = configurations;
            _signatureVerificationService = signatureVerificationService;
            _ipValidationService = ipValidationService;
            _loggingService = loggingService;
        }

        [HttpPost("webhook_status_events")]
        public async Task<IActionResult> HandleTransactionStatusEvent()
        {
            try
            {
                // Step 1: Validate IP Address (Whitelisting)
                string clientIp = GetClientIp();
                if (!_ipValidationService.IsIpWhitelisted(HttpContext))
                {
                    var errorResponse = new
                    {
                        Message = $"Unauthorized. IP not whitelisted.",
                        ClientIp = clientIp,
                        Timestamp = DateTime.UtcNow
                    };
                    _logger.LogWarning($"Unauthorized access attempt from IP: {clientIp}");
                    return StatusCode((int)HttpStatusCode.Forbidden, errorResponse);
                }

                // Step 2: Read and parse the request body
                using var reader = new StreamReader(Request.Body, Encoding.UTF8);
                var requestBody = await reader.ReadToEndAsync();
                _logger.LogInformation($"Received Webhook Payload: {requestBody}");

                // Step 3: Validate the signature
                if (!Request.Headers.ContainsKey("Signature"))
                {
                    _logger.LogWarning("Missing Signature header");
                    return BadRequest("Missing Signature header");
                }

                var signatureHeader = Request.Headers["Signature"].ToString();
                var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                var destinationHost = Request.Host.ToString();

                if (!_signatureVerificationService.Verify(signatureHeader, timestamp, destinationHost, requestBody))
                {
                    _logger.LogWarning("Invalid Signature");
                    return BadRequest("Invalid Signature");
                }

                // Step 4: Deserialize the payload
                var payload = JsonConvert.DeserializeObject<TransactionStatusEventPayload>(requestBody);
                if (payload == null)
                {
                    _logger.LogWarning("Invalid Payload");
                    return BadRequest("Invalid Payload");
                }

                // Step 5: Process the event
                await ProcessTransactionStatusEvent(payload);

                // Step 6: Return success response
                return Ok("Event Processed");
            }
            catch (JsonException ex)
            {
                var clientIp = GetClientIp();
                var errorResponse = new
                {
                    ErrorCode = "INVALID_JSON",
                    ErrorMessage = "Invalid JSON payload",
                    OffendingField = "requestBody",
                    TimeStamp = DateTime.UtcNow
                };
                await LogExceptionAsync(ex, "WebhookController.HandleTransactionStatusEvent", clientIp);
                return StatusCode((int)HttpStatusCode.BadRequest, errorResponse);
            }
            catch (Exception ex)
            {
                var clientIp = GetClientIp();
                var errorResponse = new
                {
                    ErrorCode = "INTERNAL_ERROR",
                    ErrorMessage = "Internal Server Error",
                    OffendingField = "",
                    TimeStamp = DateTime.UtcNow
                };
                await LogExceptionAsync(ex, "WebhookController.HandleTransactionStatusEvent", clientIp);
                return StatusCode((int)HttpStatusCode.InternalServerError, errorResponse);
            }
        }

        private async Task ProcessTransactionStatusEvent(TransactionStatusEventPayload payload)
        {
            _logger.LogInformation($"Processing Event: {payload.EventId}, Status: {payload.EventPayload.TransactionStatus}");
            // Notification business logic here to process the event
            _logger.LogInformation($"Event Processed: {payload.EventId}");
        }

        private string GetClientIp()
        {
            return HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                ?? HttpContext.Request.Headers["X-Real-IP"].FirstOrDefault()
                ?? HttpContext.Connection.RemoteIpAddress?.ToString()
                ?? "unknown";
        }

        private async Task LogExceptionAsync(Exception ex, string actionName, string clientIp)
        {
            var exceptionLog = new ExceptionLog
            {
                Username = "Webhook", // No authenticated user context in webhook
                ExceptionMessage = ex.Message,
                InnerExceptionMessage = ex.InnerException?.Message,
                StackTrace = ex.StackTrace,
                HttpMethod = HttpContext.Request.Method,
                Url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.Path}{HttpContext.Request.QueryString}",
                Origin = clientIp,
                Timestamp = DateTime.Now
            };

            await _loggingService.LogExceptionAsync(exceptionLog);
            _logger.LogError(ex, $"Error in {actionName} from IP: {clientIp}");
        }
    }
}