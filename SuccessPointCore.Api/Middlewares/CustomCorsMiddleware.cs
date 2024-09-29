using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace SuccessPointCore.Api.Middlewares
{
    public class CustomCorsMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomCorsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var ipAddress = context.Connection.RemoteIpAddress?.ToString();

            // Define CORS policy dynamically based on the IP address
            var allowedOrigins = ipAddress == "127.0.0.1" ? "http://localhost:4200" : "*";

            context.Response.Headers.Add("Access-Control-Allow-Origin", allowedOrigins);
            context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
            context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization");

            // Handle preflight requests
            if (context.Request.Method == HttpMethods.Options)
            {
                context.Response.StatusCode = StatusCodes.Status204NoContent;
                return;
            }

            await _next(context);
        }
    }
}
