using moneygram_api.Models.GFFPRequest;
using moneygram_api.Models.GFFPResponse;
using moneygram_api.Services.Interfaces;
using moneygram_api.DTOs;
using RestSharp;
using System.Threading.Tasks;
using moneygram_api.Settings;
using RequestEnvelope = moneygram_api.Models.GFFPRequest.Envelope;
using RequestBody = moneygram_api.Models.GFFPRequest.Body;
using ResponseEnvelope = moneygram_api.Models.GFFPResponse.Envelope;
using moneygram_api.Exceptions;
using moneygram_api.Utilities;

namespace moneygram_api.Services.Implementations
{
    public class SendGFFP : IGFFP
    {
        private readonly IConfigurations _configurations;

        public SendGFFP(IConfigurations configurations)
        {
            _configurations = configurations;
        }

        public async Task<GetFieldsForProductResponse> FetchFieldsForProduct(GFFPRequestDTO request)
        {
            var options = new RestClientOptions(_configurations.BaseUrl)
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var restRequest = new RestRequest(_configurations.Resource, Method.Post);
            restRequest.AddHeader("SOAPAction", "urn:AgentConnect1512#getFieldsForProduct");
            restRequest.AddHeader("Content-Type", "application/xml");

            var envelope = new RequestEnvelope
            {
                Body = new RequestBody
                {
                    GetFieldsForProductRequest = new GetFieldsForProductRequest
                    {
                        AgentID = _configurations.AgentId,
                        Token = _configurations.Token,
                        AgentSequence = _configurations.Sequence,
                        ApiVersion = _configurations.ApiVersion,
                        ClientSoftwareVersion = _configurations.ClientSoftwareVer,
                        ChannelType = "LOCATION",
                        TimeStamp = DateTime.Now,
                        ReceiveCountry = request.ReceiveCountry,
                        DeliveryOption = request.DeliveryOption,
                        ThirdPartyType = "NONE",
                        ReceiveCurrency = request.ReceiveCurrency,
                        ReceiveAgentID = request.ReceiveAgentID,    
                        Amount = request.Amount,
                        SendCurrency = request.SendCurrency,
                        ProductType = "SEND",
                        ConsumerId = 0,
                        FormFreeStaging = false,
                        CustomerReceiveNumber = string.IsNullOrEmpty(request.CustomerReceiveNumber) ? null : request.CustomerReceiveNumber
                    }
                }
            };

            // Remove the CustomerReceiveNumber element if it is null
            if (string.IsNullOrEmpty(request.CustomerReceiveNumber))
            {
                envelope.Body.GetFieldsForProductRequest.CustomerReceiveNumber = null;
            }

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
                return responseEnvelope.Body.GetFieldsForProductResponse;
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