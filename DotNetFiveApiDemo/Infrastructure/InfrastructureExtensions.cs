using DotNetFiveApiDemo.Infrastructure.Data.Extensions;
using DotNetFiveApiDemo.Infrastructure.Email.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetFiveApiDemo.Infrastructure
{
    public static class InfrastructureExtensions
    {
        public static IServiceCollection AddInfrastructureConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddAppDbContext(configuration).AddRepositories();
            services.AddIdentityCore();
            services.AddEmailService(configuration);

            return services;
        }
    }
}