using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SchoolTripApi.Application.Common.Abstractions;
using SchoolTripApi.Infrastructure.Security.Entities;
using SchoolTripApi.Infrastructure.Security.Specifications;

namespace SchoolTripApi.Infrastructure.Security.Tasks;

internal sealed class DeleteExpiredRefreshTokens(
    ILogger<DeleteExpiredRefreshTokens> logger,
    IServiceProvider serviceProvider)
    : BackgroundService
{
    private readonly TimeSpan _interval = TimeSpan.FromDays(1);

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await DoTaskAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Couldn't complete task: {1}", ex.Message);
            }

            await Task.Delay(_interval, cancellationToken);
        }
    }

    private async Task DoTaskAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();

        var refreshTokenRepository = scope.ServiceProvider.GetRequiredService<IRepository<RefreshToken>>();

        var spec = new RefreshTokenByExpirationDateSpec();
        IEnumerable<RefreshToken> expiredRefreshTokens =
            await refreshTokenRepository.ListAsync(spec, cancellationToken);

        await refreshTokenRepository.DeleteRangeAsync(expiredRefreshTokens, cancellationToken);

        logger.LogInformation("Task completed successfully [Refresh tokens deleted: {1}].",
            expiredRefreshTokens.Count());
    }
}