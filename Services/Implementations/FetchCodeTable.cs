using moneygram_api.Models.CodeTableRequest;
using moneygram_api.Models.CodeTableResponse;
using moneygram_api.Services.Interfaces;
using moneygram_api.Settings;
using moneygram_api.DTOs;
using moneygram_api.Data;
using RestSharp;
using System.Threading.Tasks;
using RequestEnvelope = moneygram_api.Models.CodeTableRequest.Envelope;
using RequestBody = moneygram_api.Models.CodeTableRequest.Body;
using ResponseEnvelope = moneygram_api.Models.CodeTableResponse.Envelope;
using moneygram_api.Exceptions;
using moneygram_api.Utilities;
using moneygram_api.Models;


namespace moneygram_api.Services.Implementations
{
    public class FetchCodeTable : IFetchCodeTable
    {
        private readonly IConfigurations _configurations;
        private readonly IFetchCurrencyInfo _fetchCurrencyInfo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _context;


        public FetchCodeTable(IConfigurations configurations, IFetchCurrencyInfo fetchCurrencyInfo, IHttpContextAccessor httpContextAccessor, AppDbContext context)
        {
            _configurations = configurations ?? throw new ArgumentNullException(nameof(configurations));
            _fetchCurrencyInfo = fetchCurrencyInfo ?? throw new ArgumentNullException(nameof(fetchCurrencyInfo));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<CodeTableResponse> Fetch(CodeTableRequestDTO request)
        {
            string operatorName = _httpContextAccessor.HttpContext?.Items["Username"]?.ToString()  ?? "Anonymous";

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
                        
            var xmlLog = new MoneyGramXmlLog
            {
                Operation = "CodeTable",
                RequestXml = XmlUtility.CleanXml(body),
                ResponseXml = XmlUtility.CleanXml(response.Content),
                LogTime = DateTime.UtcNow,
                Username = operatorName,
                HttpMethod = "POST",
                Url = "/api/sends/code-table"
            };

            await LogMoneyGramXmlAsync(xmlLog);

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

        private async Task LogMoneyGramXmlAsync(MoneyGramXmlLog xmlLog)
        {
            _context.MoneyGramXmlLogs.Add(xmlLog);
            await _context.SaveChangesAsync();
        }
    }
}