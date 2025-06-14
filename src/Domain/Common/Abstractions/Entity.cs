namespace SchoolTripApi.Domain.Common.Abstractions;

public abstract class Entity<T>
{
    public T Id { get; protected init; } = default!;
}