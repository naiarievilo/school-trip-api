using Ardalis.Specification;
using SchoolTripApi.Infrastructure.Security.Entities;

namespace SchoolTripApi.Infrastructure.Security.Specifications;

public sealed class RefreshTokenByExpirationDate : Specification<RefreshToken>
{
    public RefreshTokenByExpirationDate()
    {
        Query
            .Where(rt => rt.ExpiresAt < DateTime.UtcNow);
    }
}