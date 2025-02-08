using moneygram_api.Models.CountryInfoRequest;
using moneygram_api.Models.CountryInfoResponse;
using moneygram_api.Services.Interfaces;
using moneygram_api.Settings;
using moneygram_api.DTOs;
using RestSharp;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RequestEnvelope = moneygram_api.Models.CountryInfoRequest.Envelope;
using RequestBody = moneygram_api.Models.CountryInfoRequest.Body;
using ResponseEnvelope = moneygram_api.Models.CountryInfoResponse.Envelope;
using moneygram_api.Exceptions;
using moneygram_api.Utilities;

namespace moneygram_api.Services.Implementations
{
    public class GetCountryInfo : IGetCountryInfo
    {
        private readonly IConfigurations _configurations;

        public GetCountryInfo(IConfigurations configurations)
        {
            _configurations = configurations;
        }
        public async Task<CountryInfoResponse> Fetch(string? countryCode = null)
        {
            var options = new RestClientOptions(_configurations.BaseUrl)
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var restRequest = new RestRequest(_configurations.Resource, Method.Post);
            restRequest.AddHeader("SOAPAction", "urn:AgentConnect1512#countryInfo");
            restRequest.AddHeader("Content-Type", "application/xml");

            var envelope = new RequestEnvelope
            {
                Body = new RequestBody
                {
                    CountryInfoRequest = new CountryInfoRequest
                    {
                        AgentID = _configurations.AgentId,
                        Token = _configurations.Token,
                        AgentSequence = _configurations.Sequence,
                        TimeStamp = DateTime.Now,
                        ApiVersion = _configurations.ApiVersion,
                        ClientSoftwareVersion = _configurations.ClientSoftwareVer,
                        ChannelType = "LOCATION",
                        CountryFilter = "RECEIVE_ACTIVE",
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
                var distinctCountries = responseEnvelope.Body.CountryInfoResponse.CountryInfo
                    .GroupBy(c => c.CountryCode)
                    .Select(g => g.First())
                    .ToList();

                if (!string.IsNullOrEmpty(countryCode))
                {
                    distinctCountries = distinctCountries
                        .Where(c => c.CountryCode.Equals(countryCode, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                // Load country data from JSON
                CountryDataLoader.LoadCountryData("Media/countries_data.json");
                
                // Enrich data with additional fields
               foreach (var country in distinctCountries)
                {
                    var countryData = CountryDataLoader.GetCountryData(country.CountryCode);
                    if (countryData != null)
                    {
                        country.Phone = countryData.Phone?.FirstOrDefault();
                        country.Emoji = countryData.Emoji;
                        country.Image = countryData.Image;

                        if (countryData.PhoneLength is JArray phoneLengthArray)
                        {
                            country.PhoneLength = phoneLengthArray.ToObject<List<int>>();
                        }
                        else if (countryData.PhoneLength is JValue phoneLengthValue)
                        {
                            country.PhoneLength = new List<int> { phoneLengthValue.ToObject<int>() };
                        }
                    }
                }

                responseEnvelope.Body.CountryInfoResponse.CountryInfo = distinctCountries;
                return responseEnvelope.Body.CountryInfoResponse;
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