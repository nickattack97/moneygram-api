using moneygram_api.Models;
using moneygram_api.Models.ConsumerLookUpResponse;
using System.Threading.Tasks;

namespace moneygram_api.Services.Interfaces
{
    public interface ICustomerLookupService
    {
        Task<MoneyGramConsumerLookupResponse> GetCustomerByNationalIDAsync(string nationalID);
    }
}