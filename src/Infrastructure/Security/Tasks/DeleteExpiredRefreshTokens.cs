using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SchoolTripApi.Application.Common.Abstractions;
using SchoolTripApi.Infrastructure.Security.Entities;
using SchoolTripApi.Infrastructure.Security.Specifications;

namespace SchoolTripApi.Infrastructure.Security.Tasks;

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

        var refreshTokenRepository =
            scope.ServiceProvider.GetRequiredService<IRepository<RefreshToken>>();

        var spec = new RefreshTokenByExpirationDate();
        IEnumerable<RefreshToken> expiredRefreshTokens =
            await refreshTokenRepository.ListAsync(spec);

        await refreshTokenRepository.DeleteRangeAsync(expiredRefreshTokens);

        _logger.LogInformation("Task completed successfully [Refresh tokens deleted: {1}].",
            expiredRefreshTokens.Count());
    }
}