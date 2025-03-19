using moneygram_api.Models;
using moneygram_api.Services.Interfaces;
using moneygram_api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

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

        public async Task LogMoneyGramXmlAsync(MoneyGramXmlLog xmlLog)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                context.MoneyGramXmlLogs.Add(xmlLog);
                await context.SaveChangesAsync();
            }
        }

        public async Task<MoneyGramXmlLog> GetMoneyGramXmlLogByIdAsync(int id)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                return await context.MoneyGramXmlLogs.FindAsync(id);
            }
        }

        public async Task<List<MoneyGramXmlLog>> GetMoneyGramXmlLogsAsync(
            DateTime? startDate = null, 
            DateTime? endDate = null, 
            string operation = null)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var query = context.MoneyGramXmlLogs.AsQueryable();

                if (startDate.HasValue)
                    query = query.Where(l => l.LogTime >= startDate.Value);
                
                if (endDate.HasValue)
                    query = query.Where(l => l.LogTime <= endDate.Value);
                
                if (!string.IsNullOrEmpty(operation))
                    query = query.Where(l => l.Operation == operation);

                return await query.OrderByDescending(l => l.LogTime).ToListAsync();
            }
        }
    }
}