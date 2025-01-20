namespace moneygram_api.Models.GFFPResponse;

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
    [XmlElement(ElementName = "getFieldsForProductResponse", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public GetFieldsForProductResponse GetFieldsForProductResponse { get; set; }
}

public class GetFieldsForProductResponse
{
    [XmlElement(ElementName = "doCheckIn", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public bool DoCheckIn { get; set; }

    [XmlElement(ElementName = "timeStamp", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public DateTime TimeStamp { get; set; }

    [XmlElement(ElementName = "flags", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public int Flags { get; set; }

    [XmlElement(ElementName = "fqdoInfo", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public FqdoInfo FqdoInfo { get; set; }

    [XmlElement(ElementName = "productFieldInfo", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public List<ProductFieldInfo> ProductFieldInfo { get; set; }
}

public class FqdoInfo
{
    [XmlElement(ElementName = "receiveCountry", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string ReceiveCountry { get; set; }

    [XmlElement(ElementName = "deliveryOption", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string DeliveryOption { get; set; }

    [XmlElement(ElementName = "receiveCurrency", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string ReceiveCurrency { get; set; }

    [XmlElement(ElementName = "deliveryOptionDisplayName", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string DeliveryOptionDisplayName { get; set; }
}

public class ProductFieldInfo
{
    [XmlElement(ElementName = "xmlTag", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string XmlTag { get; set; }

    [XmlElement(ElementName = "visibility", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string Visibility { get; set; }

    [XmlElement(ElementName = "fieldLabel", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string FieldLabel { get; set; }

    [XmlElement(ElementName = "displayOrder", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public int DisplayOrder { get; set; }

    [XmlElement(ElementName = "fieldCategory", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string FieldCategory { get; set; }

    [XmlElement(ElementName = "dynamic", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public bool Dynamic { get; set; }

    [XmlElement(ElementName = "fieldMax", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public long FieldMax { get; set; }

    [XmlElement(ElementName = "fieldMin", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public long FieldMin { get; set; }

    [XmlElement(ElementName = "dataType", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string DataType { get; set; }

    [XmlElement(ElementName = "enumerated", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public bool Enumerated { get; set; }

    [XmlElement(ElementName = "defaultValue", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string DefaultValue { get; set; }

    [XmlElement(ElementName = "arrayName", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string ArrayName { get; set; }

    [XmlElement(ElementName = "arrayLength", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public long ArrayLength { get; set; }

    [XmlElement(ElementName = "exampleFormat", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string ExampleFormat { get; set; }

    [XmlArray(ElementName = "enumeratedValues", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    [XmlArrayItem(ElementName = "enumeratedValueInfo", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public List<EnumeratedValueInfo> EnumeratedValues { get; set; }
}

public class EnumeratedValueInfo
{
    [XmlElement(ElementName = "value", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string Value { get; set; }

    [XmlElement(ElementName = "label", Namespace = "http://www.moneygram.com/AgentConnect1512")]
    public string Label { get; set; }
}