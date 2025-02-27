using moneygram_api.Models.CurrencyInfoRequest;
using moneygram_api.Models.CurrencyInfoResponse;
using moneygram_api.Services.Interfaces;
using moneygram_api.Settings;
using RestSharp;
using System.Threading.Tasks;
using RequestEnvelope = moneygram_api.Models.CurrencyInfoRequest.Envelope;
using RequestBody = moneygram_api.Models.CurrencyInfoRequest.Body;
using ResponseEnvelope = moneygram_api.Models.CurrencyInfoResponse.Envelope;
using moneygram_api.Exceptions;
using moneygram_api.Utilities;
using System.Linq;

namespace moneygram_api.Services.Implementations
{
    public class FetchCurrencyInfo : IFetchCurrencyInfo
    {
        private readonly IConfigurations _configurations;

        public FetchCurrencyInfo(IConfigurations configurations)
        {
            _configurations = configurations ?? throw new ArgumentNullException(nameof(configurations));
        }

        public async Task<CurrencyInfoResponse> Fetch()
        {
            var options = new RestClientOptions(_configurations.BaseUrl)
            {
                MaxTimeout = 30000,
            };
            ProxySettingsUtility.ApplyProxySettings(options, _configurations);

            var client = new RestClient(options);
            var restRequest = new RestRequest(_configurations.Resource, Method.Post);
            restRequest.AddHeader("SOAPAction", "urn:AgentConnect1512#currencyInfo");
            restRequest.AddHeader("Content-Type", "application/xml");

            var envelope = new RequestEnvelope
            {
                Body = new RequestBody
                {
                    CurrencyInfoRequest = new CurrencyInfoRequest
                    {
                        AgentID = _configurations.AgentId,
                        Token = _configurations.Token,
                        AgentSequence = _configurations.Sequence,
                        TimeStamp = DateTime.UtcNow,
                        ApiVersion = _configurations.ApiVersion,
                        ClientSoftwareVersion = _configurations.ClientSoftwareVer,
                        ChannelType = "LOCATION",
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
                    throw new BaseCustomException(
                        errorResponse.ErrorCode,
                        errorResponse.ErrorMessage,
                        errorResponse.OffendingField,
                        DateTime.UtcNow
                    );
                }
                return res;
            });

            if (response.IsSuccessful)
            {
                if (string.IsNullOrEmpty(response.Content))
                {
                    throw new BaseCustomException(500, "Response content is null or empty.", "responseContent", DateTime.UtcNow);
                }

                var responseEnvelope = ResponseEnvelope.Deserialize<ResponseEnvelope>(response.Content);
                return responseEnvelope.Body.CurrencyInfoResponse;
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

        public async Task<CurrencyInfoResponse> FetchByCurrencyCode(string currencyCode)
        {
            var currencyInfoResponse = await Fetch();
            var filteredCurrencyInfoList = currencyInfoResponse.CurrencyInfo
                .Where(c => c.CurrencyCode.Equals(currencyCode, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (filteredCurrencyInfoList == null || filteredCurrencyInfoList.Count == 0)
            {
                throw new BaseCustomException(
                    404,
                    $"Currency code {currencyCode} not found.",
                    "currencyCode",
                    DateTime.UtcNow
                );
            }

            var filteredCurrencyInfoResponse = new CurrencyInfoResponse
            {
                DoCheckIn = currencyInfoResponse.DoCheckIn,
                TimeStamp = currencyInfoResponse.TimeStamp,
                Flags = filteredCurrencyInfoList.Count,
                Version = currencyInfoResponse.Version,
                CurrencyInfo = filteredCurrencyInfoList
            };
            return filteredCurrencyInfoResponse;
        }
    }
}