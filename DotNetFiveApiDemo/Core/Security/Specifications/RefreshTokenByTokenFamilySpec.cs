using Ardalis.Specification;
using DotNetFiveApiDemo.Core.Security.Entities;
using DotNetFiveApiDemo.Core.User.Entities;

namespace DotNetFiveApiDemo.Core.Security.Specifications
{
    public sealed class RefreshTokenByTokenFamilySpec : Specification<RefreshToken<AppUser>>
    {
        public RefreshTokenByTokenFamilySpec(string tokenFamily)
        {
            Query.Where(rt => rt.TokenFamily.Equals(tokenFamily));
        }
    }
}