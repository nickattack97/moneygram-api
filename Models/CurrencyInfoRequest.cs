namespace moneygram_api.Models.CurrencyInfoRequest;

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
    [XmlElement(ElementName = "currencyInfoRequest", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public CurrencyInfoRequest CurrencyInfoRequest { get; set; }
}

public class CurrencyInfoRequest
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
}