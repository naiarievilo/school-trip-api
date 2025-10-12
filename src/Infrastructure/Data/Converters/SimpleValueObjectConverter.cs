using System.Reflection;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SchoolTripApi.Domain.Common.Abstractions;

namespace SchoolTripApi.Infrastructure.Data.Converters;

public sealed class SimpleValueObjectConverter<TValueObject, TValue>()
    : ValueConverter<TValueObject, TValue>(valueObject => GetValue(valueObject), value => CreateInstance(value))
    where TValueObject : class
{
    private static TValue GetValue(TValueObject valueObject)
    {
        var property = typeof(TValueObject).GetProperty("Value");
        if (property is not null) return (TValue)property.GetValue(valueObject)!;

        var baseType = typeof(TValueObject).BaseType;
        while (baseType is not null && property is null)
        {
            property = baseType.GetProperty("Value", BindingFlags.Public | BindingFlags.Instance);
            baseType = baseType.BaseType;
        }

        return (TValue)property!.GetValue(valueObject)!;
    }

    private static TValueObject CreateInstance(TValue value)
    {
        var constructor = SimpleValueObjectConstructorFactory.GetValueObjectConstructor<TValueObject, TValue>();
        return constructor(value);
    }
}