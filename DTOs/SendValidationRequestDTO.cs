namespace moneygram_api.DTOs
{
    public class SendValidationRequestDTO
    {
        public string OperatorName { get; set; }
        public decimal Amount { get; set; }
        public decimal FeeAmount { get; set; }
        public string DestinationCountry { get; set; }
        public string DeliveryOption { get; set; }
        public string ReceiveCurrency { get; set; }
        public string SenderFirstName { get; set; }
        public string SenderLastName { get; set; }
        public string SenderAddress { get; set; }
        public string SenderAddress2 { get; set; }
        public string SenderCity { get; set; }
        public string SenderCountry { get; set; }
        public string SenderHomePhone { get; set; }
        public string ReceiverFirstName { get; set; }
        public string ReceiverMiddleName { get; set; }
        public string ReceiverLastName { get; set; }
        public string ReceiverAddress { get; set; }
        public string ReceiverAddress2 { get; set; }
        public string ReceiverCity { get; set; }
        public string ReceiverCountry { get; set; }
        public string ReceiverPhone { get; set; }
        public string ReceiverPhoneCountryCode { get; set; }
        public string SenderPhotoIdType { get; set; }
        public string SenderPhotoIdNumber { get; set; }
        public string SenderPhotoIdCountry { get; set; }
        public string SenderLegalIdType { get; set; }
        public string SenderLegalIdNumber { get; set; }
        public DateTime SenderDOB { get; set; }
        public string SenderOccupation { get; set; }
        public string SenderBirthCountry { get; set; }
        public string SenderLegalIdIssueCountry { get; set; }
        public string SenderMobilePhone { get; set; }
        public string SenderMobilePhoneCountryCode { get; set; }
        public string SendCurrency { get; set; }
        public int ConsumerId { get; set; }
        public bool SenderPhotoIdStored { get; set; }
        public string SenderNationalityCountry { get; set; }
        public string SenderNationalityAtBirthCountry { get; set; }
        public string MgiTransactionSessionID { get; set; }
        public bool FormFreeStaging { get; set; }
        public string SendPurposeOfTransaction { get; set; }
        public string SourceOfFunds { get; set; }
        public string RelationshipToReceiver { get; set; }
        public string SenderGender { get; set; }
        public string SenderCitizenshipCountry { get; set; }
        public bool SenderTransactionSMSNotificationOptIn { get; set; }
        public bool SenderHomePhoneNotAvailable { get; set; }
        public string SenderHomePhoneCountryCode { get; set; }
    }
}