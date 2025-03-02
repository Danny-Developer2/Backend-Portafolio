using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using API.Errors;  // Importar la clase ApiException

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);  // Pasar al siguiente middleware
            }
            catch (ApiException ex)  // Capturar la excepción personalizada
            {
                _logger.LogError($"Something went wrong: {ex.Message}");
                httpContext.Response.StatusCode = ex.StatusCode;
                await httpContext.Response.WriteAsJsonAsync(new
                {
                    message = ex.Message,
                    details = ex.Details
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error: {ex.Message}");
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await httpContext.Response.WriteAsJsonAsync(new
                {
                    message = "An unexpected error occurred."
                });
            }
        }
    }

    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
