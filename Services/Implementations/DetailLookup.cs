using moneygram_api.Models.DetailLookupRequest;
using moneygram_api.Models.DetailLookupResponse;
using moneygram_api.Services.Interfaces;
using RestSharp;
using System.Threading.Tasks;
using moneygram_api.Settings;
using RequestEnvelope = moneygram_api.Models.DetailLookupRequest.Envelope;
using ResponseEnvelope = moneygram_api.Models.DetailLookupResponse.Envelope;
using RequestBody = moneygram_api.Models.DetailLookupRequest.Body;
using moneygram_api.DTOs;
using moneygram_api.Exceptions;
using moneygram_api.Utilities;
using moneygram_api.Models;
using moneygram_api.Data;

namespace moneygram_api.Services.Implementations
{
    public class DetailLookup : IDetailLookup
    {
        private readonly IConfigurations _configurations;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SoapContext _soapContext;
        private readonly AppDbContext _context;

        public DetailLookup(IConfigurations configurations, IHttpContextAccessor httpContextAccessor, SoapContext soapContext, AppDbContext context)
        {
            _configurations = configurations ?? throw new ArgumentNullException(nameof(configurations));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _soapContext = soapContext ?? throw new ArgumentNullException(nameof(soapContext));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<DetailLookupResponse> Lookup(string? referenceNumber, string? transactionSessionId)
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
            restRequest.AddHeader("SOAPAction", "urn:AgentConnect1512#detailLookup");
            restRequest.AddHeader("Content-Type", "application/xml");
            restRequest.AddHeader("Cookie", "incap_ses_1018_2443955=..."); // Use actual cookie value

            var envelope = new RequestEnvelope
            {
                Body = new RequestBody
                {
                    DetailLookupRequest = new DetailLookupRequest
                    {
                        AgentID = _configurations.AgentId,
                        Token = _configurations.Token,
                        AgentSequence = _configurations.Sequence,
                        ApiVersion = _configurations.ApiVersion,
                        ClientSoftwareVersion = _configurations.ClientSoftwareVer,
                        ChannelType = "LOCATION",
                        ReferenceNumber = string.IsNullOrEmpty(referenceNumber) ? null : referenceNumber,
                        MgiTransactionSessionID = string.IsNullOrEmpty(transactionSessionId) ? null : transactionSessionId,
                        IncludeUseData = false,
                        OperatorName = operatorName.Length > 7 ? operatorName.Substring(0, 7) : operatorName,
                        TimeStamp = DateTime.UtcNow,
                    }
                }
            };

            var body = envelope.ToString();
            _soapContext.RequestXml = body;
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

            _soapContext.ResponseXml = response.Content;

            var xmlLog = new MoneyGramXmlLog
            {
                Operation = "DetailLookUp",
                RequestXml = XmlUtility.CleanXml(body),
                ResponseXml = XmlUtility.CleanXml(response.Content),
                LogTime = DateTime.UtcNow,
                Username = operatorName,
                HttpMethod = "GET",
                Url = "/api/sends/detail-look-up"
            };

            //await LogMoneyGramXmlAsync(xmlLog);

            if (response.IsSuccessful)
            {
                if (string.IsNullOrEmpty(response.Content))
                {
                    throw new BaseCustomException(500, "Response content is null or empty", "responseContent", DateTime.UtcNow);
                }
                var responseEnvelope = ResponseEnvelope.Deserialize<ResponseEnvelope>(response.Content);
                return responseEnvelope.Body.DetailLookupResponse;
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

        private async Task LogMoneyGramXmlAsync(MoneyGramXmlLog xmlLog)
        {
            _context.MoneyGramXmlLogs.Add(xmlLog);
            await _context.SaveChangesAsync();
        }
    }
}