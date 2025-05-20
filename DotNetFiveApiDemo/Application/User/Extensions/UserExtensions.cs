using DotNetFiveApiDemo.Application.User.Identity;
using DotNetFiveApiDemo.Application.User.Interfaces;
using DotNetFiveApiDemo.Application.User.Services;
using DotNetFiveApiDemo.Domain.User.Interfaces;
using DotNetFiveApiDemo.Domain.User.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetFiveApiDemo.Application.User.Extensions
{
    public static class UserExtensions
    {
        public static IServiceCollection AddUserService(this IServiceCollection services)
        {
            services.AddScoped<IUserApplicationService, UserApplicationService>();
            services.AddScoped<IUserService<ApplicationUser>, UserService>();
            return services;
        }
    }
}