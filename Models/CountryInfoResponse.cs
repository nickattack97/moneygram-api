namespace moneygram_api.Models.CountryInfoResponse
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class Envelope : ResponsesBaseEnvelope
    {
        [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public Body Body { get; set; }
    }

    public class Body
    {
        [XmlElement(ElementName = "countryInfoResponse", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public CountryInfoResponse CountryInfoResponse { get; set; }
    }

    public class CountryInfoResponse
    {
        [XmlElement(ElementName = "doCheckIn", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public bool DoCheckIn { get; set; }

        [XmlElement(ElementName = "timeStamp", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public DateTime TimeStamp { get; set; }

        [XmlElement(ElementName = "flags", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public int Flags { get; set; }

        [XmlElement(ElementName = "version", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string Version { get; set; }

        [XmlElement(ElementName = "countryInfo", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public List<CountryInfo> CountryInfo { get; set; }
    }

    public class CountryInfo
    {
        [XmlElement(ElementName = "countryCode", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string CountryCode { get; set; }

        [XmlElement(ElementName = "countryName", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string CountryName { get; set; }

        [XmlElement(ElementName = "countryLegacyCode", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string CountryLegacyCode { get; set; }

        [XmlElement(ElementName = "sendActive", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public bool SendActive { get; set; }

        [XmlElement(ElementName = "receiveActive", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public bool ReceiveActive { get; set; }

        [XmlElement(ElementName = "directedSendCountry", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public bool DirectedSendCountry { get; set; }

        [XmlElement(ElementName = "mgDirectedSendCountry", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public bool MgDirectedSendCountry { get; set; }

        [XmlElement(ElementName = "baseReceiveCurrency", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public string BaseReceiveCurrency { get; set; }

        [XmlElement(ElementName = "isZipCodeRequired", Namespace = "http://www.moneygram.com/AgentConnect1512")]
        public bool IsZipCodeRequired { get; set; }
        
        //Additional fields for data from countries_data.json
        public string Phone { get; set; }
        public string Emoji { get; set; }
        public string Image { get; set; }
        public List<int> PhoneLength { get; set; } = new();
    }
}