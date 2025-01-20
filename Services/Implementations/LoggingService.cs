using moneygram_api.Models;
using moneygram_api.Services.Interfaces;
using moneygram_api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace moneygram_api.Services.Implementations
{
    public class LoggingService : ILoggingService
    {
        private readonly IServiceProvider _serviceProvider;

        public LoggingService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task LogRequestAsync(RequestLog requestLog)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                context.RequestLogs.Add(requestLog);
                await context.SaveChangesAsync();
            }
        }

        public async Task LogExceptionAsync(ExceptionLog exceptionLog)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                context.ExceptionLogs.Add(exceptionLog);
                await context.SaveChangesAsync();
            }
        }

        public async Task<RequestLog> GetRequestLogByIdAsync(int id)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                return await context.RequestLogs.FindAsync(id);
            }
        }

        public async Task UpdateRequestLogAsync(RequestLog requestLog)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                context.RequestLogs.Update(requestLog);
                await context.SaveChangesAsync();
            }
        }
    }
}