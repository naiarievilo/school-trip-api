namespace SchoolTripApi.Domain.Common.Abstractions;

public abstract class AuditableEntity<T> : Entity<T>
{
    public DateTimeOffset CreatedAt { get; protected init; }
    public string CreatedBy { get; protected set; } = null!;
    public DateTimeOffset? LastModifiedAt { get; protected set; }
    public string? LastModifiedBy { get; protected set; }
}