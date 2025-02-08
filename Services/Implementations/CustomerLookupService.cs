using moneygram_api.Data;
using moneygram_api.Models;
using moneygram_api.Models.ConsumerLookUpResponse;
using moneygram_api.Services.Interfaces;
using moneygram_api.Enums;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace moneygram_api.Services.Implementations
{
    public class CustomerLookupService : ICustomerLookupService
    {
        private readonly KycDbContext _context;

        public CustomerLookupService(KycDbContext context)
        {
            _context = context;
        }

        public async Task<MoneyGramConsumerLookupResponse> GetCustomerByNationalIDAsync(string nationalID)
        {
            var customer = await _context.tblClientele.FirstOrDefaultAsync(c => c.NationalID == nationalID);
            if (customer == null)
            {
                throw new Exception("Customer not found");
            }

            var response = new MoneyGramConsumerLookupResponse
            {
                TimeStamp = DateTime.UtcNow,
                Flags = 1,
                SenderInfo = new List<SenderInfo>
                {
                    new SenderInfo
                    {
                        SenderFirstName = customer.FirstName,
                        SenderMiddleName = customer.MiddleName,
                        SenderLastName = customer.Surname,
                        SenderGender = Enum.TryParse(customer.Gender.Trim().ToUpper(), out Gender gender) ? gender.ToString() : string.Empty,
                        SenderAddress = customer.Address,
                        SenderAddress2 = customer.Suburb,
                        SenderCity = customer.City,
                        SenderState = customer.District,
                        SenderCountry = "ZWE", 
                        ReceiverInfo = null
                    }
                },
            };

            return response;
        }
    }
}