using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DotNetFiveApiDemo.Core.Security.Entities;
using DotNetFiveApiDemo.Core.User.Entities;
using DotNetFiveApiDemo.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DotNetFiveApiDemo.Core.Security.Tasks
{
    public sealed class DeleteExpiredRefreshTokens : BackgroundService
    {
        private readonly TimeSpan _interval = TimeSpan.FromDays(1);
        private readonly ILogger<DeleteExpiredRefreshTokens> _logger;
        private readonly IServiceProvider _serviceProvider;

        public DeleteExpiredRefreshTokens(ILogger<DeleteExpiredRefreshTokens> logger, IServiceProvider serviceProvider)
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

            IEnumerable<RefreshToken<AppUser>> expiredRefreshTokens = await appDbContext.RefreshTokens
                .Where(rt => rt.ExpiresAt < DateTime.UtcNow)
                .ToListAsync();

            appDbContext.RefreshTokens.RemoveRange(expiredRefreshTokens);
            var entriesModified = await appDbContext.SaveChangesAsync();

            _logger.LogInformation("Task completed successfully [Refresh tokens deleted: {1}].", entriesModified);
        }
    }
}