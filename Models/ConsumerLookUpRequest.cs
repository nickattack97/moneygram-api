namespace moneygram_api.Models.ConsumerLookUpRequest;

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
    [XmlElement(ElementName = "moneyGramConsumerLookupRequest", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public MoneyGramConsumerLookupRequest MoneyGramConsumerLookupRequest { get; set; }
}

public class MoneyGramConsumerLookupRequest
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

    [XmlElement(ElementName = "customerPhone", Namespace = "http://www.moneygram.com/AgentConnect1512", IsNullable = true)]
    public string CustomerPhone { get; set; }

    [XmlElement(ElementName = "maxSendersToReturn", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public int MaxSendersToReturn { get; set; }

    [XmlElement(ElementName = "maxReceiversToReturn", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public int MaxReceiversToReturn { get; set; }
}