namespace SucessPointCore.Api.Middlewares
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using SuccessPointCore.Application.Interfaces;
    using SucessPointCore.Domain.Entities;
    using System;
    using System.Threading.Tasks;

    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IErrorLogService _errorLogService;
        public ExceptionMiddleware(RequestDelegate next, IErrorLogService errorLogService)
        {
            _next = next;
            _errorLogService = errorLogService;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _errorLogService.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                // Optionally, you can customize the response based on the exception
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync($"An unexpected error occurred. \r\n{ex.Message}\r\n{ex.StackTrace}");
            }
        }
    }

    public static class ExceptionMiddlewareExtensions
    {
        public static void UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}