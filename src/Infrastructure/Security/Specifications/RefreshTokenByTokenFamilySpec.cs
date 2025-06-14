using Ardalis.Specification;
using SchoolTripApi.Infrastructure.Security.Entities;

namespace SchoolTripApi.Infrastructure.Security.Specifications;

public sealed class RefreshTokenByTokenFamilySpec : Specification<RefreshToken>
{
    public RefreshTokenByTokenFamilySpec(string tokenFamily)
    {
        Query.Where(rt => rt.TokenFamily.Equals(tokenFamily));
    }
}