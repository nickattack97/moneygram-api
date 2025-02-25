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
        private const int MaxTimestampDifferenceMinutes = 65;
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

        public bool Verify(string signatureHeader, long currentTimestamp, string destinationHost, string body)
        {
            try
            {
                _logger.LogInformation($"Starting signature verification process for host: {destinationHost}");

                // Step 1: Parse the signature header
                var (headerTimestamp, signature) = ParseSignatureHeader(signatureHeader);
                _logger.LogDebug($"Parsed header - Timestamp: {headerTimestamp}, Signature length: {signature?.Length ?? 0}");

                if (headerTimestamp == 0 || string.IsNullOrEmpty(signature))
                {
                    _logger.LogWarning("Invalid signature header format");
                    return false;
                }

                // Step 2: Verify request freshness
                var timestampDifference = Math.Abs(currentTimestamp - headerTimestamp);
                var maxDifferenceSeconds = MaxTimestampDifferenceMinutes * 60;
                
                _logger.LogDebug($"Timestamp difference: {timestampDifference}s (max allowed: {maxDifferenceSeconds}s)");
                
                if (timestampDifference > maxDifferenceSeconds)
                {
                    _logger.LogWarning($"Request too old. Difference: {timestampDifference}s");
                    return false;
                }

                // Step 3: Prepare the payload string
                var payloadToVerify = $"{headerTimestamp}.{destinationHost}.{body}";
                _logger.LogDebug($"Full payload to verify: {payloadToVerify}");

                // Step 4: Convert payload to bytes
                var payloadBytes = Encoding.UTF8.GetBytes(payloadToVerify);
                _logger.LogDebug($"Payload bytes length: {payloadBytes.Length}");

                // Step 5: Decode the base64 signature
                var signatureBytes = Convert.FromBase64String(signature);
                _logger.LogDebug($"Signature bytes length: {signatureBytes.Length}");

                // Step 6: Verify the signature
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

        private (long timestamp, string signature) ParseSignatureHeader(string header)
        {
            if (string.IsNullOrEmpty(header))
                return (0, null);

            var parts = header.Split(',', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2)
                return (0, null);

            var timestamp = ParseTimestamp(parts[0]);
            var signature = ParseSignature(parts[1]);

            return (timestamp, signature);
        }

        private long ParseTimestamp(string timestampPart)
        {
            if (!timestampPart.StartsWith("t="))
                return 0;

            return long.TryParse(timestampPart.Substring(2), out var timestamp) ? timestamp : 0;
        }

        private string ParseSignature(string signaturePart)
        {
            if (!signaturePart.StartsWith("s="))
                return null;

            return signaturePart.Substring(2);
        }
    }
}