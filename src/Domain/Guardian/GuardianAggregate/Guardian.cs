using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Guardian.GuardianAggregate.ValueObjects;

namespace SchoolTripApi.Domain.Guardian.GuardianAggregate;

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
    public FullName? FullName { get; }
    public Cpf? Cpf { get; private set; }
    public Address? Address { get; private set; }
    public EmergencyContact? EmergencyContact { get; private set; }
    public GuardianStatus Status { get; private set; } = GuardianStatus.Active;
}