namespace moneygram_api.Models.SaveRewardsRequest
{
    using System;
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class Envelope : RequestsBaseEnvelope<Body>
    {
        [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public new Body Body { get; set; }
    }

    public class Body
    {
        [XmlElement(ElementName = "saveRewardsRequest", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public SaveRewardsRequest SaveRewardsRequest { get; set; }
    }

    public class SaveRewardsRequest
    {
        [XmlElement(ElementName = "agentID", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string AgentID { get; set; }

        [XmlElement(ElementName = "agentSequence", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public int AgentSequence { get; set; }

        [XmlElement(ElementName = "token", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string Token { get; set; }

        [XmlElement(ElementName = "language", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string Language { get; set; }

        [XmlElement(ElementName = "timeStamp", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public DateTime TimeStamp { get; set; }

        [XmlElement(ElementName = "apiVersion", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public int ApiVersion { get; set; }

        [XmlElement(ElementName = "clientSoftwareVersion", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public int ClientSoftwareVersion { get; set; }

        [XmlElement(ElementName = "channelType", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string ChannelType { get; set; }

        [XmlElement(ElementName = "consumerFirstName", Namespace = "http://www.moneygram.com/AgentConnect1512", IsNullable = true)]
        public string ConsumerFirstName { get; set; }

        [XmlElement(ElementName = "consumerLastName", Namespace = "http://www.moneygram.com/AgentConnect1512", IsNullable = true)]
        public string ConsumerLastName { get; set; }

        [XmlElement(ElementName = "consumerAddress", Namespace = "http://www.moneygram.com/AgentConnect1512", IsNullable = true)]
        public string ConsumerAddress { get; set; }

        [XmlElement(ElementName = "consumerAddress2", Namespace = "http://www.moneygram.com/AgentConnect1512", IsNullable = true)]
        public string ConsumerAddress2 { get; set; }

        [XmlElement(ElementName = "consumerCity", Namespace = "http://www.moneygram.com/AgentConnect1512", IsNullable = true)]
        public string ConsumerCity { get; set; }

        [XmlElement(ElementName = "consumerCountry", Namespace = "http://www.moneygram.com/AgentConnect1512", IsNullable = true)]
        public string ConsumerCountry { get; set; }

        [XmlElement(ElementName = "consumerHomePhone", Namespace = "http://www.moneygram.com/AgentConnect1512", IsNullable = true)]
        public string ConsumerHomePhone { get; set; }

        [XmlElement(ElementName = "consumerMobilePhone", Namespace = "http://www.moneygram.com/AgentConnect1512", IsNullable = true)]
        public string ConsumerMobilePhone { get; set; }

        [XmlIgnore]
        public DateTime ConsumerDOB { get; set; }

        [XmlElement(ElementName = "consumerDOB", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string SenderDOBString
        {
            get { return ConsumerDOB.ToString("yyyy-MM-dd"); }
            set { ConsumerDOB = DateTime.Parse(value); }
        }

        [XmlElement(ElementName = "gender", Namespace = "http://www.moneygram.com/AgentConnect1512", IsNullable = true)]
        public string Gender { get; set; }

        [XmlElement(ElementName = "marketingOptIn", Namespace = "http://www.moneygram.com/AgentConnect1512", IsNullable = true)]
        public bool? MarketingOptIn { get; set; }

        [XmlElement(ElementName = "marketingBySMS", Namespace = "http://www.moneygram.com/AgentConnect1512", IsNullable = true)]
        public bool? MarketingBySMS { get; set; }

        [XmlElement(ElementName = "marketingLanguage", Namespace = "http://www.moneygram.com/AgentConnect1512", IsNullable = true)]
        public string MarketingLanguage { get; set; }

        [XmlElement(ElementName = "consentOfPersonalInformationThirdParty", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public bool ConsentOfPersonalInformationThirdParty { get; set; }

        [XmlElement(ElementName = "agentAcknowledgement", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public bool AgentAcknowledgement { get; set; }
    }
}