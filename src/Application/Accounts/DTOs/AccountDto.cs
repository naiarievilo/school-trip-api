namespace SchoolTripApi.Application.Accounts.DTOs;

public sealed class AccountDto
{
    public required Guid Id { get; init; }
    public required string Email { get; init; }
    public required bool IsEmailConfirmed { get; init; }
    public string? PhoneNumber { get; init; }
    public required List<string> Roles { get; init; }
}