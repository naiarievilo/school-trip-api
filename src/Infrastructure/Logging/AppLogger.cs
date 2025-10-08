using Microsoft.Extensions.Logging;
using SchoolTripApi.Application.Common.Abstractions;

namespace SchoolTripApi.Infrastructure.Logging;

internal sealed class AppLogger<T>(ILoggerFactory loggerFactory) : IAppLogger<T>
{
    private readonly ILogger<T> _logger = loggerFactory.CreateLogger<T>();

    public void LogInformation(string message, params object[] args)
    {
        _logger.LogInformation(message, args);
    }

    public void LogError(Exception exception, string message, params object[] args)
    {
        _logger.LogError(exception, message, args);
    }

    public void LogError(string message, params object[] args)
    {
        _logger.LogError(message, args);
    }

    public void LogWarning(string message, params object[] args)
    {
        _logger.LogWarning(message, args);
    }

    public void LogDebug(string message, params object[] args)
    {
        _logger.LogDebug(message, args);
    }
}