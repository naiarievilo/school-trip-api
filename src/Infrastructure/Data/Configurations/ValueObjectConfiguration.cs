using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Infrastructure.Data.Converters;
using ValueConverter = Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter;

namespace SchoolTripApi.Infrastructure.Data.Configurations;

/// <summary>
///     Contains general value object configuration based on value object abstractions in
///     <see cref="Domain.Common.Abstractions" />.
/// </summary>
public static class ValueObjectConfiguration
{
    /// <summary>
    ///     Applies default converter configuration based on <see cref="SimpleValueObjectConverter{TValueObject,TValue}" /> for
    ///     flat (e.g., <see cref="Domain.Common.ValueObjects.City" /> ) and nested (e.g.,
    ///     <see cref="Domain.Common.ValueObjects.Address" />) value object entity properties.
    /// </summary>
    /// <param name="modelBuilder">
    ///     <see cref="ModelBuilder" /> passed to the <c>void OnModelCreating(ModelBuilder modelBuilder)</c> database
    ///     configuration method from <see cref="DbContext" /> or its subtypes.
    /// </param>
    public static void ApplyValueObjectConverterConfiguration(this ModelBuilder modelBuilder)
    {
        var entityTypes = modelBuilder.Model.GetEntityTypes();
        foreach (var entityType in entityTypes)
        {
            var properties = entityType.ClrType.GetProperties();
            foreach (var property in properties)
                ApplyConverterToProperty(modelBuilder, entityType, property);
        }
    }

    /// <summary>
    ///     Configures an instance of <see cref="SimpleValueObjectConverter{TValueObject,TValue}" /> for the property if it is
    ///     a flat value object, otherwise delegates the conversion configuration to <see cref="ConfigureOwnedType" /> if the
    ///     property is a nested value object (i.e., owned type in the context of EF Core).
    /// </summary>
    /// <param name="modelBuilder">Database model builder.</param>
    /// <param name="entityType">Entity type being parsed.</param>
    /// <param name="propertyInfo">Information of one of the entity type properties.</param>
    private static void ApplyConverterToProperty(ModelBuilder modelBuilder, IMutableEntityType entityType,
        PropertyInfo propertyInfo)
    {
        var propertyType = propertyInfo.PropertyType;
        var propertyName = propertyInfo.Name;

        var valueType = GetSimpleValueObjectValueType(propertyType);
        if (valueType is not null)
        {
            var converterType = typeof(SimpleValueObjectConverter<,>).MakeGenericType(propertyType, valueType);
            var converter = Activator.CreateInstance(converterType);
            modelBuilder.Entity(entityType.ClrType)
                .Property(propertyName)
                .HasConversion(converter as ValueConverter);
        }
        else if (IsReferenceTypeWithValueObjectProperties(propertyType))
        {
            ConfigureOwnedType(modelBuilder, entityType, propertyInfo);
        }
    }

    /// <summary>
    ///     Applies the same configuration logic as <see cref="ApplyConverterToProperty" />, but for a property of an
    ///     entity's owned type, or a property of a nested owned property.
    ///     <remarks>
    ///         Ideally, this method should rarely be called as nested, owned properties shouldn't be present in most
    ///         entities.
    ///     </remarks>
    /// </summary>
    /// <param name="ownedTypeBuilder">Owned type navigation builder.</param>
    /// <param name="ownedTypeProperty">Property of an owned type.</param>
    /// <param name="ownedTypePropertyName">Property name of <c>ownedTypeProperty</c>.</param>
    private static void ApplyConverterToNestedProperty(OwnedNavigationBuilder ownedTypeBuilder,
        PropertyInfo ownedTypeProperty, string ownedTypePropertyName)
    {
        var nestedPropertyType = ownedTypeProperty.PropertyType;

        var valueType = GetSimpleValueObjectValueType(ownedTypeProperty.PropertyType);
        if (valueType is not null)
        {
            var nestedPropertyConverterType = typeof(SimpleValueObjectConverter<,>)
                .MakeGenericType(nestedPropertyType, valueType);

            var nestedPropertyConverter = Activator.CreateInstance(nestedPropertyConverterType);

            ownedTypeBuilder.Property(nestedPropertyType, ownedTypePropertyName)
                .HasConversion(nestedPropertyConverter as ValueConverter)
                .HasColumnName(ownedTypePropertyName);
        }
        else if (IsReferenceTypeWithValueObjectProperties(nestedPropertyType))
        {
            ConfigureNestedOwnedProperty(ownedTypeBuilder, ownedTypeProperty,
                ownedTypePropertyName);
        }
    }

