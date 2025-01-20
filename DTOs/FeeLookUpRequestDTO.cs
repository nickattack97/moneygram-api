namespace moneygram_api.DTOs
{
    public class FeeLookUpRequestDTO
    {
        public decimal ReceiveAmount { get; set; }
        public required string ReceiveCountry { get; set; }
        public required string SendCountry { get; set; }
        public required string DeliveryOption { get; set; }
        public required string SendCurrency { get; set; }
    }
}