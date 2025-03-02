using System.Collections.Generic;
using System.Threading.Tasks;
using moneygram_api.DTOs;

namespace moneygram_api.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<List<FundsSentData>> GetFundsSentOverviewAsync();
        Task<TransactionStatusData> GetTransactionStatusAsync();
        Task<List<TopRecipientCountryData>> GetTopRecipientCountriesAsync();
        Task<List<RecentTransactionData>> GetRecentTransactionsAsync();
    }
}