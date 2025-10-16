using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Infrastructure.Data.Converters;
using ValueConverter = Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter;

namespace SchoolTripApi.Infrastructure.Data.Configurations;

internal static class ValueObjectConfiguration
{
    public static void ConfigureValueObjects<TEntity>(this EntityTypeBuilder<TEntity> entityTypeBuilder)
        where TEntity : class
    {
        var entityType = typeof(TEntity);
        var properties = entityType.GetProperties();

        foreach (var property in properties)
        {
            if (IsNavigationProperty(property.PropertyType)) continue;
            ApplyConverterToProperty(entityTypeBuilder, property);
        }
    }

    public static void ConfigureValueObjects(this OwnedNavigationBuilder ownedTypeBuilder)
    {
        var ownedType = ownedTypeBuilder.OwnedEntityType.ClrType;
        var properties = ownedType.GetProperties();

        foreach (var property in properties)
        {
            if (IsNavigationProperty(property.PropertyType)) continue;
            ApplyConverterToOwnedProperty(ownedTypeBuilder, property);
        }
    }

    private static bool IsNavigationProperty(Type propertyType)
    {
        if (typeof(IAggregateRoot).IsAssignableFrom(propertyType)) return true;
        if (!propertyType.IsGenericType) return false;

        var genericTypeDefinition = propertyType.GetGenericTypeDefinition();
        if (genericTypeDefinition != typeof(ICollection<>) &&
            genericTypeDefinition != typeof(IEnumerable<>) &&
            genericTypeDefinition != typeof(IList<>) &&
            genericTypeDefinition != typeof(List<>) &&
            genericTypeDefinition != typeof(HashSet<>) &&
            genericTypeDefinition != typeof(ISet<>)) return false;

        var elementType = propertyType.GetGenericArguments()[0];
        return typeof(IAggregateRoot).IsAssignableFrom(elementType);
    }

    private static void ApplyConverterToProperty<TEntity>(EntityTypeBuilder<TEntity> entityTypeBuilder,
        PropertyInfo propertyInfo) where TEntity : class
    {
        var propertyType = propertyInfo.PropertyType;
        var propertyName = propertyInfo.Name;

        var valueType = GetSimpleValueObjectValueType(propertyType);
        if (valueType is not null)
        {
            var converterType = typeof(SimpleValueObjectConverter<,>).MakeGenericType(propertyType, valueType);
            var converter = Activator.CreateInstance(converterType);
            var propertyBuilder = entityTypeBuilder
                .Property(propertyName)
                .HasConversion(converter as ValueConverter)
                .HasColumnName(propertyName);

            IncludeMaxLength(propertyType, propertyBuilder);
            IncludeIntegerIdGeneratedValues(propertyType, propertyBuilder);
        }
    }

    private static void IncludeIntegerIdGeneratedValues(Type propertyType, PropertyBuilder propertyBuilder)
    {
        var currentType = propertyType;
        while (currentType is not null && currentType != typeof(object))
        {
            if (currentType.IsGenericType && currentType.GetGenericTypeDefinition() == typeof(IntegerId<>))
                propertyBuilder.ValueGeneratedOnAdd();

            currentType = currentType.BaseType;
        }
    }

    private static void ApplyConverterToOwnedProperty(OwnedNavigationBuilder ownedTypeBuilder,
        PropertyInfo propertyInfo)
    {
        var propertyType = propertyInfo.PropertyType;
        var propertyName = propertyInfo.Name;

        var valueType = GetSimpleValueObjectValueType(propertyType);
        if (valueType is not null)
        {
            var converterType = typeof(SimpleValueObjectConverter<,>).MakeGenericType(propertyType, valueType);
            var converter = Activator.CreateInstance(converterType);
            var propertyBuilder = ownedTypeBuilder
                .Property(propertyType, propertyName)
                .HasConversion(converter as ValueConverter)
                .HasColumnName(propertyName);

            IncludeMaxLength(propertyType, propertyBuilder);
        }
    }

    private static void IncludeMaxLength(Type propertyType, PropertyBuilder propertyBuilder)
    {
        if (propertyType != typeof(string)) return;
        var maxLength = GetMaxLengthFromValueObject(propertyType);
        propertyBuilder.HasMaxLength(maxLength);
    }

    private static int GetMaxLengthFromValueObject(Type valueObjectType)
    {
        var maxLengthProperty = valueObjectType.GetProperty("MaxLength",
            BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

        if (maxLengthProperty is not null && maxLengthProperty.PropertyType == typeof(int))
            return (int)(maxLengthProperty.GetValue(null) ?? throw new InvalidOperationException(
                $"Field 'MaxLength' of type '{valueObjectType}' must have valid maximum length."));

        throw new InvalidOperationException(
            $"Field 'MaxLength' for value object '{valueObjectType}' not found or not of type '{typeof(int)}'.");
    }

    private static Type? GetSimpleValueObjectValueType(Type propertyType)
    {
        var currentType = propertyType;
        while (currentType is not null && currentType != typeof(object))
        {
            if (currentType.IsGenericType && currentType.GetGenericTypeDefinition() == typeof(SimpleValueObject<,>))
                return currentType.GetGenericArguments()[1];

            currentType = currentType.BaseType;
        }

        return null;
    }
}