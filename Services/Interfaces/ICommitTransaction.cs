using moneygram_api.Models.CommitTransactionRequest;
using moneygram_api.Models.CommitTransactionResponse;
using System.Threading.Tasks;
using moneygram_api.DTOs;

namespace moneygram_api.Services.Interfaces
{
    public interface ICommitTransaction
    {
        Task<CommitTransactionResponse> Commit(CommitRequestDTO request);
    }
}