using System.Xml;
using System.Text.RegularExpressions;

namespace moneygram_api.Utilities
{
    public static class XmlUtility
    {
        public static string CleanXml(string xml)
        {
            try
            {
                // Remove encoding declaration
                if (xml.StartsWith("<?xml"))
                {
                    xml = Regex.Replace(xml, @"encoding\s*=\s*[""'][^""']*[""']", "");
                }
                
                // Validate XML structure
                var doc = new XmlDocument();
                doc.LoadXml(xml);
                return doc.OuterXml;
            }
            catch (XmlException)
            {
                return "<error>Invalid XML content</error>";
            }
        }
    }
}