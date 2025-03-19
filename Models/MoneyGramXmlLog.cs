using System;

namespace moneygram_api.Models
{
    public class MoneyGramXmlLog
    {
        public long Id { get; set; }
        public string Operation { get; set; }
        public string RequestXml { get; set; }
        public string ResponseXml { get; set; }
        public string HttpMethod { get; set; }
        public string Url { get; set; }
        public string Username { get; set; }
        public DateTime LogTime { get; set; }

    }
}