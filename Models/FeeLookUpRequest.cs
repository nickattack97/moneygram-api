namespace moneygram_api.Models.FeeLookUpRequest;

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
    [XmlElement(ElementName = "feeLookupRequest", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public FeeLookUpRequest FeeLookUpRequest { get; set; }
}

public class FeeLookUpRequest
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

    [XmlElement(ElementName = "productType", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string ProductType { get; set; }

    [XmlElement(ElementName = "operatorName", Namespace = "http://www.moneygram.com/AgentConnect1512", IsNullable = true)]
    public string OperatorName { get; set; }

    [XmlElement(ElementName = "receiveAmount", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public decimal ReceiveAmount { get; set; }

    [XmlElement(ElementName = "receiveCountry", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string ReceiveCountry { get; set; }

    [XmlElement(ElementName = "sendCountry", Namespace = "http://www.moneygram.com/AgentConnect1512", IsNullable = true)]
    public string SendCountry { get; set; }

    [XmlElement(ElementName = "deliveryOption", Namespace = "http://www.moneygram.com/AgentConnect1512", IsNullable = true)]
    public string DeliveryOption { get; set; }

    [XmlElement(ElementName = "receiveCurrency", Namespace = "http://www.moneygram.com/AgentConnect1512", IsNullable = true)]
    public string ReceiveCurrency { get; set; }

    [XmlElement(ElementName = "sendCurrency", Namespace = "http://www.moneygram.com/AgentConnect1512", IsNullable = true)]
    public string SendCurrency { get; set; }

    [XmlElement(ElementName = "allOptions", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public bool AllOptions { get; set; }
}