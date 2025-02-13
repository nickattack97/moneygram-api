using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using moneygram_api.Models.ConsumerLookUpRequest;
using moneygram_api.Models.ConsumerLookUpResponse;
using moneygram_api.Models.SendValidationRequest;
using moneygram_api.Models.SendValidationResponse;
using moneygram_api.Models.CommitTransactionRequest;
using moneygram_api.Models.CommitTransactionResponse;
using moneygram_api.Services.Interfaces;
using moneygram_api.DTOs;
using moneygram_api.Exceptions;
using moneygram_api.Utilities;
using System.Threading.Tasks;
using moneygram_api.Models;
using moneygram_api.Utilities;

namespace moneygram_api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SendsController : ControllerBase
    {
        private readonly ISendConsumerLookUp _sendConsumerLookUp;
        private readonly IFetchCodeTable _fetchCodeTable;
        private readonly IFeeLookUp _feeLookUp;
        private readonly IGFFP _gffp;
        private readonly ISendValidation _sendValidation;
        private readonly ICommitTransaction _commitTransaction;
        private readonly IGetCountryInfo _fetchCountryInfo;
        private readonly IFetchCurrencyInfo _fetchCurrencyInfo;
        private readonly ICustomerLookupService _customerLookupService;
        private readonly IMGSendTransactionService _sendTransactionService;
        private readonly ILoggingService _loggingService; 

        public SendsController(
            ISendConsumerLookUp sendConsumerLookUp,
            IFetchCodeTable fetchCodeTable,
            IGetCountryInfo fetchCountryInfo,
            IFetchCurrencyInfo fetchCurrencyInfo,
            IFeeLookUp feeLookUp,
            IGFFP gffp,
            ISendValidation sendValidation,
            ICommitTransaction commitTransaction,
            ICustomerLookupService customerLookupService,
            IMGSendTransactionService sendTransactionService,
            ILoggingService loggingService
        )
        {
            _sendConsumerLookUp = sendConsumerLookUp;
            _fetchCodeTable = fetchCodeTable;
            _feeLookUp = feeLookUp;
            _fetchCountryInfo = fetchCountryInfo;
            _fetchCurrencyInfo = fetchCurrencyInfo;
            _gffp = gffp;
            _sendValidation = sendValidation;
            _commitTransaction = commitTransaction;
            _customerLookupService = customerLookupService;
            _sendTransactionService = sendTransactionService;
            _loggingService = loggingService;
            _sendTransactionService = sendTransactionService;
        }

        [HttpPost("consumer-lookup")]
        public async Task<IActionResult> ConsumerLookUp([FromBody] ConsumerLookUpRequestDTO request)
        {
            if (request == null)
            {
                return BadRequest(ErrorDictionary.GetErrorResponse(400, "consumerLookUpRequest"));
            }

            return await HandleRequestAsync(() => _sendConsumerLookUp.Push(request), "SendsController.ConsumerLookUp");
        }
        
        [HttpGet("customer-lookup/{nationalID}")]
        public async Task<IActionResult> CustomerLookup(string nationalID)
        {
            if (string.IsNullOrEmpty(nationalID))
            {
                return BadRequest(ErrorDictionary.GetErrorResponse(400, "nationalID"));
            }

            return await HandleRequestAsync(() => _customerLookupService.GetCustomerByNationalIDAsync(nationalID), "SendsController.CustomerLookup");
        }

        [HttpPost("code-table")]
        public async Task<IActionResult> CodeTable([FromBody] CodeTableRequestDTO request)
        {
            if (request == null)
            {
                return BadRequest(ErrorDictionary.GetErrorResponse(400, "codeTableRequest"));
            }

            return await HandleRequestAsync(() => _fetchCodeTable.Fetch(request), "SendsController.CodeTable");
        }
        [HttpPost("filtered-code-table")]
        public async Task<IActionResult> FilteredCodeTable([FromBody] FilteredCodeTableRequestDTO request)
        {
            if (request == null)
            {
                return BadRequest(ErrorDictionary.GetErrorResponse(400, "filteredCodeTableRequest"));
            }

            return await HandleRequestAsync(() => _fetchCodeTable.FetchFilteredCodeTable(request), "SendsController.FilteredCodeTable");
        }
        [HttpGet("country-info")]
        public async Task<IActionResult> CountryInfo([FromQuery] string? countryCode = null)
        {
            return await HandleRequestAsync(() => _fetchCountryInfo.Fetch(countryCode), "SendsController.CountryInfo");
        }

        [HttpGet("currency-info")]
        public async Task<IActionResult> CurrencyInfo()
        {
            return await HandleRequestAsync(() => _fetchCurrencyInfo.Fetch(), "CurrencyInfo");
        }

        [HttpGet("filtered-currency-info/{currencyCode}")]
        public async Task<IActionResult> FilteredCurrencyInfo(string currencyCode)
        {
            if (string.IsNullOrEmpty(currencyCode))
            {
                return BadRequest(ErrorDictionary.GetErrorResponse(400, "currencyCode"));
            }

            return await HandleRequestAsync(() => _fetchCurrencyInfo.FetchByCurrencyCode(currencyCode), "FilteredCurrencyInfo");
        }

        [HttpPost("fee-lookup")]
        public async Task<IActionResult> FeeLookUp([FromBody] FeeLookUpRequestDTO request)
        {
            if (request == null)
            {
                return BadRequest(ErrorDictionary.GetErrorResponse(400, "feeLookupRequest"));
            }

            return await HandleRequestAsync(() => _feeLookUp.FetchFeeLookUp(request), "FeeLookUp");
        }
        [HttpPost("filtered-fee-lookup")]
        public async Task<IActionResult> FilteredFeeLookUp([FromBody] FeeLookUpRequestDTO request)
        {
            if (request == null)
            {
                return BadRequest(ErrorDictionary.GetErrorResponse(400, "feeLookupRequest"));
            }

            return await HandleRequestAsync(() => _feeLookUp.FetchFilteredFeeLookUp(request), "SendsController.FilteredFeeLookUp");
        }
        [HttpPost("gffp")]
        public async Task<IActionResult> GetFieldsForProduct([FromBody] GFFPRequestDTO request)
        {
            if (request == null)
            {
                return BadRequest("Request is null");
            }

            return await HandleRequestAsync(() => _gffp.FetchFieldsForProduct(request), "SendsController.GFFP");
        }

        [HttpPost("send-validation")]
        public async Task<IActionResult> SendValidation([FromBody] SendValidationRequestDTO request)
        {
            if (request == null)
            {
                return BadRequest("Request is null");
            }

            return await HandleRequestAsync(() => _sendValidation.Push(request), "SendsController.SendValidation");
        }
        [HttpPost("commit-transaction")]
        public async Task<IActionResult> CommitTransaction([FromBody] CommitRequestDTO request)
        {
            if (request == null)
            {
                return BadRequest(ErrorDictionary.GetErrorResponse(400, "commitTransactionRequest"));
            }

            return await HandleRequestAsync(() => _commitTransaction.Commit(request), "CommitTransaction");
        }

        [HttpPost("log-transaction")]
        public async Task<IActionResult> LogTransaction([FromBody] MGSendTransactionDTO transaction)
        {
            if (transaction == null)
            {
                return BadRequest(ErrorDictionary.GetErrorResponse(400, "logTransactionRequest"));
            }

            return await HandleRequestAsync(() => _sendTransactionService.LogTransactionAsync(transaction), "LogTransaction");
        }

        [HttpGet("get-send-transactions")]
        public async Task<IActionResult> GetSendTransactions()
       {
            return await HandleRequestAsync(() => _sendTransactionService.GetSendTransactionsAsync(), "GetSendTransactions");
        }
        private async Task<IActionResult> HandleRequestAsync<T>(Func<Task<T>> func, string actionName)
        {
            try
            {
                var response = await func();
                return Ok(response);
            }
            catch (SoapFaultException ex)
            {
                var soapFaultResponse = new
                {
                    ex.ErrorCode,
                    ex.ErrorMessage,
                    ex.OffendingField,
                    ex.TimeStamp
                };
                await LogExceptionAsync(ex, actionName);
                return StatusCode(500, soapFaultResponse);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Service Unavailable"))
                {
                    var errorResponse = ErrorDictionary.GetErrorResponse(503, actionName);
                    var serviceUnavailableResponse = new
                    {
                        errorResponse.ErrorCode,
                        errorResponse.ErrorMessage,
                        errorResponse.OffendingField,
                        TimeStamp = DateTime.Now
                    };
                    await LogExceptionAsync(ex, actionName);
                    return StatusCode(503, serviceUnavailableResponse);
                }
                else if (ex.Message.Contains("No fee information found for the provided filters"))
                {
                    var errorResponse = ErrorDictionary.GetErrorResponse(204, actionName);
                    var noFeeInfoResponse = new
                    {
                        errorResponse.ErrorCode,
                        errorResponse.ErrorMessage,
                        errorResponse.OffendingField,
                        TimeStamp = DateTime.Now
                    };
                    await LogExceptionAsync(ex, actionName);
                    return StatusCode(404, noFeeInfoResponse);
                }
                else
                {
                    var error = ErrorDictionary.GetErrorResponse(500, actionName);
                    var genericException = new
                    {
                        error.ErrorCode,
                        error.ErrorMessage,
                        error.OffendingField,
                        TimeStamp = DateTime.UtcNow
                    };
                    await LogExceptionAsync(ex, actionName);
                    return StatusCode(500, genericException);
                }
            }
        }

        private async Task LogExceptionAsync(Exception ex, string actionName)
        {
            var exceptionLog = new ExceptionLog
            {
                Username = User.Identity?.Name ?? "Anonymous",
                ExceptionMessage = ex.Message,
                InnerExceptionMessage = ex.InnerException?.Message,
                StackTrace = ex.StackTrace,
                HttpMethod = HttpContext.Request.Method,
                Url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.Path}{HttpContext.Request.QueryString}",
                Origin = HttpContext.Connection.RemoteIpAddress?.ToString(),
                Timestamp = DateTime.Now
            };

            await _loggingService.LogExceptionAsync(exceptionLog);
        }
    }
}