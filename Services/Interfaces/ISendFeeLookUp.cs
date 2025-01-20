using moneygram_api.Models.FeeLookUpRequest;
using moneygram_api.Models.FeeLookUpResponse;
using System.Threading.Tasks;
using moneygram_api.DTOs;

namespace moneygram_api.Services.Interfaces
{
    public interface IFeeLookUp
    {
        Task<FeeLookUpResponse> FetchFeeLookUp(FeeLookUpRequestDTO request);
    }
}