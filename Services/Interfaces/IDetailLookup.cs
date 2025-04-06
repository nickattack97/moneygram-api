using moneygram_api.Models.DetailLookupResponse;
using moneygram_api.DTOs;

namespace moneygram_api.Services.Interfaces
{
    public interface IDetailLookup
    {
        Task<DetailLookupResponse> Lookup(string? referenceNumber, string? transactionSessionId);
    }
}