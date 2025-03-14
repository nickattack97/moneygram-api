namespace moneygram_api.DTOs
{
    using moneygram_api.Enums;
    using System.Xml.Serialization;

    public class SendReversalRequestDTO
    {
        public string ReferenceNumber { get; set; }
        public ReversalType ReversalType { get; set; }
        public SendReversalReason SendReversalReason { get; set; }
    }
}