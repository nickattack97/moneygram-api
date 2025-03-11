using moneygram_api.Models.SendValidationRequest;
using moneygram_api.Models.SendValidationResponse;
using moneygram_api.Services.Interfaces;
using RestSharp;
using System.Threading.Tasks;
using moneygram_api.Settings;
using RequestEnvelope = moneygram_api.Models.SendValidationRequest.Envelope;
using RequestBody = moneygram_api.Models.SendValidationRequest.Body;
using ResponseEnvelope = moneygram_api.Models.SendValidationResponse.Envelope;
using KeyValuePair = moneygram_api.Models.SendValidationRequest.KeyValuePair;
using moneygram_api.DTOs;
using moneygram_api.Exceptions;
using moneygram_api.Utilities;
using System.Linq;

namespace moneygram_api.Services.Implementations
{
    public class SendValidation : ISendValidation
    {
        private readonly IConfigurations _configurations;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SendValidation(IConfigurations configurations, IHttpContextAccessor httpContextAccessor)
        {
            _configurations = configurations ?? throw new ArgumentNullException(nameof(configurations));
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<SendValidationResponse> Push(SendValidationRequestDTO request)
        {
            string operatorName = _httpContextAccessor.HttpContext?.Items["Username"]?.ToString();

            if (string.IsNullOrEmpty(operatorName))
            {
                throw new UnauthorizedAccessException("Username name not found in token. Re-authenticate.");
            }

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
            restRequest.AddHeader("SOAPAction", "urn:AgentConnect1512#sendValidation");
            restRequest.AddHeader("Content-Type", "application/xml");

            var envelope = new RequestEnvelope
            {
                Body = new RequestBody
                {
                    SendValidationRequest = new SendValidationRequest
                    {
                        AgentID = _configurations.AgentId,
                        AgentSequence = _configurations.Sequence,
                        Token = _configurations.Token,
                        TimeStamp = DateTime.UtcNow,
                        ApiVersion = _configurations.ApiVersion,
                        ClientSoftwareVersion = _configurations.ClientSoftwareVer,
                        ChannelType = "LOCATION",
                        OperatorName = operatorName.Length > 7 ? operatorName.Substring(0, 7) : operatorName,
                        Amount = request.Amount,
                        FeeAmount = request.FeeAmount,
                        DestinationCountry = request.DestinationCountry,
                        ReceiveAgentID = string.IsNullOrEmpty(request.ReceiveAgentID) ? null : request.ReceiveAgentID,
                        AccountNumber = string.IsNullOrEmpty(request.AccountNumber) ? null : request.AccountNumber,
                        MgiRewardsNumber = string.IsNullOrEmpty(request.RewardsNumber) ? null : request.RewardsNumber,
                        DeliveryOption = request.DeliveryOption,
                        ReceiveCurrency = request.ReceiveCurrency,
                        SenderFirstName = request.SenderFirstName,
                        SenderLastName = request.SenderLastName,
                        SenderAddress = request.SenderAddress,
                        SenderAddress2 = request.SenderAddress2,
                        SenderCity = request.SenderCity,
                        SenderCountry = request.SenderCountry,
                        SenderHomePhone = request.SenderHomePhone,
                        ReceiverFirstName = request.ReceiverFirstName,
                        ReceiverMiddleName = string.IsNullOrEmpty(request.ReceiverMiddleName) ? null : request.ReceiverMiddleName,
                        ReceiverLastName = request.ReceiverLastName,
                        ReceiverAddress = request.ReceiverAddress,
                        ReceiverAddress2 = request.ReceiverAddress2,
                        ReceiverCity = request.ReceiverCity,
                        ReceiverCountry = request.ReceiverCountry,
                        ReceiverPhone = request.ReceiverPhone,
                        ReceiverPhoneCountryCode = request.ReceiverPhoneCountryCode,
                        SenderPhotoIdType = request.SenderPhotoIdType,
                        SenderPhotoIdNumber = request.SenderPhotoIdNumber,
                        SenderPhotoIdCountry = request.SenderPhotoIdCountry,
                        SenderLegalIdType = request.SenderLegalIdType,
                        SenderLegalIdNumber = request.SenderLegalIdNumber,
                        SenderDOB = request.SenderDOB,
                        SenderOccupation = request.SenderOccupation,
                        SenderBirthCountry = request.SenderBirthCountry,
                        SenderLegalIdIssueCountry = request.SenderLegalIdIssueCountry,
                        SenderMobilePhone = request.SenderMobilePhone,
                        SenderMobilePhoneCountryCode = request.SenderMobilePhoneCountryCode,
                        SendCurrency = "USD", // Assuming hardcoded for now
                        ConsumerId = request.ConsumerId,
                        SenderPhotoIdStored = request.SenderPhotoIdStored,
                        SenderNationalityCountry = request.SenderNationalityCountry,
                        SenderNationalityAtBirthCountry = request.SenderNationalityAtBirthCountry,
                        MgiTransactionSessionID = request.MgiTransactionSessionID,
                        FormFreeStaging = request.FormFreeStaging,
                        SendPurposeOfTransaction = request.SendPurposeOfTransaction,
                        SourceOfFunds = request.SourceOfFunds,
                        RelationshipToReceiver = request.RelationshipToReceiver,
                        SenderGender = request.SenderGender,
                        SenderCitizenshipCountry = request.SenderCitizenshipCountry,
                        SenderIntendedUseOfMGIServices = request.SenderIntendedUseOfMGIServices,
                        SenderTransactionSMSNotificationOptIn = request.SenderTransactionSMSNotificationOptIn,
                        SenderHomePhoneNotAvailable = request.SenderHomePhoneNotAvailable,
                        SenderHomePhoneCountryCode = request.SenderHomePhoneCountryCode,
                        PromoCodeValues = string.IsNullOrEmpty(request.PromoCode) ? new List<string>() : new List<string> { request.PromoCode },
                        FieldValues = request.FieldValues != null ? request.FieldValues.Select(fv => new KeyValuePair
                        {
                            XmlTag = fv.XmlTag,
                            FieldValue = fv.FieldValue
                        }).ToList() : new List<KeyValuePair>()
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
                return responseEnvelope.Body.SendValidationResponse;
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
    }
}