using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using moneygram_api.Data;
using moneygram_api.Services.Interfaces;
using moneygram_api.Settings;
using moneygram_api.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace moneygram_api.Services.Background
{
    public class DataSyncBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfigurations _configurations;
        private readonly ILogger<DataSyncBackgroundService> _logger;

        public DataSyncBackgroundService(
            IServiceProvider serviceProvider,
            IConfigurations configurations,
            ILogger<DataSyncBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _configurations = configurations;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Starting data sync...");
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                        var codeTableService = scope.ServiceProvider.GetRequiredService<IFetchCodeTable>();
                        var countryInfoService = scope.ServiceProvider.GetRequiredService<IGetCountryInfo>();
                        var currencyInfoService = scope.ServiceProvider.GetRequiredService<IFetchCurrencyInfo>();

                        // Fetch CurrencyInfo first to use for mapping currency names
                        var currencyInfoResponse = await currencyInfoService.Fetch();
                        var currencyVer = currencyInfoResponse.Version;

                        // Create a lookup dictionary for currency codes to names
                        var currencyLookup = currencyInfoResponse.CurrencyInfo
                            .ToDictionary(
                                ci => ci.CurrencyCode,
                                ci => ci.CurrencyName,
                                StringComparer.OrdinalIgnoreCase
                            );

                        // Fetch and update CodeTable
                        var codeTableResponse = await codeTableService.Fetch(new DTOs.CodeTableRequestDTO { AgentAllowedOnly = false });
                        var codeVer = codeTableResponse.Version;
                        var codeTables = codeTableResponse.CountryCurrencyInfo.Select(cci => new CodeTable
                        {
                            CountryCode = cci.CountryCode,
                            BaseCurrency = cci.BaseCurrency,
                            BaseCurrencyName = currencyLookup.TryGetValue(cci.BaseCurrency ?? "", out var baseName) ? baseName : null,
                            LocalCurrency = cci.LocalCurrency,
                            LocalCurrencyName = currencyLookup.TryGetValue(cci.LocalCurrency ?? "", out var localName) ? localName : null,
                            ReceiveCurrency = cci.ReceiveCurrency,
                            ReceiveCurrencyName = currencyLookup.TryGetValue(cci.ReceiveCurrency ?? "", out var receiveName) ? receiveName : null,
                            IndicativeRateAvailable = cci.IndicativeRateAvailable,
                            DeliveryOption = cci.DeliveryOption,
                            AgentManaged = cci.AgentManaged,
                            MgManaged = cci.MgManaged,
                            ReceiveAgentAbbreviation = cci.ReceiveAgentAbbreviation,
                            ReceiveAgentID = cci.ReceiveAgentID,
                            Version = codeVer,
                            LastUpdated = DateTime.Now
                        }).ToList();

                        dbContext.CodeTables.RemoveRange(dbContext.CodeTables);
                        dbContext.CodeTables.AddRange(codeTables);

                        // Fetch and update CountryInfo
                        var countryInfoResponse = await countryInfoService.Fetch(null);
                        var countriesVer = countryInfoResponse.Version;
                        var countriesInfo = countryInfoResponse.CountryInfo.Select(ci => new CountryInfoEntity
                        {
                            CountryCode = ci.CountryCode,
                            CountryName = ci.CountryName,
                            CountryLegacyCode = ci.CountryLegacyCode,
                            SendActive = ci.SendActive,
                            ReceiveActive = ci.ReceiveActive,
                            DirectedSendCountry = ci.DirectedSendCountry,
                            MgDirectedSendCountry = ci.MgDirectedSendCountry,
                            BaseReceiveCurrency = ci.BaseReceiveCurrency,
                            IsZipCodeRequired = ci.IsZipCodeRequired,
                            Phone = ci.Phone,
                            Emoji = ci.Emoji,
                            Image = ci.Image,
                            PhoneLength = string.Join(",", ci.PhoneLength),
                            Version = countriesVer,
                            LastUpdated = DateTime.Now
                        }).ToList();

                        dbContext.CountriesInfo.RemoveRange(dbContext.CountriesInfo);
                        dbContext.CountriesInfo.AddRange(countriesInfo);

                        // Fetch and update CurrencyInfo
                        var currencyInfo = currencyInfoResponse.CurrencyInfo.Select(ci => new CurrencyInfoEntity
                        {
                            CurrencyCode = ci.CurrencyCode,
                            CurrencyName = ci.CurrencyName,
                            CurrencyPrecision = ci.CurrencyPrecision,
                            Version = currencyVer,
                            LastUpdated = DateTime.Now
                        }).ToList();

                        dbContext.CurrencyInfo.RemoveRange(dbContext.CurrencyInfo);
                        dbContext.CurrencyInfo.AddRange(currencyInfo);

                        await dbContext.SaveChangesAsync();
                        _logger.LogInformation("Data sync completed successfully.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during data sync.");
                }

                // Wait for the configured frequency (default 14 days)
                await Task.Delay(TimeSpan.FromDays(_configurations.UpdateFrequencyInDays), stoppingToken);
            }
        }
    }
}