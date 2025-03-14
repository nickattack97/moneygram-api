using System;
using System.Xml.Serialization;

namespace moneygram_api.Models.AmendTransactionResponse
{
    [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class Envelope : ResponsesBaseEnvelope
    {
        [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public Body Body { get; set; }
    }

    public class Body
    {
        [XmlElement(ElementName = "amendTransactionResponse", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public AmendTransactionResponse AmendTransactionResponse { get; set; }
    }

    public class AmendTransactionResponse
    {
        [XmlElement(ElementName = "doCheckIn", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public bool DoCheckIn { get; set; }

        [XmlElement(ElementName = "timeStamp", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public DateTime TimeStamp { get; set; }

        [XmlElement(ElementName = "flags", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public int Flags { get; set; }

        [XmlElement(ElementName = "transactionSucceeded", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public bool TransactionSucceeded { get; set; }
    }
}