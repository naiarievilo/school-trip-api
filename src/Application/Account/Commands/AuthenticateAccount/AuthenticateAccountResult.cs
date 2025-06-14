using AccountId = SchoolTripApi.Domain.Guardian.GuardianAggregate.ValueObjects.AccountId;

namespace SchoolTripApi.Application.Account.Commands.AuthenticateAccount;

public class AuthenticateAccountResult
{
    public required AccountId AccountId { get; init; }
    public required bool IsEmailConfirmed { get; init; }
    public required string AccessToken { get; init; }
    public required DateTimeOffset ExpiresAt { get; init; }
    public required string RefreshToken { get; init; }
}