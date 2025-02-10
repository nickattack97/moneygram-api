using moneygram_api.Models.CurrencyInfoRequest;
using moneygram_api.Models.CurrencyInfoResponse;
using System.Threading.Tasks;
using moneygram_api.DTOs;

namespace moneygram_api.Services.Interfaces
{
    public interface IFetchCurrencyInfo
    {
        Task<CurrencyInfoResponse> Fetch();
        Task<CurrencyInfoResponse> FetchByCurrencyCode(string currencyCode);
    }
}