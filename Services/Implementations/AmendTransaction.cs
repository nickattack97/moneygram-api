using moneygram_api.Models.AmendTransactionRequest;
using moneygram_api.Models.AmendTransactionResponse;
using moneygram_api.Services.Interfaces;
using RestSharp;
using System.Threading.Tasks;
using moneygram_api.Settings;
using RequestEnvelope = moneygram_api.Models.AmendTransactionRequest.Envelope;
using ResponseEnvelope = moneygram_api.Models.AmendTransactionResponse.Envelope;
using RequestBody = moneygram_api.Models.AmendTransactionRequest.Body;
using moneygram_api.DTOs;
using moneygram_api.Exceptions;
using moneygram_api.Utilities;

namespace moneygram_api.Services.Implementations
{
    public class AmendTransaction : IAmendTransaction
    {
        private readonly IConfigurations _configurations;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AmendTransaction(IConfigurations configurations, IHttpContextAccessor httpContextAccessor)
        {
            _configurations = configurations ?? throw new ArgumentNullException(nameof(configurations));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<AmendTransactionResponse> Amend(AmendTransactionRequestDTO request)
        {
            string operatorName = _httpContextAccessor.HttpContext?.Items["Username"]?.ToString();

            if (string.IsNullOrEmpty(operatorName))
            {
                throw new UnauthorizedAccessException("Operator name not found in token. Re-authenticate.");
            }

            var options = new RestClientOptions(_configurations.BaseUrl)
            {
                MaxTimeout = 30000,
            };

            ProxySettingsUtility.ApplyProxySettings(options, _configurations);

            var client = new RestClient(options);
            var restRequest = new RestRequest(_configurations.Resource, Method.Post);
            restRequest.AddHeader("SOAPAction", "urn:AgentConnect1512#amendTransaction");
            restRequest.AddHeader("Content-Type", "application/xml");
            restRequest.AddHeader("Cookie", "incap_ses_1018_2443955=...");

            var envelope = new RequestEnvelope
            {
                Body = new RequestBody
                {
                    AmendTransactionRequest = new AmendTransactionRequest
                    {
                        AgentID = _configurations.AgentId,
                        Token = _configurations.Token,
                        AgentSequence = _configurations.Sequence,
                        ApiVersion = _configurations.ApiVersion,
                        ClientSoftwareVersion = _configurations.ClientSoftwareVer,
                        ChannelType = "LOCATION",
                        ReferenceNumber = request.ReferenceNumber,
                        OperatorName = operatorName.Length > 7 ? operatorName.Substring(0, 7) : operatorName,
                        ReceiverFirstName = request.ReceiverFirstName,
                        ReceiverLastName = request.ReceiverLastName,
                        TimeStamp = DateTime.UtcNow,
                    }
                }
            };


            var body = envelope.ToString();
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

            if (response.IsSuccessful)
            {
                if (string.IsNullOrEmpty(response.Content))
                {
                    throw new BaseCustomException(500, "Response content is null or empty", "responseContent", DateTime.UtcNow);
                }
                var responseEnvelope = ResponseEnvelope.Deserialize<ResponseEnvelope>(response.Content);
                return responseEnvelope.Body.AmendTransactionResponse;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                var (errorCode, errorString, offendingField, timeStamp) = SoapFaultParser.ParseSoapFault(response.Content);
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