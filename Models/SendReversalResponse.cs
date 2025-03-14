namespace moneygram_api.Models.SendReversalResponse
{
    using System;
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class Envelope : ResponsesBaseEnvelope
    {
        [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public Body Body { get; set; }
    }

    public class Body
    {
        [XmlElement(ElementName = "sendReversalResponse", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public SendReversalResponse SendReversalResponse { get; set; }
    }

    public class SendReversalResponse
    {
        [XmlElement(ElementName = "doCheckIn", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public bool DoCheckIn { get; set; }

        [XmlElement(ElementName = "timeStamp", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public DateTime TimeStamp { get; set; }

        [XmlElement(ElementName = "flags", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public int Flags { get; set; }

        [XmlElement(ElementName = "transactionDateTime", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public DateTime TransactionDateTime { get; set; }

        [XmlElement(ElementName = "refundTotalAmount", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public decimal RefundTotalAmount { get; set; }

        [XmlElement(ElementName = "refundFaceAmount", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public decimal RefundFaceAmount { get; set; }

        [XmlElement(ElementName = "refundFeeAmount", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public decimal RefundFeeAmount { get; set; }

        [XmlElement(ElementName = "reversalType", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string ReversalType { get; set; }

        [XmlElement(ElementName = "agentCheckAuthorizationNumber", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string AgentCheckAuthorizationNumber { get; set; }
    }
}