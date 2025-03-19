using moneygram_api.Models.FeeLookUpRequest;
using moneygram_api.Models.FeeLookUpResponse;
using moneygram_api.Services.Interfaces;
using moneygram_api.DTOs;
using RestSharp;
using System.Threading.Tasks;
using moneygram_api.Settings;
using RequestEnvelope = moneygram_api.Models.FeeLookUpRequest.Envelope;
using RequestBody = moneygram_api.Models.FeeLookUpRequest.Body;
using ResponseEnvelope = moneygram_api.Models.FeeLookUpResponse.Envelope;
using moneygram_api.Exceptions;
using moneygram_api.Utilities;
using System.Linq;
using moneygram_api.Models;

namespace moneygram_api.Services.Implementations
{
    public class FeeLookUp : IFeeLookUp
    {
        private readonly IConfigurations _configurations;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SoapContext _soapContext;

        public FeeLookUp(IConfigurations configurations, IHttpContextAccessor httpContextAccessor, SoapContext soapContext)
        {
            _configurations = configurations ?? throw new ArgumentNullException(nameof(configurations));
            _httpContextAccessor = httpContextAccessor;
            _soapContext = soapContext ?? throw new ArgumentNullException(nameof(soapContext));
        }

        public async Task<FeeLookUpResponse> FetchFeeLookUp(FeeLookUpRequestDTO request)
        {
            string operatorName = _httpContextAccessor.HttpContext?.Items["Username"]?.ToString();

            if (string.IsNullOrEmpty(operatorName))
            {
                throw new UnauthorizedAccessException("Username name not found in token. Re-authenticate.");
            }

            if (request == null)
            {
                throw new BaseCustomException(400, "Request cannot be null.", nameof(request), DateTime.UtcNow);
            }

            var options = new RestClientOptions(_configurations.BaseUrl)
            {
                MaxTimeout = 30000,
            };
            ProxySettingsUtility.ApplyProxySettings(options, _configurations);

            var client = new RestClient(options);
            var restRequest = new RestRequest(_configurations.Resource, Method.Post);
            restRequest.AddHeader("SOAPAction", "urn:AgentConnect1512#feeLookUp");
            restRequest.AddHeader("Content-Type", "application/xml");
            restRequest.AddHeader("Cookie", "incap_ses_1018_2443955=SyYzAM4xd1oUKDXRyakgDt8JWGcAAAAAQYtntJhzk08U92hnA2tg2A==; nlbi_2443955=1rTMXGJcKWqxzeo5DQOeYgAAAAC2GJxMTQZ5Ec/5fuh3d4xW; visid_incap_2443955=2MrhAMFHS1izzAXJr0ZFVtuhD2cAAAAAQUIPAAAAAADXLrkPmKTaHmWdVzJQKR23");

            var envelope = new RequestEnvelope
            {
                Body = new RequestBody
                {
                    FeeLookUpRequest = new FeeLookUpRequest
                    {
                        AgentID = _configurations.AgentId,
                        Token = _configurations.Token,
                        AgentSequence = _configurations.Sequence,
                        ApiVersion = _configurations.ApiVersion,
                        ClientSoftwareVersion = _configurations.ClientSoftwareVer,
                        ChannelType = "LOCATION",
                        TimeStamp = DateTime.UtcNow,
                        ProductType = "SEND",
                        OperatorName = operatorName.Length > 7 ? operatorName.Substring(0, 7) : operatorName,
                        ReceiveAgentID = string.IsNullOrEmpty(request.ReceiveAgentID) ? null : request.ReceiveAgentID,
                        MgCustomerReceiveNumber = string.IsNullOrWhiteSpace(request.CustomerReceiveNumber) ? null : request.CustomerReceiveNumber,
                        MgiRewardsNumber = string.IsNullOrEmpty(request.RewardsNumber) ? null : request.RewardsNumber,
                        PromoCodeValues = string.IsNullOrEmpty(request.PromoCode) ? new List<string>() : new List<string> { request.PromoCode },
                        ReceiveAmount = request.ReceiveAmount,
                        ReceiveCountry = request.ReceiveCountry ?? throw new BaseCustomException(400, "Receive country is required.", nameof(request.ReceiveCountry), DateTime.UtcNow),
                        ReceiveCurrency = request.ReceiveCurrency ?? throw new BaseCustomException(400, "Receive currency is required.", nameof(request.ReceiveCurrency), DateTime.UtcNow),
                        SendCountry = request.SendCountry ?? throw new BaseCustomException(400, "Send country is required.", nameof(request.SendCountry), DateTime.UtcNow),
                        DeliveryOption = request.DeliveryOption ?? throw new BaseCustomException(400, "Delivery option is required.", nameof(request.DeliveryOption), DateTime.UtcNow),
                        SendCurrency = request.SendCurrency ?? throw new BaseCustomException(400, "Send currency is required.", nameof(request.SendCurrency), DateTime.UtcNow),
                        AllOptions = true
                    }
                }
            };

            var body = envelope.ToString();
            _soapContext.RequestXml = body;
            restRequest.AddParameter("application/xml", body, ParameterType.RequestBody);

            var response = await client.ExecuteAsync(restRequest);
            
            _soapContext.ResponseXml = response.Content;

            if (response.IsSuccessful)
            {
                if (string.IsNullOrEmpty(response.Content))
                {
                    throw new BaseCustomException(500, "Response content is null or empty.", "responseContent", DateTime.UtcNow);
                }

                var responseEnvelope = ResponseEnvelope.Deserialize<ResponseEnvelope>(response.Content);
                return responseEnvelope.Body.FeeLookUpResponse;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                if (response.Content != null)
                {
                    var (errorCode, errorString, offendingField, timeStamp) = SoapFaultParser.ParseSoapFault(response.Content);
                    throw new SoapFaultException(errorCode, errorString, offendingField, timeStamp);
                }
                else
                {
                    throw new BaseCustomException(500, "Response content is null.", "responseContent", DateTime.UtcNow);
                }
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
            {
                var errorResponse = ErrorDictionary.GetErrorResponse(503);
                throw new BaseCustomException(
                    errorResponse.ErrorCode,
                    errorResponse.ErrorMessage,
                    errorResponse.OffendingField,
                    DateTime.UtcNow
                );
            }
            else
            {
                var errorResponse = ErrorDictionary.GetErrorResponse(
                    (int)response.StatusCode,
                    response.Content ?? $"Request failed with status code {response.StatusCode}"
                );
                throw new BaseCustomException(
                    errorResponse.ErrorCode,
                    errorResponse.ErrorMessage,
                    errorResponse.OffendingField,
                    DateTime.UtcNow
                );
            }
        }

        public async Task<FeeLookUpResponse> FetchFilteredFeeLookUp(FeeLookUpRequestDTO filters)
        {
            var feeLookUpResponse = await FetchFeeLookUp(filters);

            var filteredFeeInfo = feeLookUpResponse.FeeInfo
                .Where(fi => fi.DeliveryOption == filters.DeliveryOption && fi.ValidReceiveCurrency == filters.ReceiveCurrency)
                .ToList();

            if (filteredFeeInfo.Count == 0)
            {
                var errorResponse = ErrorDictionary.GetErrorResponse(204);
                throw new BaseCustomException(
                    errorResponse.ErrorCode,
                    errorResponse.ErrorMessage,
                    errorResponse.OffendingField,
                    DateTime.UtcNow
                );
            }

            var filteredFeeInfoResponse = new FeeLookUpResponse
            {
                DoCheckIn = feeLookUpResponse.DoCheckIn,
                TimeStamp = feeLookUpResponse.TimeStamp,
                Flags = filteredFeeInfo.Count,
                FeeInfo = filteredFeeInfo,
            };

            return filteredFeeInfoResponse;
        }
    }
}