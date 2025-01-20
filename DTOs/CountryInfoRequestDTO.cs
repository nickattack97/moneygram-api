namespace moneygram_api.DTOs
{
    public class CountryInfoRequestDTO
    {
        public bool All { get; set; }
        public string? CountryCode { get; set; }

        public CountryInfoRequestDTO(bool all, string? countryCode = null)
        {
            All = all;
            CountryCode = all ? null : countryCode;
        }
    }
}