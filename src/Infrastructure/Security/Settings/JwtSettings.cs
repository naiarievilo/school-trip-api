namespace SchoolTripApi.Infrastructure.Security.Settings;

internal sealed class JwtSettings
{
    public required string Secret { get; init; }
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
    public required int AccessTokenExpiresAtInMinutes { get; init; }
    public required int RefreshTokenExpiresAtInDays { get; init; }
}