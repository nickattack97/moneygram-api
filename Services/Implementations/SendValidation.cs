using moneygram_api.Models.SendValidationRequest;
using moneygram_api.Models.SendValidationResponse;
using moneygram_api.Services.Interfaces;
using RestSharp;
using System.Threading.Tasks;
using moneygram_api.Settings;
using RequestEnvelope = moneygram_api.Models.SendValidationRequest.Envelope;
using RequestBody = moneygram_api.Models.SendValidationRequest.Body;
using ResponseEnvelope = moneygram_api.Models.SendValidationResponse.Envelope;
using moneygram_api.DTOs;
using moneygram_api.Models;
using System.Xml.Serialization;
using System.Xml.Linq;
using moneygram_api.Exceptions;
using moneygram_api.Utilities;

namespace moneygram_api.Services.Implementations
{
    public class SendValidation : ISendValidation
    {
        private readonly IConfigurations _configurations;

        public SendValidation(IConfigurations configurations)
        {
            _configurations = configurations;
        }

        public async Task<SendValidationResponse> Push(SendValidationRequestDTO request)
        {
            var options = new RestClientOptions(_configurations.BaseUrl)
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var restRequest = new RestRequest(_configurations.Resource, Method.Post);
            restRequest.AddHeader("SOAPAction", "urn:AgentConnect1512#sendValidation");
            restRequest.AddHeader("Content-Type", "application/xml");
            restRequest.AddHeader("Cookie", "incap_ses_1018_2443955=GO7YG7wOAUj4UTbTyakgDiZcWWcAAAAAHJgpxrF9flvB+aG0TsxRQA==; incap_ses_1021_2443955=UQqIIZRyhgQc589kSlIrDghuWGcAAAAAhfdSbPQaLfOnP48BUlPoJA==; nlbi_2443955=1rTMXGJcKWqxzeo5DQOeYgAAAAC2GJxMTQZ5Ec/5fuh3d4xW; visid_incap_2443955=2MrhAMFHS1izzAXJr0ZFVtuhD2cAAAAAQUIPAAAAAADXLrkPmKTaHmWdVzJQKR23");

            var envelope = new RequestEnvelope
            {
                Body = new RequestBody
                {
                    SendValidationRequest = new SendValidationRequest
                    {
                        AgentID = _configurations.AgentId,
                        AgentSequence = _configurations.Sequence,
                        Token = _configurations.Token,
                        TimeStamp = DateTime.Now,
                        ApiVersion = _configurations.ApiVersion,
                        ClientSoftwareVersion = _configurations.ClientSoftwareVer,
                        ChannelType = "LOCATION",
                        OperatorName = request.OperatorName,
                        Amount = request.Amount,
                        FeeAmount = request.FeeAmount,
                        DestinationCountry = request.DestinationCountry,
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
                        ReceiverMiddleName = string.IsNullOrEmpty(request.ReceiverMiddleName) ? null : request.ReceiverMiddleName,                        ReceiverLastName = request.ReceiverLastName,
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
                        SendCurrency = "USD", //request.SendCurrency,
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
                        SenderTransactionSMSNotificationOptIn = request.SenderTransactionSMSNotificationOptIn,
                        SenderHomePhoneNotAvailable = request.SenderHomePhoneNotAvailable,
                        SenderHomePhoneCountryCode = request.SenderHomePhoneCountryCode
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