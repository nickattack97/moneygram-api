namespace moneygram_api.Services.Interfaces
{
    public interface ISignatureVerificationService
    {
        (long timestamp, string signature) ParseSignatureHeader(string header);
        bool Verify(string signature, long headerTimestamp, string destinationHost, string body);
    }
}