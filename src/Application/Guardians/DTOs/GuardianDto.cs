using SchoolTripApi.Domain.Common.ValueObjects;
using SchoolTripApi.Domain.GuardianAggregate.ValueObjects;

namespace SchoolTripApi.Application.Guardians.DTOs;

public sealed class GuardianDto
{
    public required FullName? FullName { get; init; }
    public required Cpf? Cpf { get; init; }
    public required Address? Address { get; init; }
    public required EmergencyContact? EmergencyContact { get; init; }
}