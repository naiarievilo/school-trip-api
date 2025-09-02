namespace SchoolTripApi.Domain.GuardianAggregate.ValueObjects;

public sealed record EmergencyContact(
    ContactName Name,
    PhoneNumber PhoneNumber);