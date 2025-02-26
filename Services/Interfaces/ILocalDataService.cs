using moneygram_api.DTOs;
using moneygram_api.Models.CodeTableResponse;
using moneygram_api.Models.CountryInfoResponse;
using moneygram_api.Models.CurrencyInfoResponse;
using System.Threading.Tasks;

namespace moneygram_api.Services.Interfaces
{
    public interface ILocalCodeTableService
    {
        Task<CodeTableResponse> GetCodeTableAsync(CodeTableRequestDTO request);
        Task<CodeTableResponse> GetFilteredCodeTableAsync(FilteredCodeTableRequestDTO request);
    }

    public interface ILocalCountryInfoService
    {
        Task<CountryInfoResponse> GetCountryInfoAsync(string? countryCode);
    }

    public interface ILocalCurrencyInfoService
    {
        Task<CurrencyInfoResponse> GetCurrencyInfoAsync();
        Task<CurrencyInfoResponse> GetFilteredCurrencyInfoAsync(string currencyCode);
    }
}