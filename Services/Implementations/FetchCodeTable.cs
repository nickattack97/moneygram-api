using moneygram_api.Models.CodeTableRequest;
using moneygram_api.Models.CodeTableResponse;
using moneygram_api.Services.Interfaces;
using moneygram_api.Settings;
using moneygram_api.DTOs;
using RestSharp;
using System.Threading.Tasks;
using RequestEnvelope = moneygram_api.Models.CodeTableRequest.Envelope;
using RequestBody = moneygram_api.Models.CodeTableRequest.Body;
using ResponseEnvelope = moneygram_api.Models.CodeTableResponse.Envelope;
using moneygram_api.Exceptions;
using moneygram_api.Utilities;

namespace moneygram_api.Services.Implementations
{
    public class FetchCodeTable : IFetchCodeTable
    {
        private readonly IConfigurations _configurations;
        private readonly IFetchCurrencyInfo _fetchCurrencyInfo;

        public FetchCodeTable(IConfigurations configurations, IFetchCurrencyInfo fetchCurrencyInfo)
        {
            _configurations = configurations ?? throw new ArgumentNullException(nameof(configurations));
            _fetchCurrencyInfo = fetchCurrencyInfo ?? throw new ArgumentNullException(nameof(fetchCurrencyInfo));
        }

        public async Task<CodeTableResponse> Fetch(CodeTableRequestDTO request)
        {
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
            restRequest.AddHeader("SOAPAction", "urn:AgentConnect1512#codeTableRequest");
            restRequest.AddHeader("Content-Type", "application/xml");

            var envelope = new RequestEnvelope
            {
                Body = new RequestBody
                {
                    CodeTableRequest = new CodeTableRequest
                    {
                        AgentID = _configurations.AgentId,
                        Token = _configurations.Token,
                        AgentSequence = _configurations.Sequence,
                        TimeStamp = DateTime.UtcNow,
                        ApiVersion = _configurations.ApiVersion,
                        ClientSoftwareVersion = _configurations.ClientSoftwareVer,
                        ChannelType = "LOCATION",
                        AgentAllowedOnly = request.AgentAllowedOnly
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
                return responseEnvelope.Body.CodeTableResponse;
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

        public async Task<CodeTableResponse> FetchFilteredCodeTable(FilteredCodeTableRequestDTO filters)
        {
            var request = new CodeTableRequestDTO { AgentAllowedOnly = filters.AgentAllowedOnly };
            var codeTableResponse = await Fetch(request);

            var filteredCountryCurrencyInfo = codeTableResponse.CountryCurrencyInfo
                .Where(cci => cci.CountryCode == filters.CountryCode)
                .ToList();

            var tasks = filteredCountryCurrencyInfo.Select(async currencyInfo =>
            {
                var receiveCurrencyInfo = await _fetchCurrencyInfo.FetchByCurrencyCode(currencyInfo.ReceiveCurrency);
                currencyInfo.ReceiveCurrencyName = receiveCurrencyInfo.CurrencyInfo.FirstOrDefault()?.CurrencyName;
            }).ToList();

            await Task.WhenAll(tasks);

            var filteredCodeTable = new CodeTableResponse
            {
                DoCheckIn = codeTableResponse.DoCheckIn,
                TimeStamp = codeTableResponse.TimeStamp,
                Flags = filteredCountryCurrencyInfo.Count,
                Version = codeTableResponse.Version,
                StateProvinceInfo = null,
                CountryCurrencyInfo = filteredCountryCurrencyInfo,
                DeliveryOptionInfo = null
            };

            return filteredCodeTable;
        }
    }
}