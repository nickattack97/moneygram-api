using moneygram_api.Models.SendReversalRequest;
using moneygram_api.Models.SendReversalResponse;
using moneygram_api.Services.Interfaces;
using RestSharp;
using System.Threading.Tasks;
using moneygram_api.Settings;
using RequestEnvelope = moneygram_api.Models.SendReversalRequest.Envelope;
using ResponseEnvelope = moneygram_api.Models.SendReversalResponse.Envelope;
using RequestBody = moneygram_api.Models.SendReversalRequest.Body;
using moneygram_api.DTOs;
using moneygram_api.Exceptions;
using moneygram_api.Utilities;
using moneygram_api.Models;

namespace moneygram_api.Services.Implementations
{
    public class SendReversal : ISendReversal
    {
        private readonly IConfigurations _configurations;
        private readonly IDetailLookup _detailLookup;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMGSendTransactionService _sendTransactionService; 
        private readonly SoapContext _soapContext;

        public SendReversal(
            IConfigurations configurations,
            IDetailLookup detailLookup,
            IHttpContextAccessor httpContextAccessor,
            IMGSendTransactionService sendTransactionService,
            SoapContext soapContext) 
        {
            _configurations = configurations ?? throw new ArgumentNullException(nameof(configurations));
            _detailLookup = detailLookup ?? throw new ArgumentNullException(nameof(detailLookup));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _sendTransactionService = sendTransactionService ?? throw new ArgumentNullException(nameof(sendTransactionService));
            _soapContext = soapContext ?? throw new ArgumentNullException(nameof(soapContext));
        }

        public async Task<SendReversalResponse> Reverse(SendReversalRequestDTO request)
        {
            string operatorName = _httpContextAccessor.HttpContext?.Items["Username"]?.ToString();

            if (string.IsNullOrEmpty(operatorName))
            {
                throw new UnauthorizedAccessException("Operator name not found in token. Re-authenticate.");
            }

            var transactionUpdate = await _sendTransactionService.GetTransactionByReferenceNumberAsync(request.ReferenceNumber);

            if(transactionUpdate == null)
            {
                throw new BaseCustomException(
                    404,
                    "Transaction not found",
                    "referenceNumber",
                    DateTime.UtcNow
                );
            }

            // Check transaction status first and fetch details
            var detailResponse = await _detailLookup.Lookup(request.ReferenceNumber);

            // Validate transaction status
            if (!new[] { "AVAIL", "AFR", "PRCSS" }.Contains(detailResponse.TransactionStatus))
            {
                throw new BaseCustomException(
                    400,
                    $"Transaction cannot be reversed due to invalid status: {detailResponse.TransactionStatus}",
                    "transactionStatus",
                    DateTime.UtcNow
                );
            }
    
            // Validate SendReversalReason requirement for ReversalType.R
            if (request.ReversalType == Enums.ReversalType.R && string.IsNullOrEmpty(request.SendReversalReason.ToString()))
            {
                throw new BaseCustomException(
                    400,
                    "SendReversalReason is required when ReversalType is R",
                    "sendReversalReason",
                    DateTime.UtcNow
                );
            }

            var options = new RestClientOptions(_configurations.BaseUrl)
            {
                MaxTimeout = 30000,
            };

            ProxySettingsUtility.ApplyProxySettings(options, _configurations);

            var client = new RestClient(options);
            var restRequest = new RestRequest(_configurations.Resource, Method.Post);
            restRequest.AddHeader("SOAPAction", "urn:AgentConnect1512#sendReversal");
            restRequest.AddHeader("Content-Type", "application/xml");
            restRequest.AddHeader("Cookie", "incap_ses_1018_2443955=...");

            var envelope = new RequestEnvelope
            {
                Body = new RequestBody
                {
                    SendReversalRequest = new SendReversalRequest
                    {
                        AgentID = _configurations.AgentId,
                        Token = _configurations.Token,
                        AgentSequence = _configurations.Sequence,
                        ApiVersion = _configurations.ApiVersion,
                        ClientSoftwareVersion = _configurations.ClientSoftwareVer,
                        ChannelType = "LOCATION",
                        SendAmount = detailResponse.SendAmounts.SendAmount,
                        FeeAmount = detailResponse.SendAmounts.TotalSendFees,
                        SendCurrency = "USD",
                        ReferenceNumber = request.ReferenceNumber,
                        OperatorName = operatorName.Length > 7 ? operatorName.Substring(0, 7) : operatorName,
                        ReversalType = request.ReversalType.ToString(),
                        FeeRefund = request.RefundFee.ToString(),
                        SendReversalReason = request.SendReversalReason.ToString(),
                        CommunicationRetryIndicator = false,
                        TimeStamp = DateTime.UtcNow,
                    }
                }
            };

            var body = envelope.ToString();
            _soapContext.RequestXml = body;
            restRequest.AddParameter("application/xml", body, ParameterType.RequestBody);

            var response = await RetryHelper.RetryOnExceptionAsync(3, async () =>
            {
                var res = await client.ExecuteAsync(restRequest);
                if (res.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
                {
                    var errorResponse = ErrorDictionary.GetErrorResponse(503);
                    throw new BaseCustomException(errorResponse.ErrorCode, errorResponse.ErrorMessage, errorResponse.OffendingField, DateTime.UtcNow);
                }
                return res;
            });
            
            _soapContext.ResponseXml = response.Content;

            if (response.IsSuccessful)
            {
                if (string.IsNullOrEmpty(response.Content))
                {
                    throw new BaseCustomException(500, "Response content is null or empty", "responseContent", DateTime.UtcNow);
                }
                var responseEnvelope = ResponseEnvelope.Deserialize<ResponseEnvelope>(response.Content);
                var reversalResponse = responseEnvelope.Body.SendReversalResponse;

                // Check if reversal was successful (agentCheckAuthorizationNumber is present)
                if (!string.IsNullOrEmpty(reversalResponse.AgentCheckAuthorizationNumber))
                {
                    // Update transaction with reversal details
                    transactionUpdate.Reversed = true;
                    transactionUpdate.ReversalTime = DateTime.UtcNow;
                    transactionUpdate.ReversalReason = request.SendReversalReason.ToString();
                    transactionUpdate.ReversalTellerId = operatorName;

                    // Prepare reversal details for logging to DTO
                    var transaction = new MGSendTransactionDTO
                    {
                        recordId = transactionUpdate.ID,
                        ReferenceNumber = transactionUpdate.ReferenceNumber,
                        Reversed = transactionUpdate.Reversed,
                        ReversalTime = transactionUpdate.ReversalTime,
                        ReversalReason = transactionUpdate.ReversalReason,
                        ReversalTellerId = transactionUpdate.ReversalTellerId
                    };
                    // Log transaction update
                    await _sendTransactionService.LogTransactionAsync(transaction);
                }

                return reversalResponse;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                var (errorCode, errorString, offendingField, timeStamp) = SoapFaultParser.ParseSoapFault(response.Content);
                if (errorCode == 605 && errorString.Contains("same day"))
                {
                    // Retry with ReversalType.R if ReversalType.C fails due to same-day restriction
                    request.ReversalType = Enums.ReversalType.R;
                    return await Reverse(request);
                }
                throw new SoapFaultException(errorCode, errorString, offendingField, timeStamp);
            }
            else
            {
                var errorResponse = ErrorDictionary.GetErrorResponse((int)response.StatusCode, response.Content ?? $"Request failed with status code {response.StatusCode}");
                throw new BaseCustomException(errorResponse.ErrorCode, errorResponse.ErrorMessage, errorResponse.OffendingField, DateTime.UtcNow);
            }
        }
    }
}