using AccountId = SchoolTripApi.Domain.Guardian.GuardianAggregate.ValueObjects.AccountId;

namespace SchoolTripApi.Application.Account;

public class AccountDto
{
    public required AccountId Id { get; init; }
    public required string Email { get; init; }
    public required bool IsEmailConfirmed { get; init; }
    public required string PhoneNumber { get; init; }
}