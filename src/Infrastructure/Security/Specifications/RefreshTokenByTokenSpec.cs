using Ardalis.Specification;
using SchoolTripApi.Infrastructure.Security.Entities;

namespace SchoolTripApi.Infrastructure.Security.Specifications;

internal sealed class RefreshTokenByTokenSpec : SingleResultSpecification<RefreshToken>
{
    public RefreshTokenByTokenSpec(string token)
    {
        Query
            .Where(rt => rt.Token.Equals(token))
            .Include(rt => rt.User);
    }
}