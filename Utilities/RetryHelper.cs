using System;
using System.Threading.Tasks;

namespace moneygram_api.Utilities
{
    public static class RetryHelper
    {
        public static async Task<T> RetryOnExceptionAsync<T>(int maxRetries, Func<Task<T>> operation, int initialDelay = 1000, int maxDelay = 15000)
        {
            int retryCount = 0;
            int delay = initialDelay; // Initial delay in milliseconds

            while (true)
            {
                try
                {
                    return await operation();
                }
                catch (Exception) when (retryCount < maxRetries)
                {
                    retryCount++;
                    await Task.Delay(delay);
                    delay = Math.Min(delay * 2, maxDelay); // Exponential backoff with a maximum delay
                }
            }
        }
    }
}