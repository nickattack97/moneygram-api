using System;
using System.Xml.Serialization;

namespace moneygram_api.Models.AmendTransactionRequest
{
    [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class Envelope : RequestsBaseEnvelope<Body>
    {
        [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public Body Body { get; set; }
    }

    public class Body
    {
        [XmlElement(ElementName = "amendTransactionRequest", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public AmendTransactionRequest AmendTransactionRequest { get; set; }
    }

    public class AmendTransactionRequest
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

        [XmlElement(ElementName = "referenceNumber", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string ReferenceNumber { get; set; }

        [XmlElement(ElementName = "operatorName", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string OperatorName { get; set; }

        [XmlElement(ElementName = "receiverFirstName", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string ReceiverFirstName { get; set; }

        [XmlElement(ElementName = "receiverMiddleName", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string? ReceiverMiddleName { get; set; }

        [XmlElement(ElementName = "receiverLastName", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string ReceiverLastName { get; set; }

        [XmlElement(ElementName = "receiverLastName2", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string? ReceiverLastName2 { get; set; }
    }
}