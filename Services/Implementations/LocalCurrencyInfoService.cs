using Microsoft.EntityFrameworkCore;
using moneygram_api.Data;
using moneygram_api.Models.CurrencyInfoResponse;
using moneygram_api.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace moneygram_api.Services.Implementations
{
    public class LocalCurrencyInfoService : ILocalCurrencyInfoService
    {
        private readonly AppDbContext _dbContext;

        public LocalCurrencyInfoService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CurrencyInfoResponse> GetCurrencyInfoAsync()
        {
            var currencyInfos = await _dbContext.CurrencyInfo
                .Select(ci => new CurrencyInfo
                {
                    CurrencyCode = ci.CurrencyCode,
                    CurrencyName = ci.CurrencyName,
                    CurrencyPrecision = ci.CurrencyPrecision
                }).ToListAsync();

            return new CurrencyInfoResponse
            {
                DoCheckIn = false,
                TimeStamp = DateTime.Now,
                Flags = currencyInfos.Count,
                Version = "1.0",
                CurrencyInfo = currencyInfos
            };
        }

        public async Task<CurrencyInfoResponse> GetFilteredCurrencyInfoAsync(string currencyCode)
        {
            var currencyInfos = await _dbContext.CurrencyInfo
                .Where(ci => ci.CurrencyCode == currencyCode)
                .Select(ci => new CurrencyInfo
                {
                    CurrencyCode = ci.CurrencyCode,
                    CurrencyName = ci.CurrencyName,
                    CurrencyPrecision = ci.CurrencyPrecision
                }).ToListAsync();

            return new CurrencyInfoResponse
            {
                DoCheckIn = false,
                TimeStamp = DateTime.Now,
                Flags = currencyInfos.Count,
                Version = "1.0",
                CurrencyInfo = currencyInfos
            };
        }
    }
}