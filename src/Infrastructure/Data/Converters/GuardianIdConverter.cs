using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SchoolTripApi.Domain.GuardianAggregate.ValueObjects;

namespace SchoolTripApi.Infrastructure.Data.Converters;

internal sealed class GuardianIdConverter()
    : ValueConverter<GuardianId, Guid>(guardianId => guardianId.Value, guid => GuardianId.From(guid));