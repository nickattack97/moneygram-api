namespace moneygram_api.Models.CurrencyInfoResponse;

using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using moneygram_api.Utilities;

[XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
public class Envelope : ResponsesBaseEnvelope
{
    [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public Body Body { get; set; }
}

public class Body
{
    [XmlElement(ElementName = "currencyInfoResponse", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public CurrencyInfoResponse CurrencyInfoResponse { get; set; }
}

public class CurrencyInfoResponse
{
    [XmlElement(ElementName = "doCheckIn", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public bool DoCheckIn { get; set; }

    [XmlElement(ElementName = "timeStamp", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public DateTime TimeStamp { get; set; }

    [XmlElement(ElementName = "flags", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public int Flags { get; set; }

    [XmlElement(ElementName = "version", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string Version { get; set; }

    [XmlElement(ElementName = "currencyInfo", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public List<CurrencyInfo> CurrencyInfo { get; set; }
}

public class CurrencyInfo
{
    [XmlElement(ElementName = "currencyCode", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string CurrencyCode { get; set; }

    [XmlElement(ElementName = "currencyName", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string CurrencyName { get; set; }

    [XmlElement(ElementName = "currencyPrecision", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public int CurrencyPrecision { get; set; }
}