namespace moneygram_api.DTOs
{
    public class LoginRequestDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public long SystemId { get; set; }
    }
}