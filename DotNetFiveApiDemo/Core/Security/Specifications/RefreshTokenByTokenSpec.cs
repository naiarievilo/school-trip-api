using Ardalis.Specification;
using DotNetFiveApiDemo.Core.Security.Entities;
using DotNetFiveApiDemo.Core.User.Entities;

namespace DotNetFiveApiDemo.Core.Security.Specifications
{
    public sealed class RefreshTokenByTokenSpec : Specification<RefreshToken<AppUser>>, ISingleResultSpecification
    {
        public RefreshTokenByTokenSpec(string token)
        {
            Query
                .Where(rt => rt.Token.Equals(token))
                .Include(rt => rt.User);
        }
    }
}