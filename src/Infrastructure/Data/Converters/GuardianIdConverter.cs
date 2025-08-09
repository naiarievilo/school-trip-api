using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SchoolTripApi.Domain.Guardian.GuardianAggregate.ValueObjects;

namespace SchoolTripApi.Infrastructure.Data.Converters;

public class GuardianIdConverter()
    : ValueConverter<GuardianId, Guid>(guardianId => guardianId.Value, guid => GuardianId.From(guid));