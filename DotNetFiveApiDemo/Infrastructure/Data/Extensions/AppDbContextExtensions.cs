using System;
using DotNetFiveApiDemo.Core.Common.Interfaces;
using DotNetFiveApiDemo.Core.Security.Entities;
using DotNetFiveApiDemo.Core.User.Entities;
using DotNetFiveApiDemo.Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetFiveApiDemo.Infrastructure.Data.Extensions
{
    public static class AppDbContextExtensions
    {
        public static IServiceCollection AddAppDbContext(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(opts =>
            {
                opts.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            });

            return services;
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRepository<RefreshToken<AppUser>>, RefreshTokenRepository>();
        }

        public static void AddIdentityCore(this IServiceCollection services)
        {
            services.AddIdentityCore<AppUser>(opts =>
                {
                    opts.Password.RequireDigit = true;
                    opts.Password.RequireLowercase = true;
                    opts.Password.RequireNonAlphanumeric = true;
                    opts.Password.RequireUppercase = true;
                    opts.Password.RequiredLength = 12;

                    opts.User.RequireUniqueEmail = true;
                    opts.SignIn.RequireConfirmedEmail = false;

                    opts.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();
        }
    }
}