namespace SchoolTripApi.Infrastructure.Security.Settings;

internal sealed class AdminSettings
{
    public required string Email { get; init; }
    public string? Password { get; init; }
}