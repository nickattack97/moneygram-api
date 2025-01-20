using moneygram_api.DTOs;
using moneygram_api.Models.CountryInfoResponse;
using System.Threading.Tasks;

namespace moneygram_api.Services.Interfaces
{
    public interface IGetCountryInfo
    {
        Task<CountryInfoResponse> Fetch(string? countryCode = null);
    }
}