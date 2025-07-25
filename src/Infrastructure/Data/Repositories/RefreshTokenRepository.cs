using Ardalis.Specification;
using SchoolTripApi.Infrastructure.Security.Entities;

namespace SchoolTripApi.Infrastructure.Data.Repositories;

public class RefreshTokenRepository : Repository<RefreshToken>
{
    public RefreshTokenRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public RefreshTokenRepository(AppDbContext dbContext, ISpecificationEvaluator specificationEvaluator) : base(
        dbContext, specificationEvaluator)
    {
    }
}