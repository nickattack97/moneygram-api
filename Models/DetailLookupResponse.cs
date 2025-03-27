namespace moneygram_api.Models.DetailLookupResponse
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class Envelope : ResponsesBaseEnvelope
    {
        [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public Body Body { get; set; }
    }

    public class Body
    {
        [XmlElement(ElementName = "detailLookupResponse", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public DetailLookupResponse DetailLookupResponse { get; set; }
    }

    public class DetailLookupResponse
    {
        [XmlElement(ElementName = "doCheckIn", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public bool DoCheckIn { get; set; }

        [XmlElement(ElementName = "timeStamp", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public DateTime TimeStamp { get; set; }

        [XmlElement(ElementName = "flags", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public int Flags { get; set; }

        [XmlElement(ElementName = "transactionStatus", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string TransactionStatus { get; set; }

        [XmlElement(ElementName = "dateTimeSent", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public DateTime DateTimeSent { get; set; }

        [XmlElement(ElementName = "referenceNumber", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string ReferenceNumber { get; set; }

        [XmlElement(ElementName = "freqCustCardNumber", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string FreqCustCardNumber { get; set; }

        [XmlElement(ElementName = "receiveCountry", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string ReceiveCountry { get; set; }

        [XmlElement(ElementName = "deliveryOption", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string DeliveryOption { get; set; }

        [XmlElement(ElementName = "senderFirstName", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string SenderFirstName { get; set; }

        [XmlElement(ElementName = "senderLastName", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string SenderLastName { get; set; }

        [XmlElement(ElementName = "senderAddress", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string SenderAddress { get; set; }

        [XmlElement(ElementName = "senderAddress2", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string SenderAddress2 { get; set; }

        [XmlElement(ElementName = "senderCity", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string SenderCity { get; set; }

        [XmlElement(ElementName = "senderCountry", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string SenderCountry { get; set; }

        [XmlElement(ElementName = "senderHomePhone", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string SenderHomePhone { get; set; }

        [XmlElement(ElementName = "receiverFirstName", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string ReceiverFirstName { get; set; }
        
        [XmlElement(ElementName = "receiverMiddleName", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string? ReceiverMiddleName { get; set; }

        [XmlElement(ElementName = "receiverLastName", Namespace = "http://www.moneygram.com/AgentConnect1512", IsNullable = true)]
        public string ReceiverLastName { get; set; }
        [XmlElement(ElementName = "receiverLastName2", Namespace = "http://www.moneygram.com/AgentConnect1512", IsNullable = true)]
        public string ReceiverLastName2 { get; set; }

        [XmlElement(ElementName = "receiverAddress", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string ReceiverAddress { get; set; }

        [XmlElement(ElementName = "receiverAddress2", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string? ReceiverAddress2 { get; set; }

        [XmlElement(ElementName = "receiverCity", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string ReceiverCity { get; set; }

        [XmlElement(ElementName = "receiverCountry", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string ReceiverCountry { get; set; }

        [XmlElement(ElementName = "receiverPhone", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string ReceiverPhone { get; set; }

        [XmlElement(ElementName = "direction1", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string Direction1 { get; set; }

        [XmlElement(ElementName = "direction2", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string Direction2 { get; set; }

        [XmlElement(ElementName = "direction3", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string Direction3 { get; set; }

        [XmlElement(ElementName = "testQuestion", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string TestQuestion { get; set; }

        [XmlElement(ElementName = "testAnswer", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string TestAnswer { get; set; }

        [XmlElement(ElementName = "messageField1", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string MessageField1 { get; set; }

        [XmlElement(ElementName = "messageField2", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string MessageField2 { get; set; }

        [XmlElement(ElementName = "senderPhotoIdType", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string SenderPhotoIdType { get; set; }

        [XmlElement(ElementName = "senderPhotoIdNumber", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string SenderPhotoIdNumber { get; set; }

        [XmlElement(ElementName = "senderPhotoIdCountry", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string SenderPhotoIdCountry { get; set; }

        [XmlElement(ElementName = "senderLegalIdType", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string SenderLegalIdType { get; set; }

        [XmlElement(ElementName = "senderLegalIdNumber", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string SenderLegalIdNumber { get; set; }

        [XmlIgnore]
        public DateTime SenderDOB { get; set; }

        [XmlElement(ElementName = "senderDOB", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string SenderDOBString
        {
            get => SenderDOB.ToString("yyyy-MM-dd");
            set => SenderDOB = DateTime.Parse(value);
        }

        [XmlElement(ElementName = "senderOccupation", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string SenderOccupation { get; set; }

        [XmlElement(ElementName = "thirdPartyLegalIdNumber", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string ThirdPartyLegalIdNumber { get; set; }

        [XmlElement(ElementName = "thirdPartyOccupation", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string ThirdPartyOccupation { get; set; }

        [XmlElement(ElementName = "thirdPartyOrg", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string ThirdPartyOrg { get; set; }

        [XmlElement(ElementName = "senderBirthCountry", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string SenderBirthCountry { get; set; }

        [XmlElement(ElementName = "operatorName", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string OperatorName { get; set; }

        [XmlElement(ElementName = "validIndicator", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public bool ValidIndicator { get; set; }

        [XmlElement(ElementName = "customerReceiveNumber", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string CustomerReceiveNumber { get; set; }

        [XmlElement(ElementName = "expectedDateOfDelivery", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string ExpectedDateOfDelivery { get; set; }

        [XmlElement(ElementName = "redirectIndicator", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public bool RedirectIndicator { get; set; }

        [XmlElement(ElementName = "agentCheckAuthorizationNumber", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string AgentCheckAuthorizationNumber { get; set; }

        [XmlElement(ElementName = "feeRefundRequired", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public bool FeeRefundRequired { get; set; }

        [XmlElement(ElementName = "sendAmounts", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public SendAmounts SendAmounts { get; set; }

        [XmlElement(ElementName = "receiveAmounts", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public ReceiveAmounts ReceiveAmounts { get; set; }

        [XmlElement(ElementName = "exchangeRateApplied", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public decimal ExchangeRateApplied { get; set; }
    }

    public class SendAmounts
    {
        [XmlElement(ElementName = "sendAmount", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public decimal SendAmount { get; set; }

        [XmlElement(ElementName = "sendCurrency", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string SendCurrency { get; set; }

        [XmlElement(ElementName = "totalSendFees", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public decimal TotalSendFees { get; set; }

        [XmlElement(ElementName = "totalDiscountAmount", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public decimal TotalDiscountAmount { get; set; }

        [XmlElement(ElementName = "totalSendTaxes", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public decimal TotalSendTaxes { get; set; }

        [XmlElement(ElementName = "detailSendAmounts", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public List<DetailSendAmounts> DetailSendAmounts { get; set; }
    }

    public class DetailSendAmounts
    {
        [XmlElement(ElementName = "amountType", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string AmountType { get; set; }

        [XmlElement(ElementName = "amount", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public decimal Amount { get; set; }

        [XmlElement(ElementName = "amountCurrency", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string AmountCurrency { get; set; }
    }

    public class ReceiveAmounts
    {
        [XmlElement(ElementName = "receiveAmount", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public decimal ReceiveAmount { get; set; }

        [XmlElement(ElementName = "receiveCurrency", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string ReceiveCurrency { get; set; }

        [XmlElement(ElementName = "validCurrencyIndicator", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public bool ValidCurrencyIndicator { get; set; }

        [XmlElement(ElementName = "payoutCurrency", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string PayoutCurrency { get; set; }

        [XmlElement(ElementName = "totalReceiveFees", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public decimal TotalReceiveFees { get; set; }

        [XmlElement(ElementName = "totalReceiveTaxes", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public decimal TotalReceiveTaxes { get; set; }

        [XmlElement(ElementName = "totalReceiveAmount", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public decimal TotalReceiveAmount { get; set; }

        [XmlElement(ElementName = "receiveFeesAreEstimated", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public bool ReceiveFeesAreEstimated { get; set; }

        [XmlElement(ElementName = "receiveTaxesAreEstimated", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public bool ReceiveTaxesAreEstimated { get; set; }

        [XmlElement(ElementName = "detailReceiveAmounts", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public List<DetailReceiveAmounts> DetailReceiveAmounts { get; set; }
    }

    public class DetailReceiveAmounts
    {
        [XmlElement(ElementName = "amountType", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string AmountType { get; set; }

        [XmlElement(ElementName = "amount", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public decimal Amount { get; set; }

        [XmlElement(ElementName = "amountCurrency", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string AmountCurrency { get; set; }
    }
}