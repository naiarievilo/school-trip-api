using Ardalis.Specification;
using SchoolTripApi.Domain.Common.Abstractions;

namespace SchoolTripApi.Application.Common.Abstractions;

public interface IRepository<T> : IRepositoryBase<T> where T : class, IAggregateRoot
{
}