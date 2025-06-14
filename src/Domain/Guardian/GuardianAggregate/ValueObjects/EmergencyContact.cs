namespace SchoolTripApi.Domain.Guardian.GuardianAggregate.ValueObjects;

public sealed record EmergencyContact(
    ContactName Name,
    PhoneNumber PhoneNumber);