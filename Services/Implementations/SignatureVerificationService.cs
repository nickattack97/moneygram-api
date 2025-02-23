using System;
using System.Security.Cryptography;
using System.Text;
using moneygram_api.Services.Interfaces;
using Microsoft.Extensions.Logging;
using moneygram_api.Settings;
using Microsoft.Extensions.Options;

namespace moneygram_api.Services.Implementations
{
    public class SignatureVerificationService : ISignatureVerificationService
    {
        private readonly IConfigurations _configurations;
        private readonly ILogger<SignatureVerificationService> _logger;
        private const int MaxTimestampDifferenceMinutes = 65;

        public SignatureVerificationService(IConfigurations configurations, ILogger<SignatureVerificationService> logger)
        {
            _configurations = configurations;
            _logger = logger;
        }

        public bool Verify(string signatureHeader, long currentTimestamp, string destinationHost, string body)
        {
            try
            {
                _logger.LogInformation("Starting signature verification process");

                // Step 1: Parse the signature header
                var (headerTimestamp, signature) = ParseSignatureHeader(signatureHeader);
                if (headerTimestamp == 0 || string.IsNullOrEmpty(signature))
                {
                    _logger.LogWarning("Invalid signature header format");
                    return false;
                }

                // Step 2: Verify request freshness (within 65 minutes)
                var timestampDifference = Math.Abs(currentTimestamp - headerTimestamp);
                var maxDifferenceSeconds = MaxTimestampDifferenceMinutes * 60;
                
                if (timestampDifference > maxDifferenceSeconds)
                {
                    _logger.LogWarning($"Request too old. Difference: {timestampDifference} seconds. Max allowed: {maxDifferenceSeconds} seconds");
                    return false;
                }

                // Step 3: Prepare the payload string to verify
                var payloadToVerify = $"{headerTimestamp}.{destinationHost}.{body}";
                _logger.LogDebug($"Payload to verify (truncated): {payloadToVerify.Substring(0, Math.Min(100, payloadToVerify.Length))}...");

                // Step 4: Convert payload to bytes
                var payloadBytes = Encoding.UTF8.GetBytes(payloadToVerify);

                // Step 5: Decode the base64 signature
                var signatureBytes = Convert.FromBase64String(signature);

                // Step 6: Initialize RSA with MoneyGram's public key
                using var rsa = RSA.Create();
                try
                {
                    rsa.ImportFromPem(_configurations.PublicKey);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to import public key");
                    return false;
                }

                // Step 7: Verify the signature
                var isValid = rsa.VerifyData(
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
            try
            {
                if (string.IsNullOrEmpty(header))
                {
                    _logger.LogWarning("Signature header is null or empty");
                    return (0, null);
                }

                // Split the header into parts
                var parts = header.Split(',', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 2)
                {
                    _logger.LogWarning("Invalid signature header format: expected 2 parts");
                    return (0, null);
                }

                // Parse timestamp
                var timestampPart = parts[0].Trim();
                if (!timestampPart.StartsWith("t="))
                {
                    _logger.LogWarning("Invalid timestamp format in header");
                    return (0, null);
                }
                var timestampStr = timestampPart.Substring(2);

                // Parse signature
                var signaturePart = parts[1].Trim();
                if (!signaturePart.StartsWith("s="))
                {
                    _logger.LogWarning("Invalid signature format in header");
                    return (0, null);
                }
                var signature = signaturePart.Substring(2);

                if (!long.TryParse(timestampStr, out var timestamp))
                {
                    _logger.LogWarning("Failed to parse timestamp value");
                    return (0, null);
                }

                return (timestamp, signature);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error parsing signature header");
                return (0, null);
            }
        }
    }
}