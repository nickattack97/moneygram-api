namespace moneygram_api.Services.Implementations
{
    using System;
    using System.Threading.Tasks;
    using moneygram_api.DTOs;
    using moneygram_api.Models.SaveRewardsRequest;
    using moneygram_api.Models.SaveRewardsResponse;
    using moneygram_api.Services.Interfaces;
    using moneygram_api.Settings;
    using RestSharp;
    using moneygram_api.Exceptions;
    using moneygram_api.Utilities;

    public class SaveRewards : ISaveRewards
    {
        private readonly IConfigurations _configurations;

        public SaveRewards(IConfigurations configurations)
        {
            _configurations = configurations;
        }

        public async Task<SaveRewardsResponse> Save(SaveRewardsRequestDTO request)
        {
            var options = new RestClientOptions(_configurations.BaseUrl)
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var restRequest = new RestRequest(_configurations.Resource, Method.Post);
            restRequest.AddHeader("SOAPAction", "urn:AgentConnect1512#saveRewards");
            restRequest.AddHeader("Content-Type", "application/xml");
            restRequest.AddHeader("Cookie", "incap_ses_1018_2443955=SyYzAM4xd1oUKDXRyakgDt8JWGcAAAAAQYtntJhzk08U92hnA2tg2A==; nlbi_2443955=1rTMXGJcKWqxzeo5DQOeYgAAAAC2GJxMTQZ5Ec/5fuh3d4xW; visid_incap_2443955=2MrhAMFHS1izzAXJr0ZFVtuhD2cAAAAAQUIPAAAAAADXLrkPmKTaHmWdVzJQKR23");

            var envelope = new moneygram_api.Models.SaveRewardsRequest.Envelope
            {
                Body = new moneygram_api.Models.SaveRewardsRequest.Body
                {
                    SaveRewardsRequest = new SaveRewardsRequest
                    {
                        AgentID = _configurations.AgentId,
                        Token = _configurations.Token,
                        AgentSequence = _configurations.Sequence,
                        ApiVersion = _configurations.ApiVersion,
                        ClientSoftwareVersion = _configurations.ClientSoftwareVer,
                        ChannelType = "LOCATION",
                        TimeStamp = DateTime.Now,
                        Language = "EN",
                        ConsumerFirstName = request.ConsumerFirstName,
                        ConsumerLastName = request.ConsumerLastName,
                        ConsumerAddress = request.ConsumerAddress,
                        ConsumerAddress2 = request.ConsumerAddress2,
                        ConsumerCity = request.ConsumerCity,
                        ConsumerCountry = request.ConsumerCountry,
                        ConsumerHomePhone = request.ConsumerHomePhone,
                        ConsumerMobilePhone = request.ConsumerMobilePhone,
                        ConsumerDOB = request.ConsumerDOB,
                        Gender = request.Gender,
                        MarketingOptIn = request.MarketingOptIn,
                        MarketingBySMS = request.MarketingBySMS,
                        MarketingLanguage = "EN",
                        ConsentOfPersonalInformationThirdParty = request.ConsentOfPersonalInformationThirdParty,
                        AgentAcknowledgement = request.AgentAcknowledgement
                    }
                }
            };

            var body = envelope.ToString();

            restRequest.AddParameter("application/xml", body, ParameterType.RequestBody);

            var response = await client.ExecuteAsync(restRequest);

            if (response.IsSuccessful)
            {
                if (string.IsNullOrEmpty(response.Content))
                {
                    throw new Exception("Response content is null or empty");
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
                    throw new Exception("Response content is null");
                }
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
            {
                var errorResponse = ErrorDictionary.GetErrorResponse(503);
                throw new Exception($"{errorResponse.ErrorMessage} - {errorResponse.OffendingField}");
            }
            else
            {
                throw new Exception($"Request failed with status code {response.StatusCode}: {response.Content}");
            }
        }
    }
}