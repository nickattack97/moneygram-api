using moneygram_api.Models.ConsumerLookUpRequest;
using moneygram_api.Models.ConsumerLookUpResponse;
using moneygram_api.Services.Interfaces;
using moneygram_api.DTOs;
using RestSharp;
using System.Threading.Tasks;
using moneygram_api.Settings;
using RequestEnvelope = moneygram_api.Models.ConsumerLookUpRequest.Envelope;
using RequestBody = moneygram_api.Models.ConsumerLookUpRequest.Body;
using ResponseEnvelope = moneygram_api.Models.ConsumerLookUpResponse.Envelope;
using moneygram_api.Exceptions;
using moneygram_api.Utilities;
using moneygram_api.Models;

namespace moneygram_api.Services.Implementations
{
    public class SendConsumerLookUp : ISendConsumerLookUp
    {
        private readonly IConfigurations _configurations;
        private readonly SoapContext _soapContext;

        public SendConsumerLookUp(IConfigurations configurations, SoapContext soapContext)
        {
            _configurations = configurations ?? throw new ArgumentNullException(nameof(configurations));
            _soapContext = soapContext ?? throw new ArgumentNullException(nameof(soapContext));
        }

        public async Task<MoneyGramConsumerLookupResponse> Push(ConsumerLookUpRequestDTO request)
        {
            if (request == null)
            {
                throw new BaseCustomException(
                    400,
                    "Request cannot be null.",
                    nameof(request),
                    DateTime.UtcNow
                );
            }

            var options = new RestClientOptions(_configurations.BaseUrl)
            {
                MaxTimeout = 30000, // 30 seconds timeout instead of infinite
            };

            // Apply proxy settings from configurations
            ProxySettingsUtility.ApplyProxySettings(options, _configurations);

            var client = new RestClient(options);
            var restRequest = new RestRequest(_configurations.Resource, Method.Post);
            restRequest.AddHeader("SOAPAction", "urn:AgentConnect1512#moneyGramConsumerLookup");
            restRequest.AddHeader("Content-Type", "application/xml");
            restRequest.AddHeader("Cookie", "incap_ses_1018_2443955=SyYzAM4xd1oUKDXRyakgDt8JWGcAAAAAQYtntJhzk08U92hnA2tg2A==; nlbi_2443955=1rTMXGJcKWqxzeo5DQOeYgAAAAC2GJxMTQZ5Ec/5fuh3d4xW; visid_incap_2443955=2MrhAMFHS1izzAXJr0ZFVtuhD2cAAAAAQUIPAAAAAADXLrkPmKTaHmWdVzJQKR23");

            var envelope = new RequestEnvelope
            {
                Body = new RequestBody
                {
                    MoneyGramConsumerLookupRequest = new MoneyGramConsumerLookupRequest
                    {
                        AgentID = _configurations.AgentId,
                        Token = _configurations.Token,
                        AgentSequence = _configurations.Sequence,
                        ApiVersion = _configurations.ApiVersion,
                        ClientSoftwareVersion = _configurations.ClientSoftwareVer,
                        ChannelType = "LOCATION",
                        TimeStamp = DateTime.UtcNow, // Use UTC for consistency
                        CustomerPhone = request.CustomerPhone ?? throw new BaseCustomException(
                            400,
                            "Customer phone number is required.",
                            nameof(request.CustomerPhone),
                            DateTime.UtcNow
                        ),
                        MaxReceiversToReturn = request.SendersToReturn,
                        MaxSendersToReturn = request.SendersToReturn,
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
                    throw new BaseCustomException(
                        500,
                        "Response content is null or empty.",
                        "responseContent",
                        DateTime.UtcNow
                    );
                }

                var responseEnvelope = ResponseEnvelope.Deserialize<ResponseEnvelope>(response.Content);
                return responseEnvelope.Body.MoneyGramConsumerLookupResponse;
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
                    throw new BaseCustomException(
                        500,
                        "Response content is null.",
                        "responseContent",
                        DateTime.UtcNow
                    );
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
    }
}