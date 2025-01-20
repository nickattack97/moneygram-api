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

namespace moneygram_api.Services.Implementations
{
    public class FeeLookUp : IFeeLookUp
    {
        private readonly IConfigurations _configurations;

        public FeeLookUp(IConfigurations configurations)
        {
            _configurations = configurations;
        }

        public async Task<FeeLookUpResponse> FetchFeeLookUp(FeeLookUpRequestDTO request)
        {
            var options = new RestClientOptions(_configurations.BaseUrl)
            {
                MaxTimeout = -1,
            };
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
                        TimeStamp = DateTime.Now,
                        ProductType = "SEND",
                        OperatorName = "devTd",
                        ReceiveAmount = request.ReceiveAmount,
                        ReceiveCountry = request.ReceiveCountry ?? throw new ArgumentNullException(nameof(request.ReceiveCountry)),
                        SendCountry = request.SendCountry ?? throw new ArgumentNullException(nameof(request.SendCountry)),
                        DeliveryOption = request.DeliveryOption ?? throw new ArgumentNullException(nameof(request.DeliveryOption)),
                        SendCurrency = request.SendCurrency ?? throw new ArgumentNullException(nameof(request.SendCurrency)),
                        AllOptions = true
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
                    throw new Exception("Response content is null");
                }
            }
            else
            {
                throw new Exception($"Request failed with status code {response.StatusCode}: {response.Content}");
            }
        }
    }
}