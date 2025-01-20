using moneygram_api.Models.CodeTableRequest;
using moneygram_api.DTOs;
using moneygram_api.Models.CodeTableResponse;
using System.Threading.Tasks;

namespace moneygram_api.Services.Interfaces
{
    public interface IFetchCodeTable
    {
        Task<CodeTableResponse> Fetch(CodeTableRequestDTO request);
    }
}