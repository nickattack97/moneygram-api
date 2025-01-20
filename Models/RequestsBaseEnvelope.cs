namespace moneygram_api.Models;

using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

public abstract class RequestsBaseEnvelope<TBody>
{
    [XmlElement(ElementName = "Header", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public Header Header { get; set; }

    [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public TBody Body { get; set; }

    [XmlNamespaceDeclarations]
    public XmlSerializerNamespaces Namespaces { get; set; } = new XmlSerializerNamespaces(new[]
    {
        new XmlQualifiedName("soapenv", "http://schemas.xmlsoap.org/soap/envelope/"),
        new XmlQualifiedName("agen", "http://www.moneygram.com/AgentConnect1512")
    });

    public override string ToString()
    {
        var xmlSerializer = new XmlSerializer(GetType());
        using (var stringWriter = new StringWriterWithEncoding(Encoding.UTF8))
        {
            xmlSerializer.Serialize(stringWriter, this, Namespaces);
            return stringWriter.ToString();
        }
    }
}

public class Header
{
    // Add Header fields if necessary
}

public class StringWriterWithEncoding : StringWriter
{
    private readonly Encoding encoding;

    public StringWriterWithEncoding(Encoding encoding)
    {
        this.encoding = encoding;
    }

    public override Encoding Encoding => encoding;
}