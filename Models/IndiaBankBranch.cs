namespace moneygram_api.Models
{
    public class BankBranch
    {
        public string IFSCCode { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ContactInfo { get; set; }
        public string MICRCode { get; set; }
    }
}