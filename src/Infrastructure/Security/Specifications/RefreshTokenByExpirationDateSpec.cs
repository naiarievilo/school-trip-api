using Ardalis.Specification;
using SchoolTripApi.Infrastructure.Security.Entities;

namespace SchoolTripApi.Infrastructure.Security.Specifications;

internal sealed class RefreshTokenByExpirationDateSpec : Specification<RefreshToken>
{
    public RefreshTokenByExpirationDateSpec()
    {
        Query
            .Where(rt => rt.ExpiresAt < DateTime.UtcNow);
    }
}