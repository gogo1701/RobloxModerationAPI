using System.Security.Cryptography.X509Certificates;

namespace RobloxModerationAPI.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private const string API_KEY_HEADER = "X-Api-Key";
        private readonly string _configuredKey;

        public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuredKey = configuration["ApiKey"] ??
                             throw new ArgumentNullException("ApiKey is not configured in appsettings.json");
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(API_KEY_HEADER, out var incomingKey) ||
                incomingKey != _configuredKey)
            {
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("Unauthorized: Invalid API Key");
                return;
            }

            await _next(context);
        }
    }
}
