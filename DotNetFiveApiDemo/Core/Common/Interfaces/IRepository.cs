using Ardalis.Specification;

namespace DotNetFiveApiDemo.Core.Common.Interfaces
{
    public interface IRepository<T> : IRepositoryBase<T> where T : class, IAggregateRoot
    {
    }
}