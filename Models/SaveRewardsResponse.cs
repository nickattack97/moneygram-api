
namespace moneygram_api.Models.SaveRewardsResponse
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
        [XmlElement(ElementName = "saveRewardsResponse", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public SaveRewardsResponse SaveRewardsResponse { get; set; }
    }

    public class SaveRewardsResponse
    {
        [XmlElement(ElementName = "doCheckIn", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public bool DoCheckIn { get; set; }

        [XmlElement(ElementName = "timeStamp", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public DateTime TimeStamp { get; set; }

        [XmlElement(ElementName = "flags", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public int Flags { get; set; }

        [XmlElement(ElementName = "freqCustCardNumber", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string FreqCustCardNumber { get; set; }
    }
}