namespace moneygram_api.Models
{
    public class CodeTable
    {
        public int Id { get; set; }
        public string? CountryCode { get; set; }
        public string? StateProvinceCode { get; set; }
        public string? StateProvinceName { get; set; }
        public string? BaseCurrency { get; set; }
        public string? BaseCurrencyName { get; set; }
        public string? LocalCurrency { get; set; }
        public string? LocalCurrencyName { get; set; }
        public string? ReceiveCurrency { get; set; }
        public string? ReceiveCurrencyName { get; set; }
        public bool IndicativeRateAvailable { get; set; }
        public string? DeliveryOption { get; set; }
        public string? ReceiveAgentID { get; set; }
        public string? ReceiveAgentAbbreviation { get; set; }
        public string? MgManaged { get; set; }
        public string? AgentManaged { get; set; }
        public string Version { get; set; }

        public DateTime LastUpdated { get; set; }
    }

    public class CountryInfoEntity
    {
        public int Id { get; set; }
        public string? CountryCode { get; set; }
        public string? CountryName { get; set; }
        public string? CountryLegacyCode { get; set; }
        public bool SendActive { get; set; }
        public bool ReceiveActive { get; set; }
        public bool DirectedSendCountry { get; set; }
        public bool MgDirectedSendCountry { get; set; }
        public string? BaseReceiveCurrency { get; set; }
        public bool IsZipCodeRequired { get; set; }
        public string? Phone { get; set; }
        public string? Emoji { get; set; }
        public string? Image { get; set; }
        public string? PhoneLength { get; set; } // Store as JSON or delimited string
        public string Version { get; set; }
        public DateTime LastUpdated { get; set; }
    }

    public class CurrencyInfoEntity
    {
        public int Id { get; set; }
        public string? CurrencyCode { get; set; }
        public string? CurrencyName { get; set; }
        public int CurrencyPrecision { get; set; }
        public string Version { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}