    /// <summary>
    ///     Configures an entity's owned type and iterates over its properties to check for flat or nested value objects.
    /// </summary>
    /// <param name="modelBuilder">Database model builder.</param>
    /// <param name="entityType">Entity type being parsed.</param>
    /// <param name="ownedTypeInfo">Information about the owned type.</param>
    private static void ConfigureOwnedType(ModelBuilder modelBuilder, IMutableEntityType entityType,
        PropertyInfo ownedTypeInfo)
    {
        var ownedTypeName = ownedTypeInfo.Name;
        var entityBuilder = modelBuilder.Entity(entityType.ClrType);
        var ownedTypeBuilder = entityBuilder.OwnsOne(ownedTypeInfo.PropertyType, ownedTypeName);

        var ownedTypeProperties = ownedTypeInfo.PropertyType.GetProperties();
        foreach (var ownedTypeProperty in ownedTypeProperties)
        {
            var ownedTypePropertyName = $"{ownedTypeName}_{ownedTypeProperty.Name}";
            ApplyConverterToNestedProperty(ownedTypeBuilder, ownedTypeProperty, ownedTypePropertyName);
        }
    }

    /// <summary>
    ///     Applies the same logic as <see cref="ConfigureOwnedType" />, but for an owned type inside another owned
    ///     type.
    /// </summary>
    /// <param name="ownedTypeBuilder"></param>
    /// <param name="ownedTypeInfo"></param>
    /// <param name="nameOfOwnedTypeProperty">Name of the owned type property in the owned type parent.</param>
    private static void ConfigureNestedOwnedProperty(OwnedNavigationBuilder ownedTypeBuilder,
        PropertyInfo ownedTypeInfo, string nameOfOwnedTypeProperty)
    {
        var nestedOwnedTypeBuilder = ownedTypeBuilder.OwnsOne(ownedTypeInfo.PropertyType, nameOfOwnedTypeProperty);
        var nestedOwnedTypeProperties = ownedTypeInfo.PropertyType.GetProperties();
        foreach (var nestedOwnedTypeProperty in nestedOwnedTypeProperties)
        {
            var nestedOwnedTypePropertyName = $"{nameOfOwnedTypeProperty}_{nestedOwnedTypeProperty.Name}";
            ApplyConverterToNestedProperty(nestedOwnedTypeBuilder, nestedOwnedTypeProperty,
                nestedOwnedTypePropertyName);
        }
    }

    /// <summary>
    ///     Gets the type of the <c>Value</c> property if the passed type inherits from
    ///     <see cref="SimpleValueObject{TValueObject,TValue}" />.
    /// </summary>
    /// <param name="propertyType">Type of an entity's or owned type's property.</param>
    /// <returns>Returns the type of a simple value object's <c>Value</c> property, otherwise returns <c>null</c>.</returns>
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

    /// <summary>
    ///     Checks if an entity's or owned type's property is an owned type.
    /// </summary>
    /// <param name="propertyType"></param>
    /// <returns></returns>
    private static bool IsReferenceTypeWithValueObjectProperties(Type propertyType)
    {
        if (propertyType.IsPrimitive || propertyType.IsEnum || propertyType == typeof(string) ||
            propertyType == typeof(decimal) || propertyType == typeof(DateOnly) || propertyType == typeof(TimeOnly) ||
            propertyType == typeof(DateTime) || propertyType == typeof(DateTimeOffset) ||
            propertyType == typeof(TimeSpan) || propertyType == typeof(Guid)) return false;

        if (typeof(IEnumerable<>).IsAssignableFrom(propertyType) && propertyType != typeof(string)) return false;

        if (Nullable.GetUnderlyingType(propertyType) is not null)
            return IsReferenceTypeWithValueObjectProperties(Nullable.GetUnderlyingType(propertyType)!);


        return HasSimpleValueObjectProperties(propertyType);
    }

    private static bool HasSimpleValueObjectProperties(Type propertyType)
    {
        var propertyTypeProperties = propertyType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var property in propertyTypeProperties)
        {
            if (GetSimpleValueObjectValueType(property.PropertyType) != null) return true;

            if (!property.PropertyType.IsPrimitive &&
                property.PropertyType != typeof(string) &&
                property.PropertyType != propertyType &&
                IsReferenceTypeWithValueObjectProperties(property.PropertyType)) return true;
        }

        return false;
    }
}