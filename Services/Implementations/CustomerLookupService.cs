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
using moneygram_api.Services.Interfaces;
using moneygram_api.Exceptions;
using moneygram_api.Settings;
using Microsoft.Extensions.Logging; // Add this for logging

namespace moneygram_api.Services.Implementations
{
    public class CustomerLookupService : ICustomerLookupService
    {
        private readonly KycDbContext _kycContext;
        private readonly AppDbContext _appContext;
        private readonly IConfigurations _configurations;
        private readonly ILogger<CustomerLookupService> _logger; // Add logger

        public CustomerLookupService(KycDbContext kycContext, AppDbContext appContext, IConfigurations configurations, ILogger<CustomerLookupService> logger)
        {
            _kycContext = kycContext;
            _appContext = appContext;
            _configurations = configurations ?? throw new ArgumentNullException(nameof(configurations));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // Normalize National ID locally for input validation
        private string NormalizeNationalID(string nationalID)
        {
            if (string.IsNullOrWhiteSpace(nationalID))
                return string.Empty;

            return new string(nationalID
                .Where(char.IsLetterOrDigit)
                .Select(char.ToUpper)
                .ToArray());
        }

        // DTO for SendTransactions query results with nullable properties
        private class TransactionDto
        {
            public string? ReceiverFirstName { get; set; }
            public string? ReceiverMiddleName { get; set; }
            public string? ReceiverLastName { get; set; }
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
            public string? SenderDOB { get; set; }
        }

        public class ClienteleDto
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
            // Normalize the input National ID
            var normalizedNationalID = NormalizeNationalID(nationalID);

            if (string.IsNullOrEmpty(normalizedNationalID))
            {
                var error = ErrorDictionary.GetErrorResponse(616, "nationalId");
                throw new SoapFaultException(error.ErrorCode, "Invalid or empty National ID provided.", error.OffendingField, DateTime.UtcNow);
            }

            // Fetch sender details from tblClientele
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

            // Fetch past receivers from SendTransactions
            var transactions = await _appContext.Database
                .SqlQueryRaw<TransactionDto>(_configurations.TransactionsLookupQuery, normalizedNationalID)
                .ToListAsync();

            // Map sender info with null-safe handling
            var senderInfo = new SenderInfo
            {
                SenderFirstName = customer.FirstName ?? string.Empty,
                SenderMiddleName = customer.MiddleName ?? string.Empty,
                SenderLastName = customer.Surname ?? string.Empty,
                SenderGender = Enum.TryParse(customer.Gender?.Trim().ToUpper(), out Gender gender) ? gender.ToString() : string.Empty,
                SenderAddress = customer.Address ?? string.Empty,
                SenderAddress2 = customer.Suburb ?? string.Empty,
                SenderCity = customer.City ?? string.Empty,
                SenderState = customer.District ?? string.Empty,
                SenderCountry = "ZWE",
                IdImage = customer.NatID_Image != null ? Convert.ToBase64String(customer.NatID_Image) : string.Empty,
                ImgFormat = customer.Img_Format ?? string.Empty,
                ContentType = customer.ContentType ?? string.Empty,
                ReceiverInfo = transactions.Any() ? new ReceiverInfo() : null
            };

            // Parse SenderDOB from the transaction
            var senderDOBString = transactions.FirstOrDefault()?.SenderDOB;
            if (DateTime.TryParse(senderDOBString, out var senderDOB))
            {
                senderInfo.SenderDOBObject = senderDOB;
            }
            else
            {
                senderInfo.SenderDOBObject = DateTime.MinValue; 
            }


            // Map receiver info for each unique receiver from DTO
            var receiverInfos = transactions.Select(t => new ReceiverInfo
            {
                ReceiverFirstName = t.ReceiverFirstName ?? string.Empty,
                ReceiverMiddleName = t.ReceiverMiddleName ?? string.Empty,
                ReceiverLastName = t.ReceiverLastName ?? string.Empty,
                ReceiverAddress = t.ReceiverAddress1 ?? string.Empty,
                ReceiverAddress2 = t.ReceiverAddress2 ?? string.Empty,
                ReceiverCity = t.ReceiverCity ?? string.Empty,
                ReceiverCountry = t.ReceiverCountry ?? string.Empty,
                ReceiverPhone = t.ReceiverPhoneNumber ?? string.Empty,
                ReceiverPhotoIdNumber = t.ReceiverPhotoIDNumber ?? string.Empty,
                ReceiverPhotoIdType = t.ReceiverPhotoIDType ?? string.Empty,
                SendCurrency = t.SendCurrency ?? string.Empty,
                ReceiveCurrency = t.ReceiveCurrency ?? string.Empty,
                PayoutCurrency = t.ReceiveCurrency ?? string.Empty,
                SendAmount = t.SendAmount ?? 0m
            }).ToList();

            // Attach the first receiver
            if (receiverInfos.Any())
            {
                senderInfo.ReceiverInfo = receiverInfos.First();
            }

            var response = new MoneyGramConsumerLookupResponse
            {
                TimeStamp = DateTime.UtcNow,
                Flags = 1,
                SenderInfo = new List<SenderInfo> { senderInfo }
            };

            return response;
        }
    }
}