using System;

namespace moneygram_api.DTOs
{
    public class MGSendTransactionDTO
    {
        public long? recordId { get; set; }
        public string? SessionID { get; set; }
        public string? RewardsNumber { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? SenderFirstName { get; set; }
        public string? SenderMiddleName { get; set; }
        public string? SenderLastName { get; set; }
        public string? SenderLastName2 { get; set; }
        public string? SenderGender { get; set; }
        public string? SenderDOB { get; set; }
        public string? SenderBirthCountry { get; set; }
        public string? SenderPhotoIdType{ get; set; }
        public string? SenderPhotoIdNumber { get; set; }
        public string? SenderPhotoIdCountry { get; set; }
        public string? SenderPhotoIdExpiryDate { get; set; }
        public string? SenderCitizenshipCountry { get; set; }
        public string? SenderPhoneNumber { get; set; }
        public string? SenderAddress1 { get; set; }
        public string? SenderAddress2 { get; set; }
        public string? SenderAddress3 { get; set; }
        public string? SenderZipCode { get; set; }
        public string? SenderCity { get; set; }
        public string? SenderState { get; set; }
        public string? SenderCountry { get; set; }
        public string? ReceiveCurrency { get; set; }
        public decimal? Charge { get; set; }
        public decimal? ReceiveAmount { get; set; }
        public decimal? SendAmount { get; set; }
        public decimal? ExchangeRate { get; set; }
        public decimal? TotalAmountCollected { get; set; }
        public string? ReceiverFirstName { get; set; }
        public string? ReceiverMiddleName { get; set; }
        public string? ReceiverLastName { get; set; }
        public string? ReceiverLastName2 { get; set; }
        public string? ReceiverAddress1 { get; set; }
        public string? ReceiverAddress2 { get; set; }
        public string? ReceiverAddress3 { get; set; }
        public string? ReceiverCity { get; set; }
        public string? ReceiverState { get; set; }
        public string? ReceiverZipCode { get; set; }
        public string? ReceiverCountry { get; set; }
        public string? ReceiverPhotoIDType { get; set; }
        public string? ReceiverPhotoIDNumber { get; set; }
        public string? ReceiverPhotoIDExpiryDate { get; set; }
        public string? ReceiverPhoneNumber { get; set; }
        public string? Occupation { get; set; }
        public string? TransactionPurpose { get; set; }
        public string? SourceOfFunds { get; set; }
        public string? ConsumerID { get; set; }
        public bool? Successful { get; set; }
        public bool? FormFree { get; set; }
        public DateTime? AddDate { get; set; }
        public bool? Committed { get; set; }
        public DateTime? CommitDate { get; set; }
        public bool? Processed { get; set; }
        public DateTime? ProcessDate { get; set; }
        public bool? Reversed { get; set; }
        public DateTime? ReversalTime { get; set; }
        public string? ReversalReason { get; set; }
        public string? ReversalTellerId { get; set; }
        public string? idImage { get; set; }
        public string? contentType { get; set; }
    }
}