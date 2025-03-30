namespace moneygram_api.DTOs
{
    public class PRIEligibilityRequestDTO
    {
        public string SenderPhotoIdNumber { get; set; } = string.Empty;
        public string ReceiverPhotoIdNumber { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}