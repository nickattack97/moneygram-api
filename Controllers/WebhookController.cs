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
                // Step 1: Validate IP Address
                var clientIp = GetClientIp();
                if (!_ipValidationService.IsIpWhitelisted(HttpContext))
                {
                    _logger.LogWarning("Unauthorized access attempt from IP: {ClientIp}", clientIp);
                    return StatusCode((int)HttpStatusCode.Forbidden, new { Message = "IP not whitelisted", ClientIp = clientIp });
                }

                // Step 2: Read and parse the request body
                var requestBody = await new StreamReader(Request.Body).ReadToEndAsync();
                _logger.LogInformation("Received Webhook Payload: {RequestBody}", requestBody);

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

                    var errorResponse = new
                    {
                        ErrorCode = "INVALID_SIGNATURE",
                        ErrorMessage = "The provided signature is invalid",
                        Details = new
                        {
                            ReceivedSignature = signatureHeader,
                            ExpectedFormat = "t=<timestamp>,s=<base64_signature>",
                        },
                        TimeStamp = DateTime.UtcNow
                    };
                    return BadRequest(errorResponse);
                }

                // Step 5: Process the event asynchronously
                await ProcessTransactionStatusEvent(payload);

                // Step 6: Return success response
                return Ok();
            }
            catch (Exception ex)
            {
                await LogExceptionAsync(ex, "WebhookController.HandleTransactionStatusEvent", GetClientIp());
                return StatusCode((int)HttpStatusCode.InternalServerError, new { ErrorCode = "INTERNAL_ERROR", ErrorMessage = "Internal Server Error" });
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