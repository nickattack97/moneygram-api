using moneygram_api.Models;

namespace moneygram_api.Services.Interfaces
{
    public interface IBankBranchService
    {
        Task<List<BankBranch>> GetAllBranchesAsync();
        Task<BankBranch> GetBranchByIFSCAsync(string ifscCode);
    }
}