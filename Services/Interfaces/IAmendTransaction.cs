using moneygram_api.Models.AmendTransactionResponse;
using moneygram_api.DTOs;
using System.Threading.Tasks;

namespace moneygram_api.Services.Interfaces
{
    public interface IAmendTransaction
    {
        Task<AmendTransactionResponse> Amend(AmendTransactionRequestDTO request);
    }
}