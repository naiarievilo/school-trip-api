using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.Services;
using SchoolTripApi.Domain.Guardian.GuardianAggregate.ValueObjects;

namespace SchoolTripApi.Domain.Guardian.GuardianAggregate;

public class Guardian : AuditableEntity<GuardianId>, IAggregateRoot
{
    // For EF Core
    public Guardian()
    {
    }

    public Guardian(AccountId accountId, FullName fullName, Cpf? cpf = null, Address? address = null,
        EmergencyContact? emergencyContact = null)
    {
        Id = GuidFactory<GuardianId>.NewSequential();
        AccountId = accountId;
        FullName = fullName;
        Cpf = cpf;
        Address = address;
        EmergencyContact = emergencyContact;
        CreatedAt = DateTimeOffset.UtcNow;
        CreatedBy = FullName.Value;
    }

    public AccountId AccountId { get; private set; }
    public FullName FullName { get; }
    public Cpf? Cpf { get; private set; }
    public Address? Address { get; private set; }
    public EmergencyContact? EmergencyContact { get; private set; }
    public GuardianStatus Status { get; private set; } = GuardianStatus.Active;
}