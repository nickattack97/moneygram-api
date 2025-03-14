namespace moneygram_api.Models.SendReversalRequest
{
    using System;
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class Envelope : RequestsBaseEnvelope<Body>
    {
        [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public Body Body { get; set; }
    }

    public class Body
    {
        [XmlElement(ElementName = "sendReversalRequest", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public SendReversalRequest SendReversalRequest { get; set; }
    }

    public class SendReversalRequest
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

        [XmlElement(ElementName = "sendAmount", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public decimal SendAmount { get; set; }

        [XmlElement(ElementName = "feeAmount", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public decimal FeeAmount { get; set; }

        [XmlElement(ElementName = "sendCurrency", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string SendCurrency { get; set; }

        [XmlElement(ElementName = "referenceNumber", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string ReferenceNumber { get; set; }

        [XmlElement(ElementName = "operatorName", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string OperatorName { get; set; }

        [XmlElement(ElementName = "reversalType", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string ReversalType { get; set; }

        [XmlElement(ElementName = "sendReversalReason", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string SendReversalReason { get; set; }

        [XmlElement(ElementName = "communicationRetryIndicator", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public bool CommunicationRetryIndicator { get; set; }
    }
}