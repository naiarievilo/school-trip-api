namespace SchoolTripApi.Domain.Guardian.GuardianAggregate.ValueObjects;

public sealed record Address(
    Street Street,
    StreetNumber StreetNumber,
    Neighborhood Neighborhood,
    City City,
    State State,
    Country Country,
    PostalCode PostalCode);