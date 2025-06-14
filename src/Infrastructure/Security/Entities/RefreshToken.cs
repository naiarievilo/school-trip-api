using SchoolTripApi.Domain.Common.Abstractions;
using AccountId = SchoolTripApi.Domain.Guardian.GuardianAggregate.ValueObjects.AccountId;

namespace SchoolTripApi.Infrastructure.Security.Entities;

public class RefreshToken : Entity<Guid>, IAggregateRoot
{
    // For EF Core
    public RefreshToken()
    {
    }

    public RefreshToken(AccountId accountId, string token, string tokenFamily, DateTimeOffset expiresAt)
    {
        Id = Guid.NewGuid();
        AccountId = accountId;
        Token = token;
        TokenFamily = tokenFamily;
        TokenFamily = Guid.NewGuid().ToString();
        ExpiresAt = expiresAt;
        IsRevoked = false;
    }

    public AccountId AccountId { get; private set; }

    public string Token { get; private set; }

    public string TokenFamily { get; private set; }

    public DateTimeOffset ExpiresAt { get; }

    public bool IsRevoked { get; private set; }

    public Account? User { get; init; }

    public bool IsActive => !IsRevoked && DateTime.UtcNow < ExpiresAt;
    public bool IsExpired => DateTime.UtcNow > ExpiresAt;

    public RefreshToken Revoke()
    {
        IsRevoked = true;
        return this;
    }
}