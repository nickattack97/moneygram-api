using moneygram_api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace moneygram_api.Services.Interfaces
{
    public interface ILoggingService
    {
        Task LogRequestAsync(RequestLog requestLog);
        Task<RequestLog> GetRequestLogByIdAsync(int id);
        Task UpdateRequestLogAsync(RequestLog requestLog);
        Task LogExceptionAsync(ExceptionLog exceptionLog);
        Task LogMoneyGramXmlAsync(MoneyGramXmlLog xmlLog);
        Task<MoneyGramXmlLog> GetMoneyGramXmlLogByIdAsync(int id);
        Task<List<MoneyGramXmlLog>> GetMoneyGramXmlLogsAsync(
            DateTime? startDate = null,
            DateTime? endDate = null,
            string operation = null);
        Task LogWebhookRequestAsync(WebhookLog webhookLog);
        Task<WebhookLog> GetWebhookLogByIdAsync(int id);
        Task UpdateWebhookLogAsync(WebhookLog webhookLog);
        Task<List<WebhookLog>> GetWebhookLogsAsync(
            DateTime? startDate = null,
            DateTime? endDate = null,
            string transactionStatus = null);
    }
}