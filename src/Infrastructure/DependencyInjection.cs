using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using PuppeteerSharp;
using SchoolTripApi.Application.Accounts.Abstractions;
using SchoolTripApi.Application.Agreements.Abstractions;
using SchoolTripApi.Application.Common.Abstractions;
using SchoolTripApi.Domain.GuardianAggregate;
using SchoolTripApi.Infrastructure.Data;
using SchoolTripApi.Infrastructure.Data.Configurations;
using SchoolTripApi.Infrastructure.Data.Repositories;
using SchoolTripApi.Infrastructure.Email;
using SchoolTripApi.Infrastructure.FileStorage;
using SchoolTripApi.Infrastructure.Logging;
using SchoolTripApi.Infrastructure.Security.Entities;
using SchoolTripApi.Infrastructure.Security.Services;
using SchoolTripApi.Infrastructure.Security.Settings;
using SchoolTripApi.Infrastructure.Security.Tasks;
using SchoolTripApi.Infrastructure.WebScraping.Abstractions;
using SchoolTripApi.Infrastructure.WebScraping.Services;
using SchoolTripApi.Infrastructure.WebScraping.Settings;
using SignatureValidator = SchoolTripApi.Infrastructure.WebScraping.Services.SignatureValidator;

namespace SchoolTripApi.Infrastructure;

public static class DependencyInjection
{
    private const string DefaultConnection = "DefaultConnection";

    public static void AddInfrastructureConfiguration(this IServiceCollection services,
        IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.AddDatabases(configuration);
        services.AddSecurity(configuration);
        services.AddEmailing(configuration);
        services.AddFileStore(configuration, environment);
        services.AddWebScraper(configuration);
        services.AddLogging();
    }

    private static void AddDatabases(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEntityFrameworkCore(configuration)
            .AddIdentityCore()
            .AddRepositories();
        services.AddMongoDb(configuration);
    }

    private static IServiceCollection AddEntityFrameworkCore(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(opts =>
        {
            opts.UseNpgsql(configuration.GetConnectionString(DefaultConnection) ??
                           throw new InvalidOperationException());
        });

        return services;
    }

    private static void AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        MongoDbConfiguration.ConfigureEntities();
        services.AddSingleton<IMongoClient>(sp =>
        {
            var connectionString = configuration.GetConnectionString("MongoDB");
            return new MongoClient(connectionString);
        });

        services.AddScoped(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            var connectionString = configuration.GetConnectionString("MongoDB");
            var mongoUrl = MongoUrl.Create(connectionString);
            return client.GetDatabase(mongoUrl.DatabaseName);
        });
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

    private static void AddEmailing(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<EmailSettings>(configuration.GetSection(nameof(EmailSettings)));
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
        var clientSection = configuration.GetSection(nameof(ClientSettings));
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
        var jwtSettingsSection = configuration.GetSection(nameof(JwtSettings));
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

        var adminSettingsSection = configuration.GetSection(nameof(AdminSettings));
        services.Configure<AdminSettings>(adminSettingsSection);
    }

    private static void AddLogging(this IServiceCollection services)
    {
        services.AddScoped(typeof(IAppLogger<>), typeof(AppLogger<>));
    }

    private static void AddWebScraper(this IServiceCollection services, IConfiguration configuration)
    {
        var browserSection = configuration.GetSection(nameof(BrowserSettings));
        var signatureValidationSection = configuration.GetSection(nameof(SignatureValidatorSettings));
        services.Configure<BrowserSettings>(browserSection);
        services.Configure<SignatureValidatorSettings>(signatureValidationSection);

        services.AddSingleton<IAppLogger<BrowserService>, AppLogger<BrowserService>>();
        services.AddSingleton<IBrowserService<IBrowser, IPage>, BrowserService>();
        services.AddScoped<ISignatureValidator, SignatureValidator>();
    }

    private static void AddFileStore(this IServiceCollection services, IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        // Currently only set up for development environment
        if (environment.EnvironmentName != "Development") return;

        var solutionRoot = Directory.GetParent(environment.ContentRootPath)?.FullName!;

        services.Configure<LocalFileStoreSettings>(options =>
        {
            var settings = configuration.GetSection(nameof(LocalFileStoreSettings)).Get<LocalFileStoreSettings>();
            options.SignedAgreementsPath = Path.Combine(solutionRoot, settings!.SignedAgreementsPath);
            Directory.CreateDirectory(options.SignedAgreementsPath);
        });

        services.AddScoped<IFileStore, LocalFileStore>();
    }
}