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
using System.Threading.Tasks;
using moneygram_api.Models;

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
        private readonly ICustomerLookupService _customerLookupService;
        private readonly IMGSendTransactionService _sendTransactionService;
        private readonly ILoggingService _loggingService; // Add this line

        public SendsController(ISendConsumerLookUp sendConsumerLookUp, IFetchCodeTable fetchCodeTable, IGetCountryInfo fetchCountryInfo, IFeeLookUp feeLookUp, IGFFP gffp, ISendValidation sendValidation, ICommitTransaction commitTransaction, ICustomerLookupService customerLookupService, IMGSendTransactionService sendTransactionService, ILoggingService loggingService) // Add ILoggingService parameter
        {
            _sendConsumerLookUp = sendConsumerLookUp;
            _fetchCodeTable = fetchCodeTable;
            _feeLookUp = feeLookUp;
            _fetchCountryInfo = fetchCountryInfo;
            _gffp = gffp;
            _sendValidation = sendValidation;
            _commitTransaction = commitTransaction;
            _customerLookupService = customerLookupService;
            _sendTransactionService = sendTransactionService;
            _loggingService = loggingService; 
        }

        [HttpPost]
        [Route("consumer-lookup")]
        public async Task<IActionResult> ConsumerLookUp([FromBody] ConsumerLookUpRequestDTO request)
        {
            if (request == null)
            {
                return BadRequest("Request is null");
            }

            return await HandleRequestAsync(() => _sendConsumerLookUp.Push(request), "SendsController.ConsumerLookUp");
        }
        
        [HttpGet]
        [Route("customer-lookup/{nationalID}")]
        public async Task<IActionResult> CustomerLookup(string nationalID)
        {
            if (string.IsNullOrEmpty(nationalID))
            {
                return BadRequest("National ID is null or empty");
            }

            return await HandleRequestAsync(() => _customerLookupService.GetCustomerByNationalIDAsync(nationalID), "SendsController.CustomerLookup");
        }

        [HttpPost]
        [Route("code-table")]
        public async Task<IActionResult> CodeTable([FromBody] CodeTableRequestDTO request)
        {
            if (request == null)
            {
                return BadRequest("Request is null");
            }

            return await HandleRequestAsync(() => _fetchCodeTable.Fetch(request), "SendsController.CodeTable");
        }
        [HttpPost]
        [Route("filtered-code-table")]
        public async Task<IActionResult> FilteredCodeTable([FromBody] FilteredCodeTableRequestDTO request)
        {
            if (request == null)
            {
                return BadRequest("Request is null");
            }

            return await HandleRequestAsync(() => _fetchCodeTable.FetchFilteredCodeTable(request), "SendsController.FilteredCodeTable");
        }

        [HttpGet]
        [Route("country-info")]
        public async Task<IActionResult> CountryInfo([FromQuery] string? countryCode = null)
        {
            return await HandleRequestAsync(() => _fetchCountryInfo.Fetch(countryCode), "SendsController.CountryInfo");
        }

        [HttpPost]
        [Route("fee-lookup")]
        public async Task<IActionResult> FeeLookUp([FromBody] FeeLookUpRequestDTO request)
        {
            if (request == null)
            {
                return BadRequest("Request is null");
            }

            return await HandleRequestAsync(() => _feeLookUp.FetchFeeLookUp(request), "SendsController.FeeLookUp");
        }


        [HttpPost]
        [Route("filtered-fee-lookup")]
        public async Task<IActionResult> FilteredFeeLookUp([FromBody] FeeLookUpRequestDTO request)
        {
            if (request == null)
            {
                return BadRequest("Request is null");
            }

            return await HandleRequestAsync(() => _feeLookUp.FetchFilteredFeeLookUp(request), "SendsController.FilteredFeeLookUp");
        }

        [HttpPost]
        [Route("gffp")]
        public async Task<IActionResult> GetFieldsForProduct([FromBody] GFFPRequestDTO request)
        {
            if (request == null)
            {
                return BadRequest("Request is null");
            }

            return await HandleRequestAsync(() => _gffp.FetchFieldsForProduct(request), "SendsController.GFFP");
        }

        [HttpPost]
        [Route("send-validation")]
        public async Task<IActionResult> SendValidation([FromBody] SendValidationRequestDTO request)
        {
            if (request == null)
            {
                return BadRequest("Request is null");
            }

            return await HandleRequestAsync(() => _sendValidation.Push(request), "SendsController.SendValidation");
        }

        [HttpPost]
        [Route("commit-transaction")]
        public async Task<IActionResult> CommitTransaction([FromBody] CommitRequestDTO request)
        {
            if (request == null)
            {
                return BadRequest("Request is null");
            }

            return await HandleRequestAsync(() => _commitTransaction.Commit(request), "SendsController.CommitTransaction");
        }

        [HttpPost]
        [Route("log-transaction")]
        public async Task<IActionResult> LogTransaction([FromBody] MGSendTransactionDTO transaction)
        {
            if (transaction == null)
            {
                return BadRequest("Transaction is null");
            }

            return await HandleRequestAsync(() => _sendTransactionService.LogTransactionAsync(transaction), "SendsController.LogTransaction");
        }

        private async Task<IActionResult> HandleRequestAsync<T>(Func<Task<T>> func, string offendingField)
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
                await LogExceptionAsync(ex, offendingField); // Log the exception
                return StatusCode(500, soapFaultResponse);
            }
            catch (Exception ex)
            {
                var genericException = new
                {
                    ErrorCode = 400,
                    ErrorMessage = ex.Message,
                    OffendingField = offendingField,
                    TimeStamp = DateTime.Now
                };
                await LogExceptionAsync(ex, offendingField); // Log the exception
                return StatusCode(400, genericException);
            }
        }

        private async Task LogExceptionAsync(Exception ex, string offendingField)
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