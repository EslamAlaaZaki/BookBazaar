using BookBazaar.Application.Interfaces;
using BookBazaar.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BookBazaar.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
