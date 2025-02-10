using System;
using System.Threading.Tasks;

namespace moneygram_api.Utilities
{
    public static class RetryHelper
    {
        public static async Task<T> RetryOnExceptionAsync<T>(int maxRetries, Func<Task<T>> operation)
        {
            int retryCount = 0;
            int delay = 1000; // Initial delay in milliseconds

            while (true)
            {
                try
                {
                    return await operation();
                }
                catch (Exception ex) when (retryCount < maxRetries)
                {
                    retryCount++;
                    await Task.Delay(delay);
                    delay *= 2; // Exponential backoff
                }
            }
        }
    }
}