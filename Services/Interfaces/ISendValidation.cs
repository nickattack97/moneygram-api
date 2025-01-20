using moneygram_api.Models.SendValidationRequest;
using moneygram_api.Models.SendValidationResponse;
using System.Threading.Tasks;
using moneygram_api.DTOs;

namespace moneygram_api.Services.Interfaces
{
    public interface ISendValidation
    {
        Task<SendValidationResponse> Push(SendValidationRequestDTO request);
    }
}