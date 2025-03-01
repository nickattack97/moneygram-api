using System.Collections.Generic;
using System.Threading.Tasks;

namespace moneygram_api.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<List<FundsSentData>> GetFundsSentOverviewAsync();
        Task<TransactionStatusData> GetTransactionStatusAsync();
        Task<List<TopRecipientCountryData>> GetTopRecipientCountriesAsync();
        Task<List<RecentTransactionData>> GetRecentTransactionsAsync();
    }

    // DTOs for the interface
    public class FundsSentData
    {
        public string Month { get; set; }
        public decimal Amount { get; set; }
    }

    public class TransactionStatusData
    {
        public int Successful { get; set; }
        public int Failed { get; set; }
    }

    public class TopRecipientCountryData
    {
        public string Country { get; set; }
        public decimal Amount { get; set; }
    }

    public class RecentTransactionData
    {
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }
}