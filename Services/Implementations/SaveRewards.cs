using moneygram_api.DTOs;
using moneygram_api.Models.SaveRewardsRequest;
using moneygram_api.Models.SaveRewardsResponse;
using moneygram_api.Services.Interfaces;
using moneygram_api.Settings;
using RestSharp;
using System;
using System.Threading.Tasks;
using moneygram_api.Exceptions;
using moneygram_api.Utilities;
using moneygram_api.Models;

namespace moneygram_api.Services.Implementations
{
    public class SaveRewards : ISaveRewards
    {
        private readonly IConfigurations _configurations;
        private readonly SoapContext _soapContext;

        public SaveRewards(IConfigurations configurations, SoapContext soapContext)
        {
            _configurations = configurations ?? throw new ArgumentNullException(nameof(configurations));
            _soapContext = soapContext ?? throw new ArgumentNullException(nameof(soapContext));
        }

        public async Task<SaveRewardsResponse> Save(SaveRewardsRequestDTO request)
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
                MaxTimeout = 30000, // 30 seconds timeout
            };
            ProxySettingsUtility.ApplyProxySettings(options, _configurations);

            var client = new RestClient(options);
            var restRequest = new RestRequest(_configurations.Resource, Method.Post);
            restRequest.AddHeader("SOAPAction", "urn:AgentConnect1512#saveRewards");
            restRequest.AddHeader("Content-Type", "application/xml");
            restRequest.AddHeader("Cookie", "incap_ses_1018_2443955=SyYzAM4xd1oUKDXRyakgDt8JWGcAAAAAQYtntJhzk08U92hnA2tg2A==; nlbi_2443955=1rTMXGJcKWqxzeo5DQOeYgAAAAC2GJxMTQZ5Ec/5fuh3d4xW; visid_incap_2443955=2MrhAMFHS1izzAXJr0ZFVtuhD2cAAAAAQUIPAAAAAADXLrkPmKTaHmWdVzJQKR23");

            var envelope = new moneygram_api.Models.SaveRewardsRequest.Envelope
            {
                Body = new()
                {
                    SaveRewardsRequest = new()
                    {
                        AgentID = _configurations.AgentId,
                        Token = _configurations.Token,
                        AgentSequence = _configurations.Sequence,
                        ApiVersion = _configurations.ApiVersion,
                        ClientSoftwareVersion = _configurations.ClientSoftwareVer,
                        ChannelType = "LOCATION",
                        TimeStamp = DateTime.UtcNow,
                        Language = "EN",
                        ConsumerFirstName = request.ConsumerFirstName,
                        ConsumerLastName = request.ConsumerLastName,
                        ConsumerAddress = request.ConsumerAddress,
                        ConsumerAddress2 = request.ConsumerAddress2,
                        ConsumerCity = request.ConsumerCity,
                        ConsumerCountry = request.ConsumerCountry,
                        ConsumerHomePhone = request.ConsumerHomePhone,
                        ConsumerDOB = request.ConsumerDOB,
                        Gender = request.Gender == "MALE" ? "M" : "F",
                        MarketingOptIn = true,
                        MarketingBySMS = true,
                        MarketingLanguage = "EN"
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

                var responseEnvelope = moneygram_api.Models.SaveRewardsResponse.Envelope.Deserialize<moneygram_api.Models.SaveRewardsResponse.Envelope>(response.Content);
                return responseEnvelope.Body.SaveRewardsResponse;
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