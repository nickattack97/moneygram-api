using moneygram_api.DTOs;
using moneygram_api.Models.SendReversalResponse;

namespace moneygram_api.Services.Interfaces
{
    public interface ISendReversal
    {
        Task<SendReversalResponse> Reverse(SendReversalRequestDTO request);
    }
}