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
using Microsoft.AspNetCore.Http;

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

                // Step 3: Validate the signature header
                if (!Request.Headers.ContainsKey("Signature"))
                {
                    _logger.LogWarning("Missing Signature header");
                    return BadRequest("Missing Signature header");
                }

                var signatureHeader = Request.Headers["Signature"].ToString();
                var (headerTimestamp, signature) = _signatureVerificationService.ParseSignatureHeader(signatureHeader);

                if (headerTimestamp == 0 || string.IsNullOrEmpty(signature))
                {
                    _logger.LogWarning("Invalid signature header format: {SignatureHeader}", signatureHeader);
                    return BadRequest("Invalid signature header format");
                }

                // Step 4: Check request freshness (65-minute window)
                var currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                if (Math.Abs(currentTimestamp - headerTimestamp) > 65 * 60) // 65 minutes in seconds
                {
                    _logger.LogWarning("Request too old. Header timestamp: {HeaderTimestamp}, Current timestamp: {CurrentTimestamp}, SignatureHeader: {SignatureHeader}",
                        headerTimestamp, currentTimestamp, signatureHeader);
                    return BadRequest("Request too old");
                }

                // Step 5: Verify the signature
                var destinationHost = Request.Host.ToString();
                if (!_signatureVerificationService.Verify(signatureHeader, headerTimestamp, destinationHost, requestBody))
                {
                    _logger.LogWarning("Invalid Signature: {SignatureHeader}, Headers: {Headers}",
                        signatureHeader, SerializeHeaders(Request.Headers));
                    return BadRequest("Invalid Signature");
                }

                // Step 6: Deserialize the payload
                var payload = JsonConvert.DeserializeObject<TransactionStatusEventPayload>(requestBody);
                if (payload == null)
                {
                    _logger.LogWarning("Invalid Payload, SignatureHeader: {SignatureHeader}, Headers: {Headers}",
                        signatureHeader, SerializeHeaders(Request.Headers));
                    var errorResponse = new
                    {
                        ErrorCode = "INVALID_PAYLOAD",
                        ErrorMessage = "The provided payload is invalid",
                        Details = new { ReceivedPayload = requestBody },
                        TimeStamp = DateTime.UtcNow
                    };
                    return BadRequest(errorResponse);
                }

                // Step 7: Process the event asynchronously
                await ProcessTransactionStatusEvent(payload);

                // Step 8: Return success response
                return Ok();
            }
            catch (Exception ex)
            {
                await LogExceptionAsync(ex, "WebhookController.HandleTransactionStatusEvent", GetClientIp());
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    ErrorCode = "INTERNAL_ERROR",
                    ErrorMessage = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        private async Task ProcessTransactionStatusEvent(TransactionStatusEventPayload payload)
        {
            _logger.LogInformation("Processing Event: {EventId}, Status: {TransactionStatus}", payload.EventId, payload.EventPayload.TransactionStatus);
            // Add your notification or business logic here to process the event
            _logger.LogInformation("Event Processed: {EventId}", payload.EventId);
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
            var signatureHeader = HttpContext.Request.Headers["Signature"].ToString();
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
            _logger.LogError(ex, "Error in {ActionName} from IP: {ClientIp}, SignatureHeader: {SignatureHeader}, Headers: {Headers}",
                actionName, clientIp, signatureHeader, SerializeHeaders(HttpContext.Request.Headers));
        }

        private string SerializeHeaders(IHeaderDictionary headers)
        {
            var headerStrings = headers.Select(h => $"{h.Key}: {string.Join(", ", h.Value)}");
            return string.Join("; ", headerStrings);
        }
    }
}