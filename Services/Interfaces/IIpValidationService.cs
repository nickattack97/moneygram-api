namespace moneygram_api.Services.Interfaces
{
    public interface IIpValidationService
    {
        bool IsIpWhitelisted(HttpContext context);
    }
}