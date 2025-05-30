using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using DotNetFiveApiDemo.Core.Common.Interfaces;

namespace DotNetFiveApiDemo.Infrastructure.Data.Repositories
{
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
}