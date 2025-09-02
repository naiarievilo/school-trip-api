using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.ValueObjects;
using SchoolTripApi.Domain.GuardianAggregate.ValueObjects;

namespace SchoolTripApi.Domain.GuardianAggregate;

public class Guardian : AuditableEntity<GuardianId>, IAggregateRoot
{
    // For EF Core
    protected Guardian()
    {
        AccountId = null!;
    }

    private Guardian(AccountId accountId, FullName fullName, Cpf? cpf, Address? address,
        EmergencyContact? emergencyContact, string createdBy)
    {
        Id = GuardianId.From(Guid.NewGuid());
        AccountId = accountId;
        FullName = fullName;
        Cpf = cpf;
        Address = address;
        EmergencyContact = emergencyContact;
        CreatedAt = DateTimeOffset.UtcNow;
        CreatedBy = createdBy;
    }

    public Guardian(AccountId accountId, FullName fullName, string createdBy) : this(accountId, fullName, null, null,
        null, createdBy)
    {
    }

    public AccountId AccountId { get; private set; }
    public FullName? FullName { get; set; }
    public Cpf? Cpf { get; set; }
    public Address? Address { get; set; }
    public EmergencyContact? EmergencyContact { get; set; }
    public GuardianStatus Status { get; set; } = GuardianStatus.Active;


    public Guardian UpdateLastModifiedAt(string? lastModifiedBy = null)
    {
        LastModifiedAt = DateTimeOffset.UtcNow;
        if (lastModifiedBy is not null)
        {
            LastModifiedBy = lastModifiedBy;
            return this;
        }

        LastModifiedBy = FullName?.Value ?? "Guardian";
        return this;
    }
}