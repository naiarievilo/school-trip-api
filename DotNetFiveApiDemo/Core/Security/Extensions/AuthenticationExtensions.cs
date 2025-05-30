using System;
using System.Text;
using DotNetFiveApiDemo.Core.Security.Interfaces;
using DotNetFiveApiDemo.Core.Security.Services;
using DotNetFiveApiDemo.Core.Security.Settings;
using DotNetFiveApiDemo.Core.Security.Tasks;
using DotNetFiveApiDemo.Core.User.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace DotNetFiveApiDemo.Core.Security.Extensions
{
    internal static class AuthenticationExtensions
    {
        internal static IServiceCollection AddSecurityServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSettings(configuration);
            services.AddAuthenticationServices(configuration);
            services.AddSecurityTasks();

            return services;
        }

        private static void AddSecurityTasks(this IServiceCollection services)
        {
            services.AddHostedService<DeleteExpiredRefreshTokens>();
        }

        private static void AddSettings(this IServiceCollection services,
            IConfiguration configuration)
        {
            // SPA settings
            var clientSection = configuration.GetSection("Client");
            services.Configure<ClientSettings>(clientSection);
        }

        private static void AddAuthenticationServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            // Authentication Services
            services.AddScoped<IJwtTokenProvider, JwtTokenProvider>();
            services.AddScoped<IAuthenticationService<AppUser>, AuthenticationService>();

            // Avoid creating duplicate code in IJwtTokenProvider implementation and authentication settings
            var jwtSettingsSection = configuration.GetSection("Jwt");
            services.Configure<JwtSettings>(jwtSettingsSection);
            var jwtSettings = jwtSettingsSection.Get<JwtSettings>();
            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret));
            services.AddSingleton(symmetricKey);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opts =>
                {
                    opts.SaveToken = false;
                    opts.RequireHttpsMetadata = false;
                    opts.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = symmetricKey,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidateIssuer = true,
                        ValidAudience = jwtSettings.Audience,
                        ValidateAudience = true,
                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });
        }
    }
}