namespace SchoolTripApi.Application.Account;

public class AccountDto
{
    public required Guid Id { get; init; }
    public required string Email { get; init; }
    public required bool IsEmailConfirmed { get; init; }
    public required string PhoneNumber { get; init; }
}