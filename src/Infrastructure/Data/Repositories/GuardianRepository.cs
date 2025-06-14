using Ardalis.Specification;
using SchoolTripApi.Domain.Guardian.GuardianAggregate;

namespace SchoolTripApi.Infrastructure.Data.Repositories;

public class GuardianRepository : Repository<Guardian>
{
    public GuardianRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public GuardianRepository(AppDbContext dbContext, ISpecificationEvaluator specificationEvaluator) : base(dbContext,
        specificationEvaluator)
    {
    }
}