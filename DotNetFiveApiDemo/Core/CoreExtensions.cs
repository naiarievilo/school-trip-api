using DotNetFiveApiDemo.Core.Security.Extensions;
using DotNetFiveApiDemo.Core.User.Entities;
using DotNetFiveApiDemo.Core.User.Interfaces;
using DotNetFiveApiDemo.Core.User.Services;
using DotNetFiveApiDemo.Core.User.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetFiveApiDemo.Core
{
    public static class CoreExtensions
    {
        public static IServiceCollection AddCoreConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSecurityServices(configuration);
            services.AddUserServices();

            return services;
        }

        private static void AddUserServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService<AppUser>, UserService>();
            services.AddHostedService<DeleteUnverifiedUsers>();
        }
    }
}