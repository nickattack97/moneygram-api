using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;

namespace moneygram_api.HealthChecks
{
    public static class CustomHealthCheckResponseWriter
    {
        public static async Task WriteResponse(HttpContext context, HealthReport report)
        {
            context.Response.ContentType = "application/json";

            var response = new
            {
                status = report.Status.ToString(),
                results = report.Entries.Select(entry => new
                {
                    component = entry.Key,
                    status = entry.Value.Status.ToString(),
                    description = entry.Value.Description,
                    data = entry.Value.Data
                })
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}