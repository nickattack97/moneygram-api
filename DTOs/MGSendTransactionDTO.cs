using System;

namespace moneygram_api.DTOs
{
    public class MGSendTransactionDTO
    {
        public string? SessionID { get; set; }
        public string? ReferenceNumber { get; set; }
        public string SenderFirstName { get; set; }
        public string? SenderMiddleName { get; set; }
        public string SenderLastName { get; set; }
        public string SenderGender { get; set; }
        public string SenderDOB { get; set; }
        public string SenderPhotoIdType{ get; set; }
        public string SenderPhotoIdNumber { get; set; }
        public string SenderAddress1 { get; set; }
        public string SenderAddress2 { get; set; }
        public string SenderAddress3 { get; set; }
        public string? SenderZipCode { get; set; }
        public string SenderCity { get; set; }
        public string? SenderState { get; set; }
        public string SenderCountry { get; set; }
        public decimal Amount { get; set; }
        public string ReceiverFirstName { get; set; }
        public string ReceiverMiddleName { get; set; }
        public string ReceiverLastName { get; set; }
        public string ReceiverAddress1 { get; set; }
        public string ReceiverAddress2 { get; set; }
        public string ReceiverAddress3 { get; set; }
        public string ReceiverCity { get; set; }
        public string? ReceiverState { get; set; }
        public string? ReceiverZipCode { get; set; }
        public string ReceiverCountry { get; set; }
        public string ReceiverPhotoIDType { get; set; }
        public string ReceiverPhotoIDNumber { get; set; }
    }
}