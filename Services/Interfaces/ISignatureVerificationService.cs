namespace moneygram_api.Services.Interfaces
{
    public interface ISignatureVerificationService
    {
        bool Verify(string signatureHeader, long unixTimeInSeconds, string destinationHost, string body);
    }
}