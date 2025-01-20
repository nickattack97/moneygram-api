namespace moneygram_api.Models.ConsumerLookUpResponse;

using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using moneygram_api.Utilities;

[XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
public class Envelope : ResponsesBaseEnvelope
{
    [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public Body Body { get; set; }
}

public class Body
{
    [XmlElement(ElementName = "moneyGramConsumerLookupResponse", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public MoneyGramConsumerLookupResponse MoneyGramConsumerLookupResponse { get; set; }
}

public class MoneyGramConsumerLookupResponse
{
    [XmlElement(ElementName = "doCheckIn", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public bool DoCheckIn { get; set; }

    [XmlElement(ElementName = "timeStamp", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public DateTime TimeStamp { get; set; }

    [XmlElement(ElementName = "flags", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public int Flags { get; set; }

    [XmlElement(ElementName = "senderInfo", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public List<SenderInfo> SenderInfo { get; set; }
}

public class SenderInfo
{
    [XmlElement(ElementName = "senderFirstName", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string SenderFirstName { get; set; }

    [XmlElement(ElementName = "senderMiddleName", Namespace = "http://www.moneygram.com/AgentConnect1512", IsNullable = true)]
    public string SenderMiddleName { get; set; }

    [XmlElement(ElementName = "senderLastName", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string SenderLastName { get; set; }

    [XmlElement(ElementName = "senderAddress", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string SenderAddress { get; set; }

    [XmlElement(ElementName = "senderAddress2", Namespace = "http://www.moneygram.com/AgentConnect1512", IsNullable = true)]
    public string SenderAddress2 { get; set; }

    [XmlElement(ElementName = "senderAddress3", Namespace = "http://www.moneygram.com/AgentConnect1512", IsNullable = true)]
    public string SenderAddress3 { get; set; }

    [XmlElement(ElementName = "senderCity", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string SenderCity { get; set; }

    [XmlElement(ElementName = "senderState", Namespace = "http://www.moneygram.com/AgentConnect1512", IsNullable = true)]
    public string SenderState { get; set; }

    [XmlElement(ElementName = "senderZipCode", Namespace = "http://www.moneygram.com/AgentConnect1512", IsNullable = true)]
    public string SenderZipCode { get; set; }

    [XmlElement(ElementName = "senderCountry", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string SenderCountry { get; set; }

    [XmlElement(ElementName = "senderHomePhone", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string SenderHomePhone { get; set; }

    [XmlElement(ElementName = "consumerId", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public int ConsumerId { get; set; }

    [XmlElement(ElementName = "senderBirthCountry", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string SenderBirthCountry { get; set; }

    [XmlElement(ElementName = "senderDOB", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public DateTime SenderDOB { get; set; }

    [XmlElement(ElementName = "senderHomePhoneCountryCode", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string SenderHomePhoneCountryCode { get; set; }

    [XmlElement(ElementName = "senderTransactionSMSNotificationOptIn", Namespace = "http://www.moneygram.com/AgentConnect1512", IsNullable = true)]
    public bool? SenderTransactionSMSNotificationOptIn { get; set; }

    [XmlElement(ElementName = "receiverInfo", Namespace = "http://www.moneygram.com/AgentConnect1512", IsNullable = true)]
    public ReceiverInfo ReceiverInfo { get; set; }
}

public class ReceiverInfo
{
    [XmlElement(ElementName = "receiveCountry", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string ReceiveCountry { get; set; }

    [XmlElement(ElementName = "receiverFirstName", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string ReceiverFirstName { get; set; }

    [XmlElement(ElementName = "receiverMiddleName", Namespace = "http://www.moneygram.com/AgentConnect1512", IsNullable = true)]
    public string ReceiverMiddleName { get; set; }

    [XmlElement(ElementName = "receiverLastName", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string ReceiverLastName { get; set; }

    [XmlElement(ElementName = "receiverAddress", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string ReceiverAddress { get; set; }

    [XmlElement(ElementName = "receiverAddress2", Namespace = "http://www.moneygram.com/AgentConnect1512", IsNullable = true)]
    public string ReceiverAddress2 { get; set; }

    [XmlElement(ElementName = "receiverAddress3", Namespace = "http://www.moneygram.com/AgentConnect1512", IsNullable = true)]
    public string ReceiverAddress3 { get; set; }

    [XmlElement(ElementName = "receiverCity", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string ReceiverCity { get; set; }

    [XmlElement(ElementName = "receiverCountry", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string ReceiverCountry { get; set; }

    [XmlElement(ElementName = "receiverPhone", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string ReceiverPhone { get; set; }

    [XmlElement(ElementName = "sendAmount", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public decimal SendAmount { get; set; }

    [XmlElement(ElementName = "deliveryOption", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string DeliveryOption { get; set; }

    [XmlElement(ElementName = "sendCurrency", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string SendCurrency { get; set; }

    [XmlElement(ElementName = "receiveCurrency", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string ReceiveCurrency { get; set; }

    [XmlElement(ElementName = "payoutCurrency", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string PayoutCurrency { get; set; }
}