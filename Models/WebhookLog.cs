using System;

namespace moneygram_api.Models
{
    public class WebhookLog
    {
        public int Id { get; set; }
        public string HttpMethod { get; set; }
        public string Url { get; set; }
        public string Headers { get; set; }
        public string RequestBody { get; set; }
        public string ResponseBody { get; set; } = string.Empty;
        public int StatusCode { get; set; }
        public string Device { get; set; }
        public string Origin { get; set; }
        public DateTime RequestTime { get; set; }
        public DateTime? ResponseTime { get; set; } 
    }
}