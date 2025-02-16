using moneygram_api.DTOs;
using moneygram_api.Models.SaveRewardsResponse;
using System.Threading.Tasks;

namespace moneygram_api.Services.Interfaces
{
    public interface ISaveRewards
    {
        Task<SaveRewardsResponse> Save(SaveRewardsRequestDTO request);
    }
}