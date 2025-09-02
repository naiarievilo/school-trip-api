using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SchoolTripApi.Application.Accounts.Abstractions;
using SchoolTripApi.Application.Common.Abstractions;
using SchoolTripApi.Domain.GuardianAggregate;
using SchoolTripApi.Infrastructure.Data;
using SchoolTripApi.Infrastructure.Data.Repositories;
using SchoolTripApi.Infrastructure.Email;
using SchoolTripApi.Infrastructure.Logging;
using SchoolTripApi.Infrastructure.Security.Entities;
using SchoolTripApi.Infrastructure.Security.Services;
using SchoolTripApi.Infrastructure.Security.Settings;
using SchoolTripApi.Infrastructure.Security.Tasks;

namespace SchoolTripApi.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructureConfiguration(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAppDbContext(configuration)
            .AddIdentityCore()
            .AddRepositories();

        services.AddSecurity(configuration);
        services.AddEmailService(configuration);
        services.AddLogging();
    }

    private static IServiceCollection AddAppDbContext(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(opts =>
        {
            opts.UseNpgsql(configuration.GetConnectionString("DefaultConnection") ??
                           throw new InvalidOperationException());
        });

        return services;
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IRepository<RefreshToken>, RefreshTokenRepository>();
        services.AddScoped<IRepository<Guardian>, GuardianRepository>();
    }

    private static IServiceCollection AddIdentityCore(this IServiceCollection services)
    {
        services.AddIdentityCore<Account>(opts =>
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
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddRoleManager<RoleManager<IdentityRole<Guid>>>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

        return services;
    }

    private static void AddEmailService(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<EmailSettings>(configuration.GetSection("MailingSettings"));
        services.AddScoped<IEmailSender, EmailSender>();
    }

    private static void AddSecurity(this IServiceCollection services,
        IConfiguration configuration)
    {
        AddSecuritySettings(services, configuration);
        AddSecurityServices(services, configuration);
        AddSecurityTasks(services);
    }

    private static void AddSecurityTasks(this IServiceCollection services)
    {
        services.AddHostedService<DeleteExpiredRefreshTokens>();
        services.AddHostedService<DeleteUnverifiedUsers>();
    }

    private static void AddSecuritySettings(this IServiceCollection services,
        IConfiguration configuration)
    {
        // SPA settings
        var clientSection = configuration.GetSection("Client");
        services.Configure<ClientSettings>(clientSection);
    }

    private static void AddSecurityServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        // Authentication Services
        services.AddScoped<ISecurityTokenProvider, SecurityTokenProvider>();
        services.AddScoped<ISecurityEmailService, SecurityEmailService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IAccountManager, AccountManager>();

        // Avoid creating duplicate code in IJwtTokenProvider implementation and authentication settings
        var jwtSettingsSection = configuration.GetSection("Jwt");
        services.Configure<JwtSettings>(jwtSettingsSection);
        var jwtSettings = jwtSettingsSection.Get<JwtSettings>();
        var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings!.Secret));
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

        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        var adminSettingsSection = configuration.GetSection("AdminSettings");
        services.Configure<AdminSettings>(adminSettingsSection);
    }

    private static void AddLogging(this IServiceCollection services)
    {
        services.AddScoped(typeof(IAppLogger<>), typeof(AppLogger<>));
    }
}