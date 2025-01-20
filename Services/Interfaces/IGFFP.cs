using moneygram_api.Models.GFFPRequest;
using moneygram_api.Models.GFFPResponse;
using System.Threading.Tasks;
using moneygram_api.DTOs;

namespace moneygram_api.Services.Interfaces
{
    public interface IGFFP
    {
        Task<GetFieldsForProductResponse> FetchFieldsForProduct(GFFPRequestDTO request);
    }
}