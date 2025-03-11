using moneygram_api.Models;
using System.Threading.Tasks;
using moneygram_api.DTOs; // Add this line if MGSendTransactionDTO is in the DTOs namespace

namespace moneygram_api.Services.Interfaces
{
    public interface IMGSendTransactionService
    {
        Task<List<SendTransaction>> GetSendTransactionsAsync();
        Task<Response> LogTransactionAsync(MGSendTransactionDTO transaction);
        Task<List<SendTransaction>> GetTransactionsByUserAsync(string username = null);
        Task<List<SendTransaction>> GetMyTransactionsAsync();
    }
}