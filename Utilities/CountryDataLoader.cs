using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace moneygram_api.Utilities
{
    public static class CountryDataLoader
    {
        private static Dictionary<string, CountryJsonData> _countries;
        private static readonly object _lock = new object();

        public static void LoadCountryData(string filePath)
        {
            try
            {
                lock (_lock)
                {
                    if (_countries == null)
                    {
                        _countries = new Dictionary<string, CountryJsonData>();
                        using var streamReader = new StreamReader(filePath);
                        using var jsonReader = new JsonTextReader(streamReader);
                        var serializer = new JsonSerializer();
                        var countries = serializer.Deserialize<List<CountryJsonData>>(jsonReader);

                        if (countries == null)
                        {
                            throw new Exception("Country data is null");
                        }

                        foreach (var country in countries)
                        {
                            try
                            {
                                if (country == null || country.Iso == null || string.IsNullOrEmpty(country.Iso.Alpha3))
                                {
                                    // Skip the problematic record 
                                    continue;
                                }

                                _countries[country.Iso.Alpha3] = country;
                            }
                            catch (Exception ex)
                            {
                                // Log the exception and continue processing the next record
                                Console.WriteLine($"Error processing country record: {JsonConvert.SerializeObject(country)}. Error: {ex.Message}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle or log the exception as needed
                throw new Exception($"Error loading country data from file", ex);
            }
        }

        public static CountryJsonData GetCountryData(string alpha3Code)
        {
            try
            {
                _countries.TryGetValue(alpha3Code, out var countryData);
                return countryData;
            }
            catch (Exception ex)
            {
                // Handle or log the exception as needed
                throw new Exception($"Error retrieving country data for code {alpha3Code}", ex);
            }
        }
    }

    public class CountryJsonData
    {
        public string Name { get; set; }
        public string Region { get; set; }
        public Dictionary<string, string> Timezones { get; set; }
        public IsoInfo Iso { get; set; }
        public List<string> Phone { get; set; }
        public string Emoji { get; set; }
        public string Image { get; set; }
        public JToken PhoneLength { get; set; }
    }

    public class IsoInfo
    {
        [JsonProperty("alpha-2")]
        public string Alpha2 { get; set; }

        [JsonProperty("alpha-3")]
        public string Alpha3 { get; set; }

        public string Numeric { get; set; }
    }
}