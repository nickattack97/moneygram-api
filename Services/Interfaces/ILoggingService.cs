using moneygram_api.Models;
using System.Threading.Tasks;

namespace moneygram_api.Services.Interfaces
{
    public interface ILoggingService
    {
        Task LogRequestAsync(RequestLog requestLog);
        Task LogExceptionAsync(ExceptionLog exceptionLog);
        Task<RequestLog> GetRequestLogByIdAsync(int id);
        Task UpdateRequestLogAsync(RequestLog requestLog);
    }
}