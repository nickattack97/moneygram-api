namespace moneygram_api.Models.SendValidationRequest;

using System;
using System.Xml.Serialization;
using moneygram_api.Models;

[XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
public class Envelope : RequestsBaseEnvelope<Body>
{
    [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public new Body Body { get; set; }
}

public class Body
{
    [XmlElement(ElementName = "sendValidationRequest", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public SendValidationRequest SendValidationRequest { get; set; }
}

public class SendValidationRequest
{
    [XmlElement(ElementName = "agentID", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string AgentID { get; set; }

    [XmlElement(ElementName = "agentSequence", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public int AgentSequence { get; set; }

    [XmlElement(ElementName = "token", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string Token { get; set; }

    [XmlElement(ElementName = "timeStamp", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public DateTime TimeStamp { get; set; }

    [XmlElement(ElementName = "apiVersion", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public int ApiVersion { get; set; }

    [XmlElement(ElementName = "clientSoftwareVersion", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public int ClientSoftwareVersion { get; set; }

    [XmlElement(ElementName = "channelType", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string ChannelType { get; set; }

    [XmlElement(ElementName = "operatorName", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string OperatorName { get; set; }

    [XmlElement(ElementName = "amount", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public decimal Amount { get; set; }

    [XmlElement(ElementName = "feeAmount", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public decimal FeeAmount { get; set; }

    [XmlElement(ElementName = "destinationCountry", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string DestinationCountry { get; set; }

    [XmlElement(ElementName = "deliveryOption", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string DeliveryOption { get; set; }

    [XmlElement(ElementName = "receiveCurrency", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string ReceiveCurrency { get; set; }

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

    [XmlElement(ElementName = "receiverLastName", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string ReceiverLastName { get; set; }

    [XmlElement(ElementName = "receiverAddress", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string ReceiverAddress { get; set; }

    [XmlElement(ElementName = "receiverAddress2", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string ReceiverAddress2 { get; set; }

    [XmlElement(ElementName = "receiverCity", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string ReceiverCity { get; set; }

    [XmlElement(ElementName = "receiverCountry", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string ReceiverCountry { get; set; }

    [XmlElement(ElementName = "receiverPhone", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string ReceiverPhone { get; set; }

    [XmlElement(ElementName = "receiverPhoneCountryCode", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string ReceiverPhoneCountryCode { get; set; }

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
        get { return SenderDOB.ToString("yyyy-MM-dd"); }
        set { SenderDOB = DateTime.Parse(value); }
    }

    [XmlElement(ElementName = "senderOccupation", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string SenderOccupation { get; set; }

    [XmlElement(ElementName = "senderBirthCountry", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string SenderBirthCountry { get; set; }

    [XmlElement(ElementName = "senderLegalIdIssueCountry", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string SenderLegalIdIssueCountry { get; set; }

    [XmlElement(ElementName = "senderMobilePhone", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string SenderMobilePhone { get; set; }

    [XmlElement(ElementName = "senderMobilePhoneCountryCode", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string SenderMobilePhoneCountryCode { get; set; }

    [XmlElement(ElementName = "sendCurrency", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string SendCurrency { get; set; }

    [XmlElement(ElementName = "consumerId", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public int ConsumerId { get; set; }

    [XmlElement(ElementName = "senderPhotoIdStored", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public bool SenderPhotoIdStored { get; set; }

    [XmlElement(ElementName = "senderNationalityCountry", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string SenderNationalityCountry { get; set; }

    [XmlElement(ElementName = "senderNationalityAtBirthCountry", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string SenderNationalityAtBirthCountry { get; set; }

    [XmlElement(ElementName = "mgiTransactionSessionID", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string MgiTransactionSessionID { get; set; }

    [XmlElement(ElementName = "formFreeStaging", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public bool FormFreeStaging { get; set; }

    [XmlElement(ElementName = "sendPurposeOfTransaction", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string SendPurposeOfTransaction { get; set; }

    [XmlElement(ElementName = "sourceOfFunds", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string SourceOfFunds { get; set; }

    [XmlElement(ElementName = "relationshipToReceiver", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string RelationshipToReceiver { get; set; }

    [XmlElement(ElementName = "senderGender", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string SenderGender { get; set; }

    [XmlElement(ElementName = "senderCitizenshipCountry", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string SenderCitizenshipCountry { get; set; }

    [XmlElement(ElementName = "senderTransactionSMSNotificationOptIn", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public bool SenderTransactionSMSNotificationOptIn { get; set; }

    [XmlElement(ElementName = "senderHomePhoneNotAvailable", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public bool SenderHomePhoneNotAvailable { get; set; }

    [XmlElement(ElementName = "senderHomePhoneCountryCode", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string SenderHomePhoneCountryCode { get; set; }
    
    [XmlElement(ElementName = "senderIntendedUseOfMGIServices", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string SenderIntendedUseOfMGIServices { get; set; }
}