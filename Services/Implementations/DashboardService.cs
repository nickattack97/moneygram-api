using moneygram_api.Data;
using moneygram_api.Models;
using moneygram_api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace moneygram_api.Services.Implementations
{
    public class DashboardService : IDashboardService
    {
        private readonly AppDbContext _context;

        public DashboardService(AppDbContext context)
        {
            _context = context;
        }

        // Fetch data for FundsSentOverview (LineChart)
        public async Task<List<FundsSentData>> GetFundsSentOverviewAsync()
        {
            return await _context.SendTransactions
                .GroupBy(t => t.CommitDate.Value.Month)
                .Select(g => new FundsSentData
                {
                    Month = g.Key.ToString(),
                    Amount = g.Sum(t => t.SendAmount ?? 0)
                })
                .ToListAsync();
        }

        // Fetch data for TransactionStatus (PieChart)
        public async Task<TransactionStatusData> GetTransactionStatusAsync()
        {
            var totalTransactions = await _context.SendTransactions.CountAsync();
            var successfulTransactions = await _context.SendTransactions.CountAsync(t => t.Successful == true);
            var failedTransactions = totalTransactions - successfulTransactions;

            return new TransactionStatusData
            {
                Successful = successfulTransactions,
                Failed = failedTransactions
            };
        }

        // Fetch data for TopRecipientCountries (BarChart)
        public async Task<List<TopRecipientCountryData>> GetTopRecipientCountriesAsync()
        {
            return await _context.SendTransactions
                .GroupBy(t => t.ReceiverCountry)
                .Select(g => new TopRecipientCountryData
                {
                    Country = g.Key,
                    Amount = g.Sum(t => t.SendAmount ?? 0)
                })
                .OrderByDescending(g => g.Amount)
                .Take(5) // Top 5 countries
                .ToListAsync();
        }

        // Fetch data for RecentTransactions (Table)
        public async Task<List<RecentTransactionData>> GetRecentTransactionsAsync()
        {
            return await _context.SendTransactions
                .OrderByDescending(t => t.CommitDate)
                .Take(10) // Last 10 transactions
                .Select(t => new RecentTransactionData
                {
                    Sender = t.Sender,
                    Receiver = t.Receiver,
                    Amount = t.SendAmount ?? 0,
                    Date = t.CommitDate ?? DateTime.MinValue
                })
                .ToListAsync();
        }
    }

}