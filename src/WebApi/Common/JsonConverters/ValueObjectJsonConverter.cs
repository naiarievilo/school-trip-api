using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.Exceptions;

namespace SchoolTripApi.WebApi.Common.JsonConverters;

public class ValueObjectJsonConverter<T> : JsonConverter<T> where T : ValueObject
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return HasSingleProperty(typeToConvert)
            ? DeserializeSingleProperty(ref reader, typeToConvert, options)
            : DeserializeMultipleProperties(ref reader, typeToConvert, options);
    }

    public override void Write(Utf8JsonWriter writer, T? value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        if (HasSingleProperty(value.GetType()))
            SerializeSingleProperty(writer, value, options);
        else
            SerializeMultipleProperties(writer, value, options);
    }

    private void SerializeSingleProperty(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        var property = value.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .First(p => p.CanRead && p.GetIndexParameters().Length == 0);

        var propertyValue = property.GetValue(value);
        JsonSerializer.Serialize(writer, propertyValue, options);
    }

    private void SerializeMultipleProperties(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        var properties = value.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanRead && p.GetIndexParameters().Length == 0);

        foreach (var property in properties)
        {
            var propertyName = property.Name;
            var propertyValue = property.GetValue(value);

            writer.WritePropertyName(propertyName);
            JsonSerializer.Serialize(writer, propertyValue, options);
        }

        writer.WriteEndObject();
    }

    private bool HasSingleProperty(Type type)
    {
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanRead && p.GetIndexParameters().Length == 0)
            .ToArray();

        return properties.Length == 1;
    }

    private T DeserializeSingleProperty(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Get property and its value
        var property = typeToConvert.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .First(p => p.CanRead && p.GetIndexParameters().Length == 0);
        var value = JsonSerializer.Deserialize(ref reader, property.PropertyType, options);

        try
        {
            // Try to find a factory method that accepts the property as a parameter
            var factoryMethod = typeToConvert.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .FirstOrDefault(fm =>
                    (fm.Name.Equals("Create") || fm.Name.Equals("From") || fm.Name.Equals("New")) &&
                    fm.GetParameters().Length == 1 &&
                    fm.GetParameters()[0].ParameterType == property.PropertyType &&
                    fm.ReturnType == typeToConvert);
            if (factoryMethod is not null) return (T)factoryMethod.Invoke(null, [value])!;

            // Try to find a constructor that accepts the property as a parameter
            var constructor = typeToConvert.GetConstructors()
                .FirstOrDefault(c =>
                    c.GetParameters().Length == 1 && c.GetParameters()[0].ParameterType == property.PropertyType);
            if (constructor is not null) return (T)constructor.Invoke([value]);

            throw new JsonException(
                $"Unable to deserialize {typeToConvert.Name}: no suitable factory method or constructor found.");
        }
        catch (TargetInvocationException ex) when (ex.InnerException is ValueObjectException valueObjectException)
        {
            throw valueObjectException.WithPropertyContext(typeToConvert.Name);
        }
    }

    private T DeserializeMultipleProperties(ref Utf8JsonReader reader, Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException($"Expected StartObject token for {typeToConvert.Name}.");

        // Get properties and their values
        var properties = new Dictionary<string, object?>();
        var propertiesInfo = typeToConvert.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanRead && p.GetIndexParameters().Length == 0)
            .ToDictionary(p => p.Name, p => p, StringComparer.OrdinalIgnoreCase);

        var currentPropertyName = string.Empty;
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject) break;
            if (reader.TokenType != JsonTokenType.PropertyName)
                throw new JsonException($"Expected PropertyName token for {typeToConvert.Name}.");

            var propertyName = reader.GetString()!;
            currentPropertyName = propertyName;
            reader.Read();

            if (propertiesInfo.TryGetValue(propertyName, out var propertyInfo))
            {
                var value = JsonSerializer.Deserialize(ref reader, propertyInfo.PropertyType, options);
                properties.Add(propertyName, value!);
            }
            else
            {
                reader.Skip();
            }
        }

        try
        {
            // Try to find a factory method that accepts the properties as parameters
            var factoryMethod = FindMatchingFactoryMethod(typeToConvert, properties);
            if (factoryMethod is not null)
            {
                var parameters = factoryMethod.GetParameters();
                var args = new object?[parameters.Length];

                for (var i = 0; i < parameters.Length; i++)
                {
                    var paramName = parameters[i].Name;
                    var matchingProperty = properties.Keys
                        .FirstOrDefault(k => string.Equals(k, paramName, StringComparison.OrdinalIgnoreCase));

                    if (matchingProperty != null)
                        args[i] = properties[matchingProperty];
                    else if (parameters[i].HasDefaultValue)
                        args[i] = parameters[i].DefaultValue;
                    else
                        throw new JsonException(
                            $"No value found for required parameter '{paramName}' in factory method {factoryMethod.Name} for {typeToConvert.Name}.");
                }

                return (T)factoryMethod.Invoke(null, args)!;
            }

            // Try to find a constructor that accepts the property as a parameter
            var constructor = FindMatchingConstructor(typeToConvert, properties);
            if (constructor is not null)
            {
                var parameters = constructor.GetParameters();
                var args = new object?[parameters.Length];

                for (var i = 0; i < parameters.Length; i++)
                {
                    var paramName = parameters[i].Name;
                    var matchingProperty = properties.Keys
                        .FirstOrDefault(k => string.Equals(k, paramName, StringComparison.OrdinalIgnoreCase));

                    if (matchingProperty != null)
                        args[i] = properties[matchingProperty];
                    else if (parameters[i].HasDefaultValue)
                        args[i] = parameters[i].DefaultValue;
                    else
                        throw new JsonException(
                            $"No value found for required parameter '{paramName}' in {typeToConvert.Name}");
                }

                return (T)constructor.Invoke(args);
            }

            throw new JsonException(
                $"Unable to deserialize {typeToConvert.Name}: no suitable factory method or constructor found.");
        }
        catch (TargetInvocationException ex) when (ex.InnerException is ValueObjectException valueObjectException)
        {
            throw valueObjectException.WithPropertyContext(currentPropertyName);
        }
    }

    private MethodInfo? FindMatchingFactoryMethod(Type type, Dictionary<string, object?> properties)
    {
        IEnumerable<MethodInfo?> factoryMethods = type.GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(fm =>
                (fm.Name.Equals("Create") || fm.Name.Equals("From") || fm.Name.Equals("Of") ||
                 fm.Name.Equals("New")) && fm.ReturnType == type);

        return factoryMethods.FirstOrDefault(fm => FactoryMethodMatches(fm, properties));
    }

    private bool FactoryMethodMatches(MethodInfo? factoryMethod, Dictionary<string, object?> properties)
    {
        if (factoryMethod is null) return false;

        var parameters = factoryMethod.GetParameters();
        return (from parameter in parameters
                where !parameter.HasDefaultValue
                select properties.Keys.Any(k =>
                    string.Equals(k, parameter.Name, StringComparison.InvariantCultureIgnoreCase)))
            .All(hasMatchingProperty => hasMatchingProperty);
    }

    private ConstructorInfo? FindMatchingConstructor(Type type, Dictionary<string, object?> properties)
    {
        return type.GetConstructors()
            .OrderByDescending(c => c.GetParameters().Length)
            .FirstOrDefault(c => ConstructorMatches(c, properties));
    }

    private static bool ConstructorMatches(ConstructorInfo? constructor, Dictionary<string, object?> properties)
    {
        if (constructor is null) return false;

        var parameters = constructor.GetParameters();
        return (from parameter in parameters
                where !parameter.HasDefaultValue
                select properties.Keys.Any(k =>
                    string.Equals(k, parameter.Name, StringComparison.InvariantCultureIgnoreCase)))
            .All(hasMatchingProperty => hasMatchingProperty);
    }
}