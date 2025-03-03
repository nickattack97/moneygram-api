using System.ComponentModel.DataAnnotations;

namespace moneygram_api.DTOs
{
    public class LoginRequestDTO
    {
        private string _username;
        private string _password;

        public string Username
        {
            get => _username;
            set => _username = value?.Trim();
        }

        public string Password
        {
            get => _password;
            set => _password = value?.Trim();
        }

        public long SystemId { get; set; }
    }
    public class ChangeForgottenPasswordRequestDTO
    {
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "OTP is required.")]
        public string Otp { get; set; } = string.Empty;

        [Required(ErrorMessage = "New password is required.")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirm password is required.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    public class ForgotPasswordOtpResponseDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}