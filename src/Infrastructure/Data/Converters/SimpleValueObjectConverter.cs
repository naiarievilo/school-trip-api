using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SchoolTripApi.Domain.Common.Abstractions;

namespace SchoolTripApi.Infrastructure.Data.Converters;

public sealed class SimpleValueObjectConverter<TValueObject, TValue>() : ValueConverter<TValueObject, TValue>(
    valueObject =>
        valueObject.Value, value => SimpleValueObject<TValueObject, TValue>.From(value))
    where TValueObject : SimpleValueObject<TValueObject, TValue>
{
}