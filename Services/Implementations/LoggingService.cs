using moneygram_api.Data;
using moneygram_api.Models;
using moneygram_api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace moneygram_api.Services.Implementations
{
    public class LoggingService : ILoggingService
    {
        private readonly AppDbContext _context;

        public LoggingService(AppDbContext context)
        {
            _context = context;
        }

        public async Task LogRequestAsync(RequestLog requestLog)
        {
            _context.RequestLogs.Add(requestLog);
            await _context.SaveChangesAsync();
        }

        public async Task<RequestLog> GetRequestLogByIdAsync(int id)
        {
            return await _context.RequestLogs.FindAsync(id);
        }

        public async Task UpdateRequestLogAsync(RequestLog requestLog)
        {
            _context.RequestLogs.Update(requestLog);
            await _context.SaveChangesAsync();
        }

        public async Task LogExceptionAsync(ExceptionLog exceptionLog)
        {
            _context.ExceptionLogs.Add(exceptionLog);
            await _context.SaveChangesAsync();
        }

        public async Task LogMoneyGramXmlAsync(MoneyGramXmlLog xmlLog)
        {
            _context.MoneyGramXmlLogs.Add(xmlLog);
            await _context.SaveChangesAsync();
        }

        public async Task<MoneyGramXmlLog> GetMoneyGramXmlLogByIdAsync(int id)
        {
            return await _context.MoneyGramXmlLogs.FindAsync(id);
        }

        public async Task<List<MoneyGramXmlLog>> GetMoneyGramXmlLogsAsync(
            DateTime? startDate = null,
            DateTime? endDate = null,
            string operation = null)
        {
            var query = _context.MoneyGramXmlLogs.AsQueryable();

            if (startDate.HasValue)
                query = query.Where(l => l.LogTime >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(l => l.LogTime <= endDate.Value);

            if (!string.IsNullOrEmpty(operation))
                query = query.Where(l => l.Operation == operation);

            return await query.OrderByDescending(l => l.LogTime).ToListAsync();
        }

        public async Task LogWebhookRequestAsync(WebhookLog webhookLog)
        {
            _context.WebhookLogs.Add(webhookLog);
            await _context.SaveChangesAsync();
        }

        public async Task<WebhookLog> GetWebhookLogByIdAsync(int id)
        {
            return await _context.WebhookLogs.FindAsync(id);
        }

        public async Task UpdateWebhookLogAsync(WebhookLog webhookLog)
        {
            _context.WebhookLogs.Update(webhookLog);
            await _context.SaveChangesAsync();
        }

        public async Task<List<WebhookLog>> GetWebhookLogsAsync(
            DateTime? startDate = null,
            DateTime? endDate = null,
            string transactionStatus = null)
        {
            var query = _context.WebhookLogs.AsQueryable();

            if (startDate.HasValue)
                query = query.Where(l => l.RequestTime >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(l => l.RequestTime <= endDate.Value);

            if (!string.IsNullOrEmpty(transactionStatus))
                query = query.Where(l => l.RequestBody.Contains($"\"transactionStatus\":\"{transactionStatus}\"", StringComparison.OrdinalIgnoreCase));

            return await query.OrderByDescending(l => l.RequestTime).ToListAsync();
        }
    }
}