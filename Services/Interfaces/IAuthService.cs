using moneygram_api.DTOs;
using System.Threading.Tasks;
using static moneygram_api.Services.Implementations.AuthService;

namespace moneygram_api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<JWT> AuthenticateAsync(LoginRequestDTO loginRequest);
        Task<ForgotPasswordOtpResponseDTO> SendForgotPasswordOtpAsync(string username);
        Task<ForgotPasswordOtpResponseDTO> ResendForgotPasswordOtpAsync(string username);
        Task<bool> ChangePasswordAsync(string token, ChangePasswordRequestDTO request);
        Task<bool> ChangeForgottenPasswordAsync(ChangeForgottenPasswordRequestDTO request);
    }
}