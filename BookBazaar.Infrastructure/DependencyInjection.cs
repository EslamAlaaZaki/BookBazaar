using BookBazaar.Application.Interfaces;
using BookBazaar.Application.Settings;
using BookBazaar.Infrastructure.Authentication;
using BookBazaar.Infrastructure.Data;
using BookBazaar.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BookBazaar.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BookBazaarDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

           
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
