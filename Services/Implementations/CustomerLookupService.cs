using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using moneygram_api.Data;
using moneygram_api.Enums;
using moneygram_api.Utilities;
using moneygram_api.Models;
using moneygram_api.Models.ConsumerLookUpResponse;
using moneygram_api.Models.CountryInfoResponse;
using moneygram_api.Services.Interfaces;
using moneygram_api.Exceptions;
using moneygram_api.Settings;
using Microsoft.Extensions.Logging;

namespace moneygram_api.Services.Implementations
{
    public class CustomerLookupService : ICustomerLookupService
    {
        private readonly KycDbContext _kycContext;
        private readonly AppDbContext _appContext;
        private readonly IConfigurations _configurations;
        private readonly ILogger<CustomerLookupService> _logger;
        private readonly ILocalCountryInfoService _countryInfoService;
        private Dictionary<string, string> _countryNameToCodeMapping;

        public CustomerLookupService(
            KycDbContext kycContext, 
            AppDbContext appContext, 
            IConfigurations configurations, 
            ILogger<CustomerLookupService> logger,
            ILocalCountryInfoService countryInfoService)
        {
            _kycContext = kycContext;
            _appContext = appContext;
            _configurations = configurations ?? throw new ArgumentNullException(nameof(configurations));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _countryInfoService = countryInfoService ?? throw new ArgumentNullException(nameof(countryInfoService));
            _countryNameToCodeMapping = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        private string NormalizeNationalID(string nationalID)
        {
            if (string.IsNullOrWhiteSpace(nationalID))
                return string.Empty;

            return new string(nationalID
                .Where(char.IsLetterOrDigit)
                .Select(char.ToUpper)
                .ToArray());
        }

        private async Task InitializeCountryMappingAsync()
        {
            if (_countryNameToCodeMapping.Count == 0)
            {
                var countryInfoResponse = await _countryInfoService.GetCountryInfoAsync(null);
                
                foreach (var country in countryInfoResponse.CountryInfo)
                {
                    if (!string.IsNullOrEmpty(country.CountryName) && !string.IsNullOrEmpty(country.CountryCode))
                    {
                        _countryNameToCodeMapping[country.CountryName] = country.CountryCode;
                    }
                }
                
                _logger.LogInformation($"Initialized country mapping with {_countryNameToCodeMapping.Count} entries");
            }
        }

        private string NormalizeCountryName(string countryName)
        {
            if (string.IsNullOrWhiteSpace(countryName))
                return string.Empty;

            // If it's already a 3-letter code, assume it's correct
            if (countryName.Length == 3 && countryName.All(char.IsLetter))
                return countryName;

            // Try to find the country code from the mapping
            if (_countryNameToCodeMapping.TryGetValue(countryName, out var countryCode))
                return countryCode;

            // If we can't find it, log a warning and return the original value
            _logger.LogWarning($"Could not normalize country name: {countryName}");
            return countryName;
        }

        private class TransactionDto
        {
            public string? RewardsNumber { get; set; }
            public string? ReceiverFirstName { get; set; }
            public string? ReceiverMiddleName { get; set; }
            public string? ReceiverLastName { get; set; }
            public string? ReceiverLastName2 { get; set; }
            public string? ReceiverAddress1 { get; set; }
            public string? ReceiverAddress2 { get; set; }
            public string? ReceiverCity { get; set; }
            public string? ReceiverCountry { get; set; }
            public string? ReceiverPhoneNumber { get; set; }
            public string? ReceiverPhotoIDNumber { get; set; }
            public string? ReceiverPhotoIDType { get; set; }
            public string? SendCurrency { get; set; }
            public string? ReceiveCurrency { get; set; }
            public decimal? SendAmount { get; set; }
            public string? SenderLastName2 { get; set; }
            public string? SenderDOB { get; set; }
            public DateTime? ProcessDate { get; set; }
        }

        private class ClienteleDto
        {
            public long ID { get; set; }
            public string FirstName { get; set; }
            public string? MiddleName { get; set; }
            public string Surname { get; set; }
            public string Gender { get; set; }
            public string NationalID { get; set; }
            public string? Address { get; set; }
            public string? City { get; set; }
            public string? District { get; set; }
            public string? Suburb { get; set; }
            public byte[]? NatID_Image { get; set; } = Array.Empty<byte>();
            public string? Img_Format { get; set; } = string.Empty;
            public string? ContentType { get; set; } = string.Empty;
            public DateTime? AddDate { get; set; }
            public DateTime? ModifiedDate { get; set; }
            public string? Modification_Reason { get; set; }
            public string? ModifiedBy { get; set; }
        }

        public async Task<MoneyGramConsumerLookupResponse> GetCustomerByNationalIDAsync(string nationalID)
        {
            // Initialize the country mapping
            await InitializeCountryMappingAsync();
            
            var normalizedNationalID = NormalizeNationalID(nationalID);

            if (string.IsNullOrEmpty(normalizedNationalID))
            {
                var error = ErrorDictionary.GetErrorResponse(616, "nationalId");
                throw new SoapFaultException(error.ErrorCode, "Invalid or empty National ID provided.", error.OffendingField, DateTime.UtcNow);
            }

            var customer = await _kycContext.tblClientele
                .FromSqlRaw(_configurations.CustomerLookupQuery, normalizedNationalID)
                .Select(c => new ClienteleDto
                {
                    ID = c.ID,
                    FirstName = c.FirstName,
                    MiddleName = c.MiddleName,
                    Surname = c.Surname,
                    Gender = c.Gender,
                    NationalID = c.NationalID,
                    Address = c.Address,
                    City = c.City,
                    District = c.District,
                    Suburb = c.Suburb,
                    NatID_Image = c.NatID_Image ?? Array.Empty<byte>(),
                    Img_Format = c.Img_Format ?? string.Empty,
                    ContentType = c.ContentType ?? string.Empty,
                    AddDate = c.AddDate,
                    ModifiedDate = c.ModifiedDate,
                    Modification_Reason = c.Modification_Reason,
                    ModifiedBy = c.ModifiedBy
                })
                .FirstOrDefaultAsync();

            if (customer == null)
            {
                var error = ErrorDictionary.GetErrorResponse(616, "nationalId");
                throw new SoapFaultException(error.ErrorCode, error.ErrorMessage, error.OffendingField, DateTime.UtcNow);
            }

            var transactions = await _appContext.Database
                .SqlQueryRaw<TransactionDto>(_configurations.TransactionsLookupQuery, normalizedNationalID)
                .ToListAsync();

            // Normalize country names in transactions
            foreach (var transaction in transactions)
            {
                if (!string.IsNullOrEmpty(transaction.ReceiverCountry))
                {
                    transaction.ReceiverCountry = NormalizeCountryName(transaction.ReceiverCountry);
                }
            }

            var mostRecentTransactions = transactions
                .OrderByDescending(t => t.ProcessDate ?? DateTime.MinValue)
                .Take(10)
                .ToList();

            var senderInfo = new SenderInfo
            {
                SenderFirstName = customer.FirstName ?? string.Empty,
                SenderMiddleName = customer.MiddleName ?? string.Empty,
                SenderLastName = customer.Surname ?? string.Empty,
                SenderLastName2 = string.Empty,
                SenderGender = Enum.TryParse(customer.Gender?.Trim().ToUpper(), out Gender gender) ? gender.ToString() : string.Empty,
                SenderAddress = customer.Address ?? string.Empty,
                SenderAddress2 = customer.Suburb ?? string.Empty,
                SenderAddress3 = customer.District ?? string.Empty,
                SenderCity = customer.City ?? string.Empty,
                SenderCountry = NormalizeCountryName("ZWE"), // Normalize "ZWE"
                IdImage = customer.NatID_Image != null ? Convert.ToBase64String(customer.NatID_Image) : string.Empty,
                ImgFormat = customer.Img_Format ?? string.Empty,
                ContentType = customer.ContentType ?? string.Empty,
                SenderHomePhone = string.Empty,
                FreqCustCardNumber = string.Empty,
                ConsumerId = (int)customer.ID,
                SenderBirthCountry = NormalizeCountryName("ZWE") // Normalize "ZWE"
            };

            var senderDOBString = mostRecentTransactions.FirstOrDefault()?.SenderDOB;
            var rewardsNumber = mostRecentTransactions.FirstOrDefault()?.RewardsNumber;
            var senderLastName2 = mostRecentTransactions.FirstOrDefault()?.SenderLastName2;

            senderInfo.SenderLastName2 = senderLastName2;
            senderInfo.FreqCustCardNumber = rewardsNumber;

            if (DateTime.TryParse(senderDOBString, out var senderDOB))
            {
                senderInfo.SenderDOBObject = senderDOB;
            }
            else
            {
                senderInfo.SenderDOBObject = DateTime.MinValue;
            }

            // Deduplicate receivers based on key identifying fields
            var receiverInfos = mostRecentTransactions
                .GroupBy(t => new
                {
                    t.ReceiverFirstName,
                    t.ReceiverMiddleName,
                    t.ReceiverLastName,
                    t.ReceiverLastName2,
                    t.ReceiverCountry,
                    t.ReceiverPhoneNumber,
                    t.ReceiverPhotoIDNumber,
                    t.ReceiverPhotoIDType
                })
                .Select(g => g.OrderByDescending(t => t.ProcessDate ?? DateTime.MinValue).First()) // Take the most recent transaction
                .Select(t => new ReceiverInfo
                {
                    ReceiverFirstName = t.ReceiverFirstName ?? string.Empty,
                    ReceiverMiddleName = t.ReceiverMiddleName ?? string.Empty,
                    ReceiverLastName = t.ReceiverLastName ?? string.Empty,
                    ReceiverLastName2 = t.ReceiverLastName2 ?? string.Empty,
                    ReceiverAddress = t.ReceiverAddress1 ?? string.Empty,
                    ReceiverAddress2 = t.ReceiverAddress2 ?? string.Empty,
                    ReceiverCity = t.ReceiverCity ?? string.Empty,
                    ReceiverCountry = t.ReceiverCountry ?? string.Empty,
                    ReceiveCountry = t.ReceiverCountry ?? string.Empty,
                    ReceiverPhone = t.ReceiverPhoneNumber ?? string.Empty,
                    ReceiverPhotoIdNumber = t.ReceiverPhotoIDNumber ?? string.Empty,
                    ReceiverPhotoIdType = t.ReceiverPhotoIDType ?? string.Empty,
                    SendCurrency = t.SendCurrency ?? string.Empty,
                    ReceiveCurrency = t.ReceiveCurrency ?? string.Empty,
                    PayoutCurrency = t.ReceiveCurrency ?? string.Empty,
                    SendAmount = t.SendAmount ?? 0m
                })
                .ToList();

            // Pair each unique receiver with sender info
            var senderInfos = receiverInfos.Any()
                ? receiverInfos.Select(r => new SenderInfo
                {
                    SenderFirstName = senderInfo.SenderFirstName,
                    SenderMiddleName = senderInfo.SenderMiddleName,
                    SenderLastName = senderInfo.SenderLastName,
                    SenderLastName2 = senderInfo.SenderLastName2,
                    SenderGender = senderInfo.SenderGender,
                    SenderAddress = senderInfo.SenderAddress,
                    SenderAddress2 = senderInfo.SenderAddress2,
                    SenderAddress3 = senderInfo.SenderAddress3,
                    SenderCity = senderInfo.SenderCity,
                    SenderCountry = senderInfo.SenderCountry,
                    IdImage = senderInfo.IdImage,
                    ImgFormat = senderInfo.ImgFormat,
                    ContentType = senderInfo.ContentType,
                    SenderHomePhone = senderInfo.SenderHomePhone,
                    FreqCustCardNumber = senderInfo.FreqCustCardNumber,
                    ConsumerId = senderInfo.ConsumerId,
                    SenderBirthCountry = senderInfo.SenderBirthCountry,
                    SenderDOBObject = senderInfo.SenderDOBObject,
                    ReceiverInfo = r
                }).ToList()
                : new List<SenderInfo> { senderInfo }; // If no receivers, include sender info alone

            var response = new MoneyGramConsumerLookupResponse
            {
                TimeStamp = DateTime.UtcNow,
                Flags = 1,
                SenderInfo = senderInfos
            };

            _logger.LogInformation("Successfully retrieved customer data for National ID: {NationalID} with {Count} unique receivers", normalizedNationalID, receiverInfos.Count);
            return response;
        }
    }
}