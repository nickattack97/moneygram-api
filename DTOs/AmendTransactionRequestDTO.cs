namespace moneygram_api.DTOs
{
    public class AmendTransactionRequestDTO
    {
        public string ReferenceNumber { get; set; }
        public string ReceiverFirstName { get; set; }
        public string ReceiverLastName { get; set; }
    }
}