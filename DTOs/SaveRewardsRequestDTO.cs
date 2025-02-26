namespace moneygram_api.DTOs
{
    public class SaveRewardsRequestDTO
    {
        public string ConsumerFirstName { get; set; }
        public string ConsumerLastName { get; set; }
        public string ConsumerAddress { get; set; }
        public string ConsumerAddress2 { get; set; }
        public string ConsumerCity { get; set; }
        public string ConsumerCountry { get; set; }
        public string ConsumerHomePhone { get; set; }
        public DateTime ConsumerDOB { get; set; }
        public string Gender { get; set; }
    }
}