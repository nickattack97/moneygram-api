namespace moneygram_api.Models.CodeTableResponse;

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
    [XmlElement(ElementName = "codeTableResponse", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public CodeTableResponse CodeTableResponse { get; set; }
}

public class CodeTableResponse
{
    [XmlElement(ElementName = "doCheckIn", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public bool DoCheckIn { get; set; }

    [XmlElement(ElementName = "timeStamp", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public DateTime TimeStamp { get; set; }

    [XmlElement(ElementName = "flags", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public int Flags { get; set; }

    [XmlElement(ElementName = "version", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string Version { get; set; }

    [XmlElement(ElementName = "stateProvinceInfo", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public List<StateProvinceInfo> StateProvinceInfo { get; set; }

    [XmlElement(ElementName = "countryCurrencyInfo", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public List<CountryCurrencyInfo> CountryCurrencyInfo { get; set; }

    [XmlElement(ElementName = "deliveryOptionInfo", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public List<DeliveryOptionInfo> DeliveryOptionInfo { get; set; }
}

public class StateProvinceInfo
{
    [XmlElement(ElementName = "countryCode", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string CountryCode { get; set; }

    [XmlElement(ElementName = "stateProvinceCode", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string StateProvinceCode { get; set; }

    [XmlElement(ElementName = "stateProvinceName", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string StateProvinceName { get; set; }
}

public class CountryCurrencyInfo
{
    [XmlElement(ElementName = "countryCode", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string CountryCode { get; set; }

    [XmlElement(ElementName = "baseCurrency", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string BaseCurrency { get; set; }
    [XmlElement(ElementName = "baseCurrencyName", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string? BaseCurrencyName { get; set; }

    [XmlElement(ElementName = "localCurrency", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string LocalCurrency { get; set; }
        
    [XmlElement(ElementName = "localCurrencyName", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string? LocalCurrencyName { get; set; }

    [XmlElement(ElementName = "receiveCurrency", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string ReceiveCurrency { get; set; }

    [XmlElement(ElementName = "receiveCurrencyName", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string ReceiveCurrencyName { get; set; }


    [XmlElement(ElementName = "indicativeRateAvailable", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public bool IndicativeRateAvailable { get; set; }

    [XmlElement(ElementName = "deliveryOption", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string DeliveryOption { get; set; }

    [XmlElement(ElementName = "receiveAgentID", Namespace = "http://www.moneygram.com/AgentConnect1512", IsNullable = true)]
    public string ReceiveAgentID { get; set; }

    [XmlElement(ElementName = "receiveAgentAbbreviation", Namespace = "http://www.moneygram.com/AgentConnect1512", IsNullable = true)]
    public string ReceiveAgentAbbreviation { get; set; }

    [XmlElement(ElementName = "mgManaged", Namespace = "http://www.moneygram.com/AgentConnect1512", IsNullable = true)]
    public string MgManaged { get; set; }

    [XmlElement(ElementName = "agentManaged", Namespace = "http://www.moneygram.com/AgentConnect1512", IsNullable = true)]
    public string AgentManaged { get; set; }
}

public class DeliveryOptionInfo
{
    [XmlElement(ElementName = "dssOption", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public bool DssOption { get; set; }

    [XmlElement(ElementName = "deliveryOptionID", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public int DeliveryOptionID { get; set; }

    [XmlElement(ElementName = "deliveryOption", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string DeliveryOption { get; set; }

    [XmlElement(ElementName = "deliveryOptionName", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string DeliveryOptionName { get; set; }
}