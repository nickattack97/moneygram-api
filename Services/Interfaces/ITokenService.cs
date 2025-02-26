using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace moneygram_api.Services.Interfaces
{
    public interface ITokenService
    {
        Task<IActionResult> ValidateTokenAsync(string token);
    }
}