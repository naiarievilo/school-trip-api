using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DotNetFiveApiDemo.Core.User.Entities;
using DotNetFiveApiDemo.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DotNetFiveApiDemo.Core.User.Tasks
{
    public sealed class DeleteUnverifiedUsers : BackgroundService
    {
        private readonly TimeSpan _interval = TimeSpan.FromDays(1);
        private readonly ILogger<DeleteUnverifiedUsers> _logger;
        private readonly IServiceProvider _serviceProvider;

        public DeleteUnverifiedUsers(ILogger<DeleteUnverifiedUsers> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await DoTaskAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Couldn't complete task: {1}", ex.Message);
                }

                await Task.Delay(_interval, stoppingToken);
            }
        }

        private async Task DoTaskAsync()
        {
            using var scope = _serviceProvider.CreateScope();

            var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Delete users a week after account creation if they're still not verified
            IEnumerable<AppUser> unverifiedUsers = await appDbContext.Users
                .Where(u => u.CreatedAt < DateTime.UtcNow.AddDays(-7) && !u.EmailConfirmed
                ).ToListAsync();

            appDbContext.Users.RemoveRange(unverifiedUsers);
            var entriesModified = await appDbContext.SaveChangesAsync();

            _logger.LogInformation("Task completed successfully [Users deleted: {1}].", entriesModified);
        }
    }
}