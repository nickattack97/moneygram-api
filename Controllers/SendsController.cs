using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using moneygram_api.Services.Interfaces;
using moneygram_api.DTOs;
using moneygram_api.Utilities;
using moneygram_api.Models;
using moneygram_api.Exceptions;
using System.Threading.Tasks;

namespace moneygram_api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SendsController : ControllerBase
    {
        private readonly ISendConsumerLookUp _sendConsumerLookUp;
        private readonly IFeeLookUp _feeLookUp;
        private readonly IGFFP _gffp;
        private readonly ISendValidation _sendValidation;
        private readonly ICommitTransaction _commitTransaction;
        private readonly ICustomerLookupService _customerLookupService;
        private readonly IMGSendTransactionService _sendTransactionService;
        private readonly ISaveRewards _saveRewards;
        private readonly ILoggingService _loggingService;
        private readonly ILocalCodeTableService _localCodeTableService;
        private readonly ILocalCountryInfoService _localCountryInfoService;
        private readonly ILocalCurrencyInfoService _localCurrencyInfoService;
        private readonly IDetailLookup _detailLookup;
        private readonly ISendReversal _sendReversal; 
        private readonly IAmendTransaction _amendTransaction;

        public SendsController(
            ISendConsumerLookUp sendConsumerLookUp,
            IFeeLookUp feeLookUp,
            IGFFP gffp,
            ISendValidation sendValidation,
            ICommitTransaction commitTransaction,
            ICustomerLookupService customerLookupService,
            IMGSendTransactionService sendTransactionService,
            ILoggingService loggingService,
            ISaveRewards saveRewards,
            ILocalCodeTableService localCodeTableService,
            ILocalCountryInfoService localCountryInfoService,
            ILocalCurrencyInfoService localCurrencyInfoService,
            IDetailLookup detailLookup, 
            ISendReversal sendReversal, 
            IAmendTransaction amendTransaction) 
        {
            _sendConsumerLookUp = sendConsumerLookUp;
            _feeLookUp = feeLookUp;
            _gffp = gffp;
            _sendValidation = sendValidation;
            _commitTransaction = commitTransaction;
            _customerLookupService = customerLookupService;
            _sendTransactionService = sendTransactionService;
            _loggingService = loggingService;
            _saveRewards = saveRewards;
            _localCodeTableService = localCodeTableService;
            _localCountryInfoService = localCountryInfoService;
            _localCurrencyInfoService = localCurrencyInfoService;
            _detailLookup = detailLookup;
            _sendReversal = sendReversal; 
            _amendTransaction = amendTransaction; 
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
            return await HandleRequestAsync(() => _localCodeTableService.GetCodeTableAsync(request), "SendsController.CodeTable");
        }

        [HttpPost("filtered-code-table")]
        public async Task<IActionResult> FilteredCodeTable([FromBody] FilteredCodeTableRequestDTO request)
        {
            if (request == null)
            {
                return BadRequest(ErrorDictionary.GetErrorResponse(400, "filteredCodeTableRequest"));
            }
            return await HandleRequestAsync(() => _localCodeTableService.GetFilteredCodeTableAsync(request), "SendsController.FilteredCodeTable");
        }

        [HttpGet("country-info")]
        public async Task<IActionResult> CountryInfo([FromQuery] string? countryCode = null)
        {
            return await HandleRequestAsync(() => _localCountryInfoService.GetCountryInfoAsync(countryCode), "SendsController.CountryInfo");
        }

        [HttpGet("currency-info")]
        public async Task<IActionResult> CurrencyInfo()
        {
            return await HandleRequestAsync(() => _localCurrencyInfoService.GetCurrencyInfoAsync(), "SendsController.CurrencyInfo");
        }

        [HttpGet("filtered-currency-info/{currencyCode}")]
        public async Task<IActionResult> FilteredCurrencyInfo(string currencyCode)
        {
            if (string.IsNullOrEmpty(currencyCode))
            {
                return BadRequest(ErrorDictionary.GetErrorResponse(400, "currencyCode"));
            }
            return await HandleRequestAsync(() => _localCurrencyInfoService.GetFilteredCurrencyInfoAsync(currencyCode), "SendsController.FilteredCurrencyInfo");
        }

        [HttpPost("fee-lookup")]
        public async Task<IActionResult> FeeLookUp([FromBody] FeeLookUpRequestDTO request)
        {
            if (request == null)
            {
                return BadRequest(ErrorDictionary.GetErrorResponse(400, "feeLookupRequest"));
            }
            return await HandleRequestAsync(() => _feeLookUp.FetchFeeLookUp(request), "SendsController.FeeLookUp");
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
                return BadRequest(ErrorDictionary.GetErrorResponse(400, "gffpRequest"));
            }
            return await HandleRequestAsync(() => _gffp.FetchFieldsForProduct(request), "SendsController.GFFP");
        }

        [HttpPost("send-validation")]
        public async Task<IActionResult> SendValidation([FromBody] SendValidationRequestDTO request)
        {
            if (request == null)
            {
                return BadRequest(ErrorDictionary.GetErrorResponse(400, "sendValidationRequest"));
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
            return await HandleRequestAsync(() => _commitTransaction.Commit(request), "SendsController.CommitTransaction");
        }

        [HttpPost("log-transaction")]
        public async Task<IActionResult> LogTransaction([FromBody] MGSendTransactionDTO transaction)
        {
            if (transaction == null)
            {
                return BadRequest(ErrorDictionary.GetErrorResponse(400, "logTransactionRequest"));
            }
            return await HandleRequestAsync(() => _sendTransactionService.LogTransactionAsync(transaction), "SendsController.LogTransaction");
        }

        [HttpPost("save-rewards")]
        public async Task<IActionResult> SaveRewards([FromBody] SaveRewardsRequestDTO request)
        {
            if (request == null)
            {
                return BadRequest(ErrorDictionary.GetErrorResponse(400, "saveRewardsRequest"));
            }
            return await HandleRequestAsync(() => _saveRewards.Save(request), "SendsController.SaveRewards");
        }
        [HttpGet("detail-lookup/{referenceNumber}")]
        public async Task<IActionResult> DetailLookup(string referenceNumber)
        {
            if (string.IsNullOrEmpty(referenceNumber))
            {
            return BadRequest(ErrorDictionary.GetErrorResponse(400, "referenceNumber"));
            }
            return await HandleRequestAsync(() => _detailLookup.Lookup(referenceNumber), "SendsController.DetailLookup");
        }

        [HttpPost("send-reversal")]
        public async Task<IActionResult> SendReversal([FromBody] SendReversalRequestDTO request)
        {
            if (request == null)
            {
                return BadRequest(ErrorDictionary.GetErrorResponse(400, "sendReversalRequest"));
            }
            return await HandleRequestAsync(() => _sendReversal.Reverse(request), "SendsController.SendReversal");
        }

        [HttpPost("amend-transaction")]
        public async Task<IActionResult> AmendTransaction([FromBody] AmendTransactionRequestDTO request)
        {
            if (request == null)
            {
                return BadRequest(ErrorDictionary.GetErrorResponse(400, "amendTransactionRequest"));
            }
            return await HandleRequestAsync(() => _amendTransaction.Amend(request), "SendsController.AmendTransaction");
        }

        [HttpGet("transactions")]
        public async Task<IActionResult> GetSendTransactions()
        {
            return await HandleRequestAsync(() => _sendTransactionService.GetSendTransactionsAsync(), "SendsController.GetSendTransactions");
        }

        [HttpGet("transactions/{referenceNumber}")]
        public async Task<IActionResult> GetSendTransaction([FromRoute] string referenceNumber)
        {
            return await HandleRequestAsync(() => _sendTransactionService.GetTransactionByReferenceNumberAsync(referenceNumber), "SendsController.GetSendTransaction");
        }

        [HttpGet("transactions/my")]
        public async Task<IActionResult> GetMySendTransactions()
        {
            return await HandleRequestAsync(() => _sendTransactionService.GetMyTransactionsAsync(), "SendsController.GetMySendTransactions");
        }

        [HttpGet("national-ids")]
        public async Task<IActionResult> GetNationalIds([FromQuery] string search = null)
        {
            return await HandleRequestAsync(() => _sendTransactionService.GetNationalIdsAsync(search), "SendsController.GetNationalIds");
        }

        [HttpGet("mobile-numbers")]
        public async Task<IActionResult> GetMobileNumbers([FromQuery] string search = null)
        {
            return await HandleRequestAsync(() => _sendTransactionService.GetMobileNumbersAsync(search), "SendsController.GetMobileNumbers");
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
                    errorCode = ex.ErrorCode,
                    errorMessage = ex.ErrorMessage,
                    offendingField = ex.OffendingField,
                    timeStamp = ex.TimeStamp.ToString("o")
                };
                await LogExceptionAsync(ex, actionName);
                return StatusCode(500, soapFaultResponse);
            }
            catch (BaseCustomException ex)
            {
                var errorResponse = new
                {
                    errorCode = ex.ErrorCode,
                    errorMessage = ex.ErrorMessage,
                    offendingField = actionName, // Use actionName as the offending field context
                    timeStamp = ex.TimeStamp.ToString("o")
                };
                await LogExceptionAsync(ex, actionName);
                return StatusCode(ex.ErrorCode switch
                {
                    400 => 400,
                    401 => 401,
                    404 => 404,
                    503 => 503,
                    _ => 500
                }, errorResponse);
            }
            catch (Exception ex)
            {
                var errorResponse = ErrorDictionary.GetErrorResponse(500, ex.Message, actionName);
                var genericException = new
                {
                    errorCode = errorResponse.ErrorCode,
                    errorMessage = errorResponse.ErrorMessage,
                    offendingField = errorResponse.OffendingField,
                    timeStamp = DateTime.UtcNow.ToString("o")
                };
                await LogExceptionAsync(ex, actionName);
                return StatusCode(500, genericException);
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
                Timestamp = DateTime.UtcNow // Use UTC for consistency
            };

            await _loggingService.LogExceptionAsync(exceptionLog);
        }
    }
}