using DotNetFiveApiDemo.Core.Email.Interfaces;
using DotNetFiveApiDemo.Core.Email.Settings;
using DotNetFiveApiDemo.Infrastructure.Email.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetFiveApiDemo.Infrastructure.Email.Extensions
{
    internal static class EmailExtensions
    {
        internal static IServiceCollection AddEmailService(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<EmailSettings>(configuration.GetSection("MailingSettings"));
            services.AddSingleton<IEmailSender, EmailSender>();
            return services;
        }
    }
}