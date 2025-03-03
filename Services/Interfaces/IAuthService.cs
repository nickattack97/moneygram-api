using moneygram_api.DTOs;
using System.Threading.Tasks;

namespace moneygram_api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> AuthenticateAsync(LoginRequestDTO loginRequest);
        Task<ForgotPasswordOtpResponseDTO> SendForgotPasswordOtpAsync(string username);
        Task<ForgotPasswordOtpResponseDTO> ResendForgotPasswordOtpAsync(string username);
        Task<bool> ChangeForgottenPasswordAsync(ChangeForgottenPasswordRequestDTO request);
    }
}