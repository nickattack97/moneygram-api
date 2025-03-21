namespace moneygram_api.Models.CommitTransactionResponse;

using System;
using System.Xml.Serialization;
using moneygram_api.Models;

[XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
public class Envelope : ResponsesBaseEnvelope
{
    [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public Body Body { get; set; }
}

public class Body
{
    [XmlElement(ElementName = "commitTransactionResponse", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public CommitTransactionResponse CommitTransactionResponse { get; set; }
}

public class CommitTransactionResponse
{
    [XmlElement(ElementName = "doCheckIn", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public bool DoCheckIn { get; set; }

    [XmlElement(ElementName = "timeStamp", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public DateTime TimeStamp { get; set; }

    [XmlElement(ElementName = "flags", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public int Flags { get; set; }

    [XmlElement(ElementName = "referenceNumber", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string ReferenceNumber { get; set; }
    
    [XmlElement(ElementName = "transactionStatus", Namespace = "http://www.moneygram.com/AgentConnect1512", IsNullable = true)]
    public string? TransactionStatus { get; set; }

    [XmlElement(ElementName = "expectedDateOfDelivery", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public DateTime ExpectedDateOfDelivery { get; set; }

    [XmlElement(ElementName = "transactionDateTime", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public DateTime TransactionDateTime { get; set; }
}