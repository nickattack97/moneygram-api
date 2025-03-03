using moneygram_api.DTOs;
using moneygram_api.Exceptions;
using moneygram_api.Services.Interfaces;
using moneygram_api.Settings;
using moneygram_api.Utilities;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using RestSharp;
using System.Net;

namespace moneygram_api.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IConfigurations _configurations;

        public AuthService(IConfigurations configurations)
        {
            _configurations = configurations ?? throw new ArgumentNullException(nameof(configurations));
        }

        public async Task<string> AuthenticateAsync(LoginRequestDTO loginRequest)
        {
            // Validate all required fields
            if (loginRequest == null || 
                string.IsNullOrEmpty(loginRequest.Username) || 
                string.IsNullOrEmpty(loginRequest.Password) || 
                loginRequest.SystemId <= 0) 
            {
                string offendingField = loginRequest == null ? nameof(loginRequest) :
                                        string.IsNullOrEmpty(loginRequest.Username) ? "username" :
                                        string.IsNullOrEmpty(loginRequest.Password) ? "password" :
                                        "systemId";

                throw new BaseCustomException(
                    400,
                    $"Required field is missing or invalid: {offendingField}.",
                    offendingField,
                    DateTime.UtcNow
                );
            }

            var baseUrl = _configurations.AuthBaseUrl;
            var resource = _configurations.AuthResource;

            if (string.IsNullOrEmpty(baseUrl) || string.IsNullOrEmpty(resource))
            {
                throw new BaseCustomException(
                    500,
                    "Authentication configuration is missing.",
                    "configuration",
                    DateTime.UtcNow
                );
            }

            var options = new RestClientOptions(baseUrl)
            {
                MaxTimeout = 30000 // 30 seconds timeout; adjustable via config if needed
            };

            using var client = new RestClient(options);

            var request = new RestRequest(resource, Method.Post)
                .AddHeader("Content-Type", "application/json");

            var jsonBody = JsonSerializer.Serialize(loginRequest);
            request.AddStringBody(jsonBody, DataFormat.Json);

            try
            {
                var response = await client.ExecuteAsync(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        if (string.IsNullOrEmpty(response.Content))
                        {
                            throw new BaseCustomException(
                                500,
                                "Empty response received from authentication server.",
                                "responseContent",
                                DateTime.UtcNow
                            );
                        }

                        var jwt = JsonSerializer.Deserialize<JWT>(response.Content,
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                        if (jwt?.Token == null)
                        {
                            throw new BaseCustomException(
                                500,
                                "Token not found in the response.",
                                "jwtToken",
                                DateTime.UtcNow
                            );
                        }
                        return jwt.Token;

                    case HttpStatusCode.BadRequest:
                        string errorMessage = "Invalid login request.";
                        string offendingField = "requestBody";

                        if (!string.IsNullOrEmpty(response.Content))
                        {
                            try
                            {
                                var badRequestJwt = JsonSerializer.Deserialize<JWT>(response.Content,
                                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                                errorMessage = badRequestJwt?.Info ?? errorMessage;

                                // Handle specific "User account not found" error
                                if (errorMessage.Equals("User account not found", StringComparison.OrdinalIgnoreCase))
                                {
                                    var userNotFoundError = ErrorDictionary.GetErrorResponse(4001);
                                    throw new BaseCustomException(
                                        userNotFoundError.ErrorCode,
                                        userNotFoundError.ErrorMessage,
                                        userNotFoundError.OffendingField,
                                        DateTime.UtcNow
                                    );
                                }

                                // Handle specific "System not found" error
                                if (errorMessage.Equals("System not found", StringComparison.OrdinalIgnoreCase))
                                {
                                    var systemNotFoundError = ErrorDictionary.GetErrorResponse(4002);
                                    throw new BaseCustomException(
                                        systemNotFoundError.ErrorCode,
                                        systemNotFoundError.ErrorMessage,
                                        systemNotFoundError.OffendingField,
                                        DateTime.UtcNow
                                    );
                                }

                                // Infer offending field from other error messages
                                if (!string.IsNullOrEmpty(badRequestJwt?.Info))
                                {
                                    if (badRequestJwt.Info.Contains("username", StringComparison.OrdinalIgnoreCase))
                                        offendingField = "username";
                                    else if (badRequestJwt.Info.Contains("password", StringComparison.OrdinalIgnoreCase))
                                        offendingField = "password";
                                    else if (badRequestJwt.Info.Contains("system", StringComparison.OrdinalIgnoreCase))
                                        offendingField = "systemId"; // Updated to match DTO field name
                                }
                            }
                            catch (JsonException)
                            {
                                errorMessage = response.Content; // Use raw content if deserialization fails
                            }
                        }

                        var badRequestError = ErrorDictionary.GetErrorResponse(400, errorMessage, offendingField);
                        throw new BaseCustomException(
                            badRequestError.ErrorCode,
                            badRequestError.ErrorMessage,
                            badRequestError.OffendingField,
                            DateTime.UtcNow
                        );

                    case HttpStatusCode.Unauthorized:
                        var unauthorizedError = ErrorDictionary.GetErrorResponse(401, "Invalid credentials provided.");
                        throw new BaseCustomException(
                            unauthorizedError.ErrorCode,
                            unauthorizedError.ErrorMessage,
                            "credentials",
                            DateTime.UtcNow
                        );

                    case HttpStatusCode.ServiceUnavailable:
                        var serviceUnavailableError = ErrorDictionary.GetErrorResponse(503);
                        throw new BaseCustomException(
                            serviceUnavailableError.ErrorCode,
                            serviceUnavailableError.ErrorMessage,
                            serviceUnavailableError.OffendingField,
                            DateTime.UtcNow
                        );

                    default:
                        var defaultError = ErrorDictionary.GetErrorResponse(
                            (int)response.StatusCode,
                            response.ErrorException?.Message ?? response.StatusDescription ?? "Unknown error"
                        );
                        throw new BaseCustomException(
                            defaultError.ErrorCode,
                            defaultError.ErrorMessage,
                            defaultError.OffendingField,
                            DateTime.UtcNow
                        );
                }
            }
            catch (JsonException ex)
            {
                throw new BaseCustomException(
                    500,
                    $"Failed to parse authentication response: {ex.Message}",
                    "responseContent",
                    DateTime.UtcNow
                );
            }
            catch (BaseCustomException)
            {
                throw; // Re-throw custom exceptions directly
            }
            catch (Exception ex)
            {
                throw new BaseCustomException(
                    500,
                    $"Unexpected error during authentication: {ex.Message}",
                    "unknown",
                    DateTime.UtcNow
                );
            }
        }
public async Task<ForgotPasswordOtpResponseDTO> SendForgotPasswordOtpAsync(string username)
    {
        if (string.IsNullOrEmpty(username))
        {
            throw new BaseCustomException(
                400,
                "Username is required.",
                "username",
                DateTime.UtcNow
            );
        }

        var baseUrl = _configurations.AuthBaseUrl;
        var resource = $"{_configurations.ForgotPasswordResource}/{username}";

        if (string.IsNullOrEmpty(baseUrl) || string.IsNullOrEmpty(resource))
        {
            throw new BaseCustomException(
                500,
                "Forgot password configuration is missing.",
                "configuration",
                DateTime.UtcNow
            );
        }

        var options = new RestClientOptions(baseUrl)
        {
            MaxTimeout = 30000
        };

        using var client = new RestClient(options);
        var request = new RestRequest(resource, Method.Get);

        try
        {
            var response = await client.ExecuteAsync(request);

            var responseData = string.IsNullOrEmpty(response.Content)
                ? new SSOResponse { info = "" }
                : JsonSerializer.Deserialize<SSOResponse>(response.Content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return new ForgotPasswordOtpResponseDTO
                    {
                        Success = true,
                        Message = responseData?.info ?? "OTP sent successfully."
                    };

                case HttpStatusCode.BadRequest:
                    string errorMessage = responseData?.info ?? "Invalid username provided.";
                    string offendingField = errorMessage.Contains("username", StringComparison.OrdinalIgnoreCase)
                        ? "username"
                        : "unknown";

                    if (!string.IsNullOrEmpty(responseData?.info))
                    {
                        if (responseData.info.Contains("user", StringComparison.OrdinalIgnoreCase))
                            offendingField = "username";
                        else if (responseData.info.Contains("password", StringComparison.OrdinalIgnoreCase))
                            offendingField = "password";
                        else if (responseData.info.Contains("system", StringComparison.OrdinalIgnoreCase))
                            offendingField = "systemId"; 
                    }

                    var errorResponse = ErrorDictionary.GetErrorResponse(400, errorMessage, offendingField);
                    throw new BaseCustomException(
                        errorResponse.ErrorCode,
                        errorResponse.ErrorMessage,
                        errorResponse.OffendingField,
                        DateTime.UtcNow
                    );

                default:
                    throw new BaseCustomException(
                        (int)response.StatusCode,
                        responseData?.info ?? response.StatusDescription ?? "Unknown error",
                        "unknown",
                        DateTime.UtcNow
                    );
            }
        }
        catch (JsonException ex)
        {
            throw new BaseCustomException(
                500,
                $"Failed to parse SSO response: {ex.Message}",
                "responseContent",
                DateTime.UtcNow
            );
        }
        catch (BaseCustomException)
        {
            throw; // Re-throw custom exceptions directly
        }
        catch (Exception ex)
        {
            throw new BaseCustomException(
                500,
                $"Unexpected error during OTP sending: {ex.Message}",
                "unknown",
                DateTime.UtcNow
            );
        }
    }

    public async Task<ForgotPasswordOtpResponseDTO> ResendForgotPasswordOtpAsync(string username)
    {
        if (string.IsNullOrEmpty(username))
        {
            throw new BaseCustomException(
                400,
                "Username is required.",
                "username",
                DateTime.UtcNow
            );
        }

        var baseUrl = _configurations.AuthBaseUrl;
        var resource = $"{_configurations.ResendForgotPasswordOtpResource}/{username}";

        if (string.IsNullOrEmpty(baseUrl) || string.IsNullOrEmpty(resource))
        {
            throw new BaseCustomException(
                500,
                "Resend OTP configuration is missing.",
                "configuration",
                DateTime.UtcNow
            );
        }

        var options = new RestClientOptions(baseUrl)
        {
            MaxTimeout = 30000
        };

        using var client = new RestClient(options);
        var request = new RestRequest(resource, Method.Get);

        try
        {
            var response = await client.ExecuteAsync(request);

            var responseData = string.IsNullOrEmpty(response.Content)
                ? new SSOResponse { info = "" }
                : JsonSerializer.Deserialize<SSOResponse>(response.Content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return new ForgotPasswordOtpResponseDTO
                    {
                        Success = true,
                        Message = responseData?.info ?? "OTP resent successfully."
                    };

                case HttpStatusCode.BadRequest:
                    string errorMessage = responseData?.info ?? "Invalid username provided.";
                    string offendingField = errorMessage.Contains("username", StringComparison.OrdinalIgnoreCase)
                        ? "username"
                        : "unknown";

                    if (!string.IsNullOrEmpty(responseData?.info))
                    {
                        if (responseData.info.Contains("user", StringComparison.OrdinalIgnoreCase))
                            offendingField = "username";
                        else if (responseData.info.Contains("password", StringComparison.OrdinalIgnoreCase))
                            offendingField = "password";
                        else if (responseData.info.Contains("system", StringComparison.OrdinalIgnoreCase))
                            offendingField = "systemId"; 
                    }

                    var errorResponse = ErrorDictionary.GetErrorResponse(400, errorMessage, offendingField);
                    throw new BaseCustomException(
                        errorResponse.ErrorCode,
                        errorResponse.ErrorMessage,
                        errorResponse.OffendingField,
                        DateTime.UtcNow
                    );

                default:
                    throw new BaseCustomException(
                        (int)response.StatusCode,
                        responseData?.info ?? response.StatusDescription ?? "Unknown error",
                        "unknown",
                        DateTime.UtcNow
                    );
            }
        }
        catch (JsonException ex)
        {
            throw new BaseCustomException(
                500,
                $"Failed to parse SSO response: {ex.Message}",
                "responseContent",
                DateTime.UtcNow
            );
        }
        catch (BaseCustomException)
        {
            throw; // Re-throw custom exceptions directly
        }
        catch (Exception ex)
        {
            throw new BaseCustomException(
                500,
                $"Unexpected error during OTP resending: {ex.Message}",
                "unknown",
                DateTime.UtcNow
            );
        }
    }

    public async Task<bool> ChangeForgottenPasswordAsync(ChangeForgottenPasswordRequestDTO request)
    {
        if (request == null ||
            string.IsNullOrEmpty(request.Username) ||
            string.IsNullOrEmpty(request.Otp) ||
            string.IsNullOrEmpty(request.NewPassword) ||
            string.IsNullOrEmpty(request.ConfirmPassword))
        {
            throw new BaseCustomException(
                400,
                "All fields are required.",
                "request",
                DateTime.UtcNow
            );
        }

        if (request.NewPassword != request.ConfirmPassword)
        {
            throw new BaseCustomException(
                400,
                "New password and confirm password do not match.",
                "confirmPassword",
                DateTime.UtcNow
            );
        }

        var baseUrl = _configurations.AuthBaseUrl;
        var resource = _configurations.ChangeForgottenPasswordResource;

        if (string.IsNullOrEmpty(baseUrl) || string.IsNullOrEmpty(resource))
        {
            throw new BaseCustomException(
                500,
                "Change password configuration is missing.",
                "configuration",
                DateTime.UtcNow
            );
        }

        var options = new RestClientOptions(baseUrl)
        {
            MaxTimeout = 30000
        };

        using var client = new RestClient(options);

        var requestBody = new
        {
            username = request.Username,
            otp = request.Otp,
            newPassword = request.NewPassword,
            confirmPassword = request.ConfirmPassword
        };

        var jsonBody = JsonSerializer.Serialize(requestBody);
        var restRequest = new RestRequest(resource, Method.Put)
            .AddHeader("Content-Type", "application/json")
            .AddStringBody(jsonBody, DataFormat.Json);

        try
        {
            var response = await client.ExecuteAsync(restRequest);

            var responseData = string.IsNullOrEmpty(response.Content)
                ? new SSOResponse { info = "" }
                : JsonSerializer.Deserialize<SSOResponse>(response.Content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return true;

                case HttpStatusCode.BadRequest:
                    string errorMessage = responseData?.info ?? "Invalid OTP or username provided.";
                    string offendingField = "unknown";
                    
                    if (errorMessage.Contains("password", StringComparison.OrdinalIgnoreCase))
                        offendingField = "newPassword";
                    else if (errorMessage.Contains("otp", StringComparison.OrdinalIgnoreCase))
                        offendingField = "otp";
                    else if (errorMessage.Contains("user", StringComparison.OrdinalIgnoreCase))
                        offendingField = "username";

                    var errorResponse = ErrorDictionary.GetErrorResponse(400, errorMessage, offendingField);
                    throw new BaseCustomException(
                        errorResponse.ErrorCode,
                        errorResponse.ErrorMessage,
                        errorResponse.OffendingField,
                        DateTime.UtcNow
                    );

                default:
                    throw new BaseCustomException(
                        (int)response.StatusCode,
                        responseData?.info ?? response.StatusDescription ?? "Unknown error",
                        "unknown",
                        DateTime.UtcNow
                    );
            }
        }
        catch (JsonException ex)
        {
            throw new BaseCustomException(
                500,
                $"Failed to parse SSO response: {ex.Message}",
                "responseContent",
                DateTime.UtcNow
            );
        }
        catch (BaseCustomException)
        {
            throw; // Re-throw custom exceptions directly
        }
        catch (Exception ex)
        {
            throw new BaseCustomException(
                500,
                $"Unexpected error during password change: {ex.Message}",
                "unknown",
                DateTime.UtcNow
            );
        }
    }

    private class SSOResponse
    {
        public string? info { get; set; }
    }

    public class JWT
    {
        public string? Token { get; set; }
        public string? Info { get; set; }
    }
    }
}