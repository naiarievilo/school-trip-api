namespace SchoolTripApi.Infrastructure.Security.Settings;

internal sealed class ClientUrls
{
    public required string EmailConfirmation { get; init; }
    public required string PasswordReset { get; init; }
}