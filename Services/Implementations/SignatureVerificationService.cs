using System;
using System.Security.Cryptography;
using System.Text;
using moneygram_api.Services.Interfaces;
using Microsoft.Extensions.Logging;
using moneygram_api.Settings;

namespace moneygram_api.Services.Implementations
{
    public class SignatureVerificationService : ISignatureVerificationService
    {
        private readonly IConfigurations _configurations;
        private readonly ILogger<SignatureVerificationService> _logger;
        private readonly Lazy<RSA> _cachedRsa;

        public SignatureVerificationService(IConfigurations configurations, ILogger<SignatureVerificationService> logger)
        {
            _configurations = configurations;
            _logger = logger;
            _cachedRsa = new Lazy<RSA>(() =>
            {
                var rsa = RSA.Create();
                var publicKeyPem = $"-----BEGIN PUBLIC KEY-----\n{_configurations.PublicKey}\n-----END PUBLIC KEY-----";
                rsa.ImportFromPem(publicKeyPem);
                return rsa;
            });
        }

        public bool Verify(string signatureHeader, long headerTimestamp, string destinationHost, string body)
        {
            try
            {
                _logger.LogInformation($"Starting signature verification process for host: {destinationHost}");

                // Step 1: Parse the signature header
                var (parsedTimestamp, signature) = ParseSignatureHeader(signatureHeader);
                _logger.LogDebug($"Parsed header - Timestamp: {parsedTimestamp}, Signature length: {signature?.Length ?? 0}");

                if (parsedTimestamp != headerTimestamp || string.IsNullOrEmpty(signature))
                {
                    _logger.LogWarning("Invalid signature header format or timestamp mismatch");
                    return false;
                }

                // Step 2: Prepare the payload string
                var payloadToVerify = $"{headerTimestamp}.{destinationHost}.{body}";
                _logger.LogDebug($"Full payload to verify: {payloadToVerify}");

                // Step 3: Convert payload to bytes
                var payloadBytes = Encoding.UTF8.GetBytes(payloadToVerify);
                _logger.LogDebug($"Payload bytes length: {payloadBytes.Length}");

                // Step 4: Decode the base64 signature
                var signatureBytes = Convert.FromBase64String(signature);
                _logger.LogDebug($"Signature bytes length: {signatureBytes.Length}");

                // Step 5: Verify the signature using RSA
                var isValid = _cachedRsa.Value.VerifyData(
                    payloadBytes,
                    signatureBytes,
                    HashAlgorithmName.SHA256,
                    RSASignaturePadding.Pkcs1);

                _logger.LogInformation($"Signature verification result: {isValid}");
                return isValid;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during signature verification");
                return false;
            }
        }

        public (long timestamp, string signature) ParseSignatureHeader(string header)
        {
            if (string.IsNullOrEmpty(header))
                return (0, null);

            var parts = header.Split(',', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2)
                return (0, null);

            long timestamp = 0;
            string signature = null;

            foreach (var part in parts)
            {
                var trimmedPart = part.Trim();
                if (trimmedPart.StartsWith("t="))
                    timestamp = long.TryParse(trimmedPart.Substring(2), out var ts) ? ts : 0;
                else if (trimmedPart.StartsWith("s="))
                    signature = trimmedPart.Substring(2);
            }

            return (timestamp, signature);
        }
    }
}