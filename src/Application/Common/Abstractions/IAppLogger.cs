namespace SchoolTripApi.Application.Common.Abstractions;

public interface IAppLogger<T>
{
    void LogInformation(string message, params object[] args);
    void LogError(Exception exception, string message, params object[] args);
    void LogError(string message, params object[] args);
    void LogWarning(string message, params object[] args);
}