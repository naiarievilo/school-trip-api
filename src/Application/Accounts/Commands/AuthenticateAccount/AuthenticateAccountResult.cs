namespace SchoolTripApi.Application.Accounts.Commands.AuthenticateAccount;

public class AuthenticateAccountResult
{
    public required Guid AccountId { get; init; }
    public required bool IsEmailConfirmed { get; init; }
    public required string AccessToken { get; init; }
    public required DateTimeOffset ExpiresAt { get; init; }
    public required string RefreshToken { get; init; }
}