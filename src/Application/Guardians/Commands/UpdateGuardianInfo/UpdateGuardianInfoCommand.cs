using Mediator;
using SchoolTripApi.Domain.Common.DTOs;
using SchoolTripApi.Domain.Common.ValueObjects;
using SchoolTripApi.Domain.GuardianAggregate.ValueObjects;

namespace SchoolTripApi.Application.Guardians.Commands.UpdateGuardianInfo;

public sealed class UpdateGuardianInfoCommand : ICommand<Result>
{
    public FullName? FullName { get; set; }
    public Cpf? Cpf { get; set; }
    public Address? Address { get; set; }
    public EmergencyContact? EmergencyContact { get; set; }

    public string? AccountId { get; private set; }

    public UpdateGuardianInfoCommand For(string accountId)
    {
        AccountId = accountId;
        return this;
    }
}