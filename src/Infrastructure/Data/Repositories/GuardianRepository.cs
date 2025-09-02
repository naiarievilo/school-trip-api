using Ardalis.Specification;
using SchoolTripApi.Domain.GuardianAggregate;

namespace SchoolTripApi.Infrastructure.Data.Repositories;

internal sealed class GuardianRepository : Repository<Guardian>
{
    public GuardianRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public GuardianRepository(AppDbContext dbContext, ISpecificationEvaluator specificationEvaluator) : base(dbContext,
        specificationEvaluator)
    {
    }
}