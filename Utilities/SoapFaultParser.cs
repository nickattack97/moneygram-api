using System;
using System.Xml.Linq;

namespace moneygram_api.Utilities
{
    public static class SoapFaultParser
    {
        public static (int errorCode, string errorString, string offendingField, DateTime timeStamp) ParseSoapFault(string soapFaultContent)
        {
            var xmlDoc = XDocument.Parse(soapFaultContent);

            var faultCode = xmlDoc.Root
                                    ?.Element(XName.Get("Body", "http://schemas.xmlsoap.org/soap/envelope/"))
                                    ?.Element(XName.Get("Fault", "http://schemas.xmlsoap.org/soap/envelope/"))
                                    ?.Element(XName.Get("faultcode"))?.Value;

            var faultString = xmlDoc.Root
                                    ?.Element(XName.Get("Body", "http://schemas.xmlsoap.org/soap/envelope/"))
                                    ?.Element(XName.Get("Fault", "http://schemas.xmlsoap.org/soap/envelope/"))
                                    ?.Element(XName.Get("faultstring"))?.Value;

            var detailElement = xmlDoc.Root
                                        ?.Element(XName.Get("Body", "http://schemas.xmlsoap.org/soap/envelope/"))
                                        ?.Element(XName.Get("Fault", "http://schemas.xmlsoap.org/soap/envelope/"))
                                        ?.Element(XName.Get("detail"));

            var errorElement = detailElement?.Element(XName.Get("errors", "http://www.moneygram.com/AgentConnect1512"))
                                            ?.Element(XName.Get("error", "http://www.moneygram.com/AgentConnect1512"));

            var errorString = errorElement?.Element(XName.Get("errorString", "http://www.moneygram.com/AgentConnect1512"))?.Value;

            var offendingField = errorElement?.Element(XName.Get("offendingField", "http://www.moneygram.com/AgentConnect1512"))?.Value;

            var timeStampString = errorElement?.Element(XName.Get("timeStamp", "http://www.moneygram.com/AgentConnect1512"))?.Value;

            DateTime timeStamp = timeStampString != null ? DateTime.Parse(timeStampString) : DateTime.Now;

            // Handle the case where errorCode is not present
            int errorCode = 0;
            var errorCodeElement = errorElement?.Element(XName.Get("errorCode", "http://www.moneygram.com/AgentConnect1512"));
            if (errorCodeElement != null)
            {
                errorCode = int.Parse(errorCodeElement.Value);
            }

            // Handle the case where offendingField contains a namespace
            if (offendingField != null && offendingField.Contains("{"))
            {
                offendingField = offendingField.Substring(offendingField.IndexOf("}") + 1);
            }

            return (errorCode, errorString ?? "Unknown error", offendingField ?? "Unknown field", timeStamp);
        }
    }
}