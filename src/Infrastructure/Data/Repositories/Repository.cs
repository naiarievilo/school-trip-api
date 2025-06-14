using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using SchoolTripApi.Application.Common.Abstractions;
using SchoolTripApi.Domain.Common.Abstractions;

namespace SchoolTripApi.Infrastructure.Data.Repositories;

public abstract class Repository<T> : RepositoryBase<T>, IRepository<T> where T : class, IAggregateRoot
{
    protected Repository(AppDbContext dbContext) : base(dbContext)
    {
    }

    protected Repository(AppDbContext dbContext, ISpecificationEvaluator specificationEvaluator) :
        base(dbContext, specificationEvaluator)
    {
    }
}