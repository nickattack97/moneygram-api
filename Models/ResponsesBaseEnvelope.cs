namespace moneygram_api.Models
{
    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using moneygram_api.Utilities;

    public abstract class ResponsesBaseEnvelope
    {
        [XmlNamespaceDeclarations]
        public virtual XmlSerializerNamespaces Namespaces { get; set; } = new XmlSerializerNamespaces(new[]
        {
            new XmlQualifiedName("soapenv", "http://schemas.xmlsoap.org/soap/envelope/"),
            new XmlQualifiedName("xsi", "http://www.w3.org/2001/XMLSchema-instance"),
            new XmlQualifiedName("xsd", "http://www.w3.org/2001/XMLSchema")
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

        public static T Deserialize<T>(string xml) where T : ResponsesBaseEnvelope
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            using (var stringReader = new StringReader(xml))
            {
                return (T)xmlSerializer.Deserialize(stringReader);
            }
        }
    }
}