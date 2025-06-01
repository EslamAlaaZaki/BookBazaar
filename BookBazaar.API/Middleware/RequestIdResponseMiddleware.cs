using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BookBazaar.API.Middleware
{
    public class RequestIdResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestIdResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.OnStarting(() =>
            {
                context.Response.Headers["X-Request-ID"] = context.TraceIdentifier;
                return Task.CompletedTask;
            });

            await _next(context);
        }
    }  
}