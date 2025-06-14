using System.Text.Json;
using System.Text.Json.Serialization;
using SchoolTripApi.Domain.Common.Abstractions;

namespace SchoolTripApi.WebApi.Common.JsonConverters;

public class ValueObjectJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeof(ValueObject).IsAssignableFrom(typeToConvert);
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var converterType = typeof(ValueObjectJsonConverter<>).MakeGenericType(typeToConvert);
        return (JsonConverter)Activator.CreateInstance(converterType)!;
    }
}