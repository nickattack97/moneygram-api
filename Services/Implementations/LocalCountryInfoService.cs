using Microsoft.EntityFrameworkCore;
using moneygram_api.Data;
using moneygram_api.Models.CountryInfoResponse;
using moneygram_api.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace moneygram_api.Services.Implementations
{
    public class LocalCountryInfoService : ILocalCountryInfoService
    {
        private readonly AppDbContext _dbContext;

        public LocalCountryInfoService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CountryInfoResponse> GetCountryInfoAsync(string? countryCode)
        {
            var query = _dbContext.CountriesInfo.AsQueryable();
            if (!string.IsNullOrEmpty(countryCode))
            {
                query = query.Where(ci => ci.CountryCode == countryCode);
            }

            var countryData = await query.ToListAsync();
            var countryInfos = countryData.Select(ci => new CountryInfo
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
                PhoneLength = ParsePhoneLength(ci.PhoneLength) // Use helper method to parse safely
            }).ToList();

            return new CountryInfoResponse
            {
                DoCheckIn = false,
                TimeStamp = DateTime.Now,
                Flags = countryInfos.Count,
                Version = "1.0",
                CountryInfo = countryInfos
            };
        }

        private List<int> ParsePhoneLength(string phoneLength)
        {
            // Handle null, empty, or invalid phoneLength
            if (string.IsNullOrWhiteSpace(phoneLength))
            {
                return new List<int>(); // Return empty list if no valid data
            }

            try
            {
                return phoneLength.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                 .Select(s => int.Parse(s.Trim()))
                                 .ToList();
            }
            catch (FormatException)
            {
                // Log this if needed, but return empty list to avoid breaking the response
                return new List<int>();
            }
            catch (Exception)
            {
                // Catch any other unexpected errors
                return new List<int>();
            }
        }
    }
}