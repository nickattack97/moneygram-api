using moneygram_api.Models.ConsumerLookUpRequest;
using moneygram_api.Models.ConsumerLookUpResponse;
using moneygram_api.DTOs;

namespace moneygram_api.Services.Interfaces
{
    public interface ISendConsumerLookUp
    {
        Task<MoneyGramConsumerLookupResponse> Push(ConsumerLookUpRequestDTO request);
    }
}