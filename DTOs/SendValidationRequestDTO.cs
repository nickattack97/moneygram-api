using System;
using System.Collections.Generic;

namespace moneygram_api.DTOs
{
    public class SendValidationRequestDTO
    {
        public decimal Amount { get; set; }
        public decimal FeeAmount { get; set; }
        public string DestinationCountry { get; set; }
        public string DeliveryOption { get; set; }
        public string ReceiveCurrency { get; set; }
        public string? ReceiveAgentID { get; set; }
        public string? RewardsNumber { get; set; }
        public string? AccountNumber { get; set; }

        public string SenderFirstName 
        { 
            get => _senderFirstName; 
            set => _senderFirstName = value?.Trim(); 
        }
        private string _senderFirstName;

        public string? SenderMiddleName 
        { 
            get => _senderMiddleName; 
            set => _senderMiddleName = value?.Trim(); 
        }
        private string? _senderMiddleName;

        public string SenderLastName 
        { 
            get => _senderLastName; 
            set => _senderLastName = value?.Trim(); 
        }
        private string _senderLastName;

        public string? SenderLastName2 
        { 
            get => _senderLastName2; 
            set => _senderLastName2 = value?.Trim(); 
        }
        private string? _senderLastName2;

        public string ReceiverFirstName 
        { 
            get => _receiverFirstName; 
            set => _receiverFirstName = value?.Trim(); 
        }
        private string _receiverFirstName;

        public string? ReceiverMiddleName 
        { 
            get => _receiverMiddleName; 
            set => _receiverMiddleName = value?.Trim(); 
        }
        private string? _receiverMiddleName;

        public string ReceiverLastName 
        { 
            get => _receiverLastName; 
            set => _receiverLastName = value?.Trim(); 
        }
        private string _receiverLastName;

        public string? ReceiverLastName2 
        { 
            get => _receiverLastName2; 
            set => _receiverLastName2 = value?.Trim(); 
        }
        private string? _receiverLastName2;

        // Rest of the properties remain the same
        public string SenderAddress { get; set; }
        public string SenderAddress2 { get; set; }
        public string? SenderState { get; set; }
        public string? SenderZipCode { get; set; }
        public string SenderCity { get; set; }
        public string SenderCountry { get; set; }
        public string SenderHomePhone { get; set; }
        public string ReceiverAddress { get; set; }
        public string ReceiverAddress2 { get; set; }
        public string ReceiverCity { get; set; }
        public string ReceiverCountry { get; set; }
        public string ReceiverPhone { get; set; }
        public string ReceiverPhoneCountryCode { get; set; }
        public string SenderPhotoIdType { get; set; }
        public string SenderPhotoIdNumber { get; set; }
        public string? SenderPhotoIdExpiryDate { get; set; }
        public string SenderPhotoIdCountry { get; set; }
        public string SenderLegalIdType { get; set; }
        public string SenderLegalIdNumber { get; set; }
        public DateTime SenderDOB { get; set; }
        public string SenderOccupation { get; set; }
        public string? SenderBirthCountry { get; set; }
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
        public string SenderIntendedUseOfMGIServices { get; set; }
        public string? PromoCode { get; set; }
        public List<KeyValuePair>? FieldValues { get; set; }
    }

    public class KeyValuePair
    {
        public string XmlTag { get; set; }
        public string FieldValue { get; set; }
    }
}