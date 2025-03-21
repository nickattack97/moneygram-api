using moneygram_api.Models.CommitTransactionRequest;
using moneygram_api.Models.CommitTransactionResponse;
using moneygram_api.Services.Interfaces;
using RestSharp;
using moneygram_api.Settings;
using RequestEnvelope = moneygram_api.Models.CommitTransactionRequest.Envelope;
using ResponseEnvelope = moneygram_api.Models.CommitTransactionResponse.Envelope;
using RequestBody = moneygram_api.Models.CommitTransactionRequest.Body;
using moneygram_api.Models;
using moneygram_api.DTOs;
using moneygram_api.Exceptions;
using moneygram_api.Utilities;

namespace moneygram_api.Services.Implementations
{
    public class CommitTransaction : ICommitTransaction
    {
        private readonly IConfigurations _configurations;
        private readonly SoapContext _soapContext;
        private readonly IDetailLookup _detailLookup;

        public CommitTransaction(IConfigurations configurations, SoapContext soapContext, IDetailLookup detailLookup)
        {
            _configurations = configurations ?? throw new ArgumentNullException(nameof(configurations));
            _soapContext = soapContext ?? throw new ArgumentNullException(nameof(soapContext));
            _detailLookup = detailLookup ?? throw new ArgumentNullException(nameof(detailLookup));
        }

        public async Task<CommitTransactionResponse> Commit(CommitRequestDTO request)
        {
            var options = new RestClientOptions(_configurations.BaseUrl)
            {
                Timeout = TimeSpan.FromMilliseconds(30000), // Timeout (30 seconds)
            };

            // Apply proxy settings from configurations
            ProxySettingsUtility.ApplyProxySettings(options, _configurations);

            var client = new RestClient(options);
            var restRequest = new RestRequest(_configurations.Resource, Method.Post);
            restRequest.AddHeader("SOAPAction", "urn:AgentConnect1512#commitTransaction");
            restRequest.AddHeader("Content-Type", "application/xml");
            restRequest.AddHeader("Cookie", "incap_ses_1018_2443955=SyYzAM4xd1oUKDXRyakgDt8JWGcAAAAAQYtntJhzk08U92hnA2tg2A==; nlbi_2443955=1rTMXGJcKWqxzeo5DQOeYgAAAAC2GJxMTQZ5Ec/5fuh3d4xW; visid_incap_2443955=2MrhAMFHS1izzAXJr0ZFVtuhD2cAAAAAQUIPAAAAAADXLrkPmKTaHmWdVzJQKR23");

            var envelope = new RequestEnvelope
            {
                Body = new RequestBody
                {
                    CommitTransactionRequest = new CommitTransactionRequest
                    {
                        AgentID = _configurations.AgentId,
                        Token = _configurations.Token,
                        AgentSequence = _configurations.Sequence,
                        ApiVersion = _configurations.ApiVersion,
                        ClientSoftwareVersion = _configurations.ClientSoftwareVer,
                        ChannelType = "LOCATION",
                        ProductType = "SEND",
                        TimeStamp = DateTime.UtcNow,
                        MgiTransactionSessionID = request.mgiTransactionSessionID
                    }
                }
            };

            var body = envelope.ToString();
            _soapContext.RequestXml = body;
            restRequest.AddParameter("application/xml", body, ParameterType.RequestBody);

            try
            {
                // First attempt to directly execute the commit transaction request
                var response = await ExecuteCommitRequest(client, restRequest);
                return response;
            }
            catch (BaseCustomException ex)
            {
                // If we got a service unavailable error, throw it immediately
                if (ex.ErrorCode == 503)
                {
                    throw;
                }

                // For other errors, try to check the transaction status through detail lookup
                try
                {
                    var detailLookupResponse = await _detailLookup.Lookup(request.mgiTransactionSessionID);
                    
                    // If the transaction is already committed, return the successful status
                    if (detailLookupResponse.TransactionStatus.ToUpper() != "UNCOMMITTED")
                    {
                        return new CommitTransactionResponse
                        {
                            ReferenceNumber = detailLookupResponse.ReferenceNumber,
                            TransactionStatus = detailLookupResponse.TransactionStatus,
                            ExpectedDateOfDelivery = DateTime.Parse(detailLookupResponse.ExpectedDateOfDelivery),
                            TransactionDateTime = detailLookupResponse.TimeStamp,
                            DoCheckIn = detailLookupResponse.DoCheckIn,
                            Flags = detailLookupResponse.Flags
                        };
                    }
                    
                    // If still uncommitted, rethrow the original exception
                    throw;
                }
                catch (Exception lookupEx) when (!(lookupEx is BaseCustomException))
                {
                    // If the detail lookup also fails, throw the original exception
                    throw ex;
                }
            }
        }

        private async Task<CommitTransactionResponse> ExecuteCommitRequest(RestClient client, RestRequest request)
        {
            // Use the retry helper with proper return type
            var response = await RetryHelper.RetryOnExceptionAsync<RestResponse>(3, async () =>
            {
                var res = await client.ExecuteAsync(request);
                
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
            }, 15000); // 15-second initial delay

            _soapContext.ResponseXml = response.Content ?? string.Empty;

            if (response.IsSuccessful)
            {
                if (string.IsNullOrEmpty(response.Content))
                {
                    throw new BaseCustomException(
                        500,
                        "Response content is null or empty",
                        "responseContent",
                        DateTime.UtcNow
                    );
                }

                var responseEnvelope = ResponseEnvelope.Deserialize<ResponseEnvelope>(response.Content);
                return responseEnvelope.Body.CommitTransactionResponse;
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
                        "Response content is null",
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