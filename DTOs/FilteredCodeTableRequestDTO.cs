namespace moneygram_api.DTOs
{
    public class FilteredCodeTableRequestDTO
    {
        public bool AgentAllowedOnly { get; set; }
        public string CountryCode { get; set; }
        public string DeliveryOption { get; set; }
    }
}