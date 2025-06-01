using BookBazaar.Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace BookBazaar.API.Middleware 
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";

                var statusCode = ex switch
                {
                    BookBazaarException => (int)HttpStatusCode.BadRequest,
                    UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                    KeyNotFoundException => (int)HttpStatusCode.NotFound,
                    _ => (int)HttpStatusCode.InternalServerError
                };

                if(statusCode == (int)HttpStatusCode.InternalServerError) 
                {
                    _logger.LogError(ex, "Unhandled exception occurred");
                }
                else 
                {
                    _logger.LogInformation($"[userError] {ex.Message}");
                }

                context.Response.StatusCode = statusCode;

                var result = JsonSerializer.Serialize(new
                {
                    error = ex.Message,
                    statusCode
                });


                await context.Response.WriteAsync(result);
            }
        }
    }

}

