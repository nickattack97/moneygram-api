using System.Collections.Generic;
using System.Threading.Tasks;
using moneygram_api.Models;
using moneygram_api.DTOs;

namespace moneygram_api.Services.Interfaces
{
    public interface IMGSendTransactionService
    {
        Task<List<SendTransaction>> GetSendTransactionsAsync();
        Task<List<SendTransaction>> GetTransactionsByUserAsync(string username = null);
        Task<List<SendTransaction>> GetMyTransactionsAsync();
        Task<SendTransaction> GetTransactionByReferenceNumberAsync(string referenceNumber);
        Task<List<string>> GetNationalIdsAsync(string search = null);
        Task<List<string>> GetMobileNumbersAsync(string search = null);
        Task<Response> LogTransactionAsync(MGSendTransactionDTO transaction);
        Task<bool> CheckPRIEligibilityAsync(string senderPhotoIdNumber, string receiverPhotoIdNumber, decimal amount);
    }
}