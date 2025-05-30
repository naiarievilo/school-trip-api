using DotNetFiveApiDemo.Application.User.Interfaces;
using DotNetFiveApiDemo.Application.User.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetFiveApiDemo.Application
{
    public static class ApplicationExtensions
    {
        public static IServiceCollection AddApplicationConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddScoped<IUserApplicationService, UserApplicationService>();
            services.AddAutoMapper(typeof(Startup));

            return services;
        }
    }
}