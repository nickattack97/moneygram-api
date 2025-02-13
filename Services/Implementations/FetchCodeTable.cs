using moneygram_api.Models.CodeTableRequest;
using moneygram_api.Models.CodeTableResponse;
using moneygram_api.Services.Interfaces;
using moneygram_api.Settings;
using moneygram_api.DTOs;
using RestSharp;
using System;
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
            _configurations = configurations;
            _fetchCurrencyInfo = fetchCurrencyInfo;
        }

        public async Task<CodeTableResponse> Fetch(CodeTableRequestDTO request)
        {
            var options = new RestClientOptions(_configurations.BaseUrl)
            {
                MaxTimeout = -1,
            };
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
                        TimeStamp = DateTime.Now,
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
                    throw new Exception($"{errorResponse.ErrorMessage} - {errorResponse.OffendingField}");
                }
                return res;
            });

            if (response.IsSuccessful)
            {
                if (string.IsNullOrEmpty(response.Content))
                {
                    throw new Exception("Response content is null or empty");
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

        public async Task<CodeTableResponse> FetchFilteredCodeTable(FilteredCodeTableRequestDTO filters)
        {
            var request = new CodeTableRequestDTO { AgentAllowedOnly = filters.AgentAllowedOnly };

            var codeTableResponse = await Fetch(request);

            // Filter the CountryCurrencyInfo list based on the provided countryCode and deliveryOption.
            var filteredCountryCurrencyInfo = codeTableResponse.CountryCurrencyInfo
                .Where(cci => cci.CountryCode == filters.CountryCode /*&& cci.DeliveryOption == filters.DeliveryOption*/)
                .ToList();

            // Optimize: Fetch all currency info concurrently.
            var tasks = filteredCountryCurrencyInfo.Select(async currencyInfo =>
            {
                var receiveCurrencyInfo = await _fetchCurrencyInfo.FetchByCurrencyCode(currencyInfo.ReceiveCurrency);
                currencyInfo.ReceiveCurrencyName = receiveCurrencyInfo.CurrencyInfo.FirstOrDefault()?.CurrencyName;
            }).ToList();

            await Task.WhenAll(tasks);

            // Create a new CodeTableResponse with the filtered data.
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