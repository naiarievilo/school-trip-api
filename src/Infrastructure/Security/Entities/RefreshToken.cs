using SchoolTripApi.Domain.Common.Abstractions;

namespace SchoolTripApi.Infrastructure.Security.Entities;

public class RefreshToken : Entity<Guid>, IAggregateRoot
{
    public RefreshToken(Guid accountId, string token, string tokenFamily, DateTime expiresAt)
    {
        Id = Guid.NewGuid();
        AccountId = accountId;
        Token = token;
        TokenFamily = tokenFamily;
        TokenFamily = Guid.NewGuid().ToString();
        ExpiresAt = expiresAt;
        IsRevoked = false;
    }

    public Guid AccountId { get; init; }

    public string Token { get; init; }

    public string TokenFamily { get; init; }

    public DateTime ExpiresAt { get; init; }

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