using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SchoolTripApi.Infrastructure.Data;
using SchoolTripApi.Infrastructure.Security.Entities;

namespace SchoolTripApi.Infrastructure.Security.Tasks;

internal sealed class DeleteUnverifiedUsers(ILogger<DeleteUnverifiedUsers> logger, IServiceProvider serviceProvider)
    : BackgroundService
{
    private readonly TimeSpan _interval = TimeSpan.FromDays(1);

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await DoTaskAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Couldn't complete task: {1}", ex.Message);
            }

            await Task.Delay(_interval, cancellationToken);
        }
    }

    private async Task DoTaskAsync()
    {
        using var scope = serviceProvider.CreateScope();

        var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Delete users a week after account creation if they're still not verified
        IEnumerable<Account> unverifiedUsers = await appDbContext.Users
            .Where(u => u.CreatedAt < DateTime.UtcNow.AddDays(-7) && !u.EmailConfirmed)
            .ToListAsync();

        appDbContext.RemoveRange(unverifiedUsers);
        var usersDeleted = await appDbContext.SaveChangesAsync();

        logger.LogInformation("Task completed successfully [Users deleted: {1st}].", usersDeleted);
    }
}