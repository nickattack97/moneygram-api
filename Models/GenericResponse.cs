
namespace moneygram_api.Models
{
    public class Response
    {
        public bool Success { get; set; }
        public DateTime TimeStamp { get; set; }
        public long RecordId { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}