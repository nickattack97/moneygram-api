using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using moneygram_api.Data;
using moneygram_api.DTOs;
using moneygram_api.Services.Interfaces;

namespace moneygram_api.Services.Implementations
{
    public class DashboardService : IDashboardService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DashboardService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // Fetch data for FundsSentOverview (LineChart)
        public async Task<List<FundsSentData>> GetFundsSentOverviewAsync()
        {
            string operatorName = GetCurrentOperator();

            return await _context.SendTransactions
                .Where(t => t.Successful == true && t.TellerId == operatorName) // Filter for successful transactions by this operator
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
            string operatorName = GetCurrentOperator();

            var totalTransactions = await _context.SendTransactions
                .CountAsync(t => t.TellerId == operatorName);
                
            var successfulTransactions = await _context.SendTransactions
                .CountAsync(t => t.TellerId == operatorName && t.Successful == true);
                
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
            string operatorName = GetCurrentOperator();

            return await _context.SendTransactions
                .Where(t => t.Successful == true && t.TellerId == operatorName) // Filter for successful transactions by this operator
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
            string operatorName = GetCurrentOperator();

            return await _context.SendTransactions
                .Where(t => t.Successful == true && t.TellerId == operatorName) // Filter for successful transactions by this operator
                .OrderByDescending(t => t.CommitDate)
                .Take(10) // Last 10 transactions
                .Select(t => new RecentTransactionData
                {
                    Sender = t.Sender,
                    Receiver = t.Receiver,
                    Amount = t.SendAmount ?? 0,
                    Date = t.CommitDate ?? DateTime.MinValue,
                    Successful = t.Successful ?? false
                })
                .ToListAsync();
        }


        private string GetCurrentOperator()
        {
            string operatorName = _httpContextAccessor.HttpContext?.Items["Username"]?.ToString();
            if (string.IsNullOrEmpty(operatorName))
            {
                throw new UnauthorizedAccessException("Username not found in token. Re-authenticate.");
            }
            return operatorName;
        }
    }
}