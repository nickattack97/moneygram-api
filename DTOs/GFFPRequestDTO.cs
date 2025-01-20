namespace moneygram_api.DTOs
{
    public class GFFPRequestDTO
    {
        public decimal Amount { get; set; }
        public required string ReceiveCurrency { get; set; }
        public required string CustomerReceiveNumber { get; set; }
        public required string ReceiveCountry { get; set; }
        public required string SendCurrency { get; set; }
        public required string DeliveryOption { get; set; }
    }
}