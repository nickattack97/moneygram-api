using Microsoft.EntityFrameworkCore;
using moneygram_api.Data;
using moneygram_api.DTOs;
using moneygram_api.Models.CodeTableResponse;
using moneygram_api.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace moneygram_api.Services.Implementations
{
    public class LocalCodeTableService : ILocalCodeTableService
    {
        private readonly AppDbContext _dbContext;

        public LocalCodeTableService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CodeTableResponse> GetCodeTableAsync(CodeTableRequestDTO request)
        {
            var stateProvinceInfo = await _dbContext.StatesProvincesInfo
            .OrderBy(sp => sp.StateProvinceName)
            .Select(sp => new StateProvinceInfo
            {
                CountryCode = sp.CountryCode,
                StateProvinceCode = sp.StateProvinceCode,
                StateProvinceName = sp.StateProvinceName
            })
            .ToListAsync();
            
            var codeTables = await _dbContext.CodeTables
                .Where(ct => !request.AgentAllowedOnly || ct.IndicativeRateAvailable)
                .Select(ct => new CountryCurrencyInfo
                {
                    CountryCode = ct.CountryCode,
                    BaseCurrency = ct.BaseCurrency,
                    BaseCurrencyName = ct.BaseCurrencyName,
                    LocalCurrency = ct.LocalCurrency,
                    LocalCurrencyName = ct.LocalCurrencyName,
                    ReceiveCurrency = ct.ReceiveCurrency,
                    ReceiveCurrencyName = ct.ReceiveCurrencyName,
                    IndicativeRateAvailable = ct.IndicativeRateAvailable,
                    DeliveryOption = ct.DeliveryOption,
                    ReceiveAgentID = ct.ReceiveAgentID,
                    ReceiveAgentAbbreviation = ct.ReceiveAgentAbbreviation,
                    MgManaged = ct.MgManaged,
                    AgentManaged = ct.AgentManaged
                }).ToListAsync();

            return new CodeTableResponse
            {
                DoCheckIn = false,
                TimeStamp = DateTime.Now,
                Flags = codeTables.Count,
                Version = "1.0",
                CountryCurrencyInfo = codeTables,
                StateProvinceInfo = stateProvinceInfo
            };
        }

        public async Task<CodeTableResponse> GetFilteredCodeTableAsync(FilteredCodeTableRequestDTO request)
        {
            var codeTables = await _dbContext.CodeTables
                .Where(ct => ct.CountryCode == request.CountryCode /*&& ct.DeliveryOption == request.DeliveryOption*/)
                .Select(ct => new CountryCurrencyInfo
                {
                    CountryCode = ct.CountryCode,
                    BaseCurrency = ct.BaseCurrency,
                    BaseCurrencyName = ct.BaseCurrencyName,
                    LocalCurrency = ct.LocalCurrency,
                    LocalCurrencyName = ct.LocalCurrencyName,
                    ReceiveCurrency = ct.ReceiveCurrency,
                    ReceiveCurrencyName = ct.ReceiveCurrencyName,
                    IndicativeRateAvailable = ct.IndicativeRateAvailable,
                    DeliveryOption = ct.DeliveryOption,
                    ReceiveAgentID = ct.ReceiveAgentID,
                    ReceiveAgentAbbreviation = ct.ReceiveAgentAbbreviation,
                    MgManaged = ct.MgManaged,
                    AgentManaged = ct.AgentManaged
                }).ToListAsync();

            var stateProvinceInfo = await _dbContext.StatesProvincesInfo
                    .Where(sp => sp.CountryCode == request.CountryCode)
                    .OrderBy(sp => sp.StateProvinceName)
                    .Select(sp => new StateProvinceInfo
                    {
                        CountryCode = sp.CountryCode,
                        StateProvinceCode = sp.StateProvinceCode,
                        StateProvinceName = sp.StateProvinceName
                    })
                    .ToListAsync();

            return new CodeTableResponse
            {
                DoCheckIn = false,
                TimeStamp = DateTime.Now,
                Flags = codeTables.Count,
                Version = "1.0",
                CountryCurrencyInfo = codeTables,
                StateProvinceInfo = stateProvinceInfo
            };
        }
    }
}