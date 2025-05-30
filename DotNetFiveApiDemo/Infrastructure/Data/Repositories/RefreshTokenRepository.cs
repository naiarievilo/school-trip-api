using Ardalis.Specification;
using DotNetFiveApiDemo.Core.Security.Entities;
using DotNetFiveApiDemo.Core.User.Entities;

namespace DotNetFiveApiDemo.Infrastructure.Data.Repositories
{
    public class RefreshTokenRepository : Repository<RefreshToken<AppUser>>
    {
        public RefreshTokenRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public RefreshTokenRepository(AppDbContext dbContext, ISpecificationEvaluator specificationEvaluator) : base(
            dbContext, specificationEvaluator)
        {
        }
    }
}