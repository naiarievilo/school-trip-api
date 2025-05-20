using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetFiveApiDemo.Application.Auth.Interfaces;
using DotNetFiveApiDemo.Application.Auth.Services;
using DotNetFiveApiDemo.Application.Settings;
using DotNetFiveApiDemo.Application.User.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace DotNetFiveApiDemo.Application.Auth.Extensions
{
    internal static class JwtTokenExtensions
    {
        internal static IServiceCollection AddJwtAuthentication(this IServiceCollection services,
            IConfiguration configuration)
        {
            var jwtSettingsSection = configuration.GetSection("Jwt");
            services.Configure<JwtSettings>(jwtSettingsSection);
            var jwtSettings = jwtSettingsSection.Get<JwtSettings>();

            services.AddScoped<IJwtTokenProvider<ApplicationUser>, JwtTokenProvider>();
            services.AddScoped<IAuthenticationService<ApplicationUser>, AuthenticationService>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opts =>
                {
                    opts.SaveToken = false;
                    opts.RequireHttpsMetadata = false;
                    opts.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidateIssuer = true,
                        ValidAudience = jwtSettings.Audience,
                        ValidateAudience = true,
                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };

                    opts.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = context =>
                        {
                            var tokenType = context.Principal!.Claims.FirstOrDefault(c => c.Type == "Type")?.Value;
                            if (tokenType != nameof(JwtTokenTypes.AccessToken))
                                context.Fail("Token is not an access token.");
                            return Task.CompletedTask;
                        }
                    };
                });

            return services;
        }
    }
}