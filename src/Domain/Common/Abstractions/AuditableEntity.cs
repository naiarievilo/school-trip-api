namespace SchoolTripApi.Domain.Common.Abstractions;

public abstract class AuditableEntity<TId> : Entity<TId>
{
    protected AuditableEntity(string createdBy)
    {
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
    }

    public DateTimeOffset CreatedAt { get; protected init; }
    public string CreatedBy { get; protected set; }
    public DateTimeOffset? LastModifiedAt { get; protected set; }
    public string? LastModifiedBy { get; protected set; }

    public virtual void UpdateLastModified(string? lastModifiedBy)
    {
        LastModifiedAt = DateTimeOffset.UtcNow;
        if (lastModifiedBy is null) return;
        LastModifiedBy = lastModifiedBy;
    }
}