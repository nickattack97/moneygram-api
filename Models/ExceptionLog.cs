using System;

namespace moneygram_api.Models
{
    public class ExceptionLog
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string ExceptionMessage { get; set; }
        public string? InnerExceptionMessage { get; set; }
        public string StackTrace { get; set; }
        public string HttpMethod { get; set; }
        public string Url { get; set; }
        public string Origin { get; set; }
        public DateTime Timestamp { get; set; }
    }
}