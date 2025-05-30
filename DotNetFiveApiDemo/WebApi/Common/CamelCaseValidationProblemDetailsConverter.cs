using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetFiveApiDemo.WebApi.Common
{
    public class CamelCaseValidationProblemDetailsConverter : JsonConverter<ValidationProblemDetails>
    {
        public override ValidationProblemDetails Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            // Should not be used to read ValidationProblemDetails from the client
            throw new InvalidOperationException();
        }

        public override void Write(Utf8JsonWriter writer, ValidationProblemDetails value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            // Write standard properties
            writer.WriteString("type", value.Type);
            writer.WriteString("title", value.Title);
            writer.WriteNumber("status", value.Status ?? 0);
            writer.WriteString("traceId",
                value.Extensions.TryGetValue("traceId", out var extension) ? extension.ToString() : null);

            // Write errors with camelCase keys
            writer.WriteStartObject("errors");
            foreach (var error in value.Errors)
            {
                var camelCaseKey = JsonNamingPolicy.CamelCase.ConvertName(error.Key);
                writer.WriteStartArray(camelCaseKey);
                foreach (var errorMessage in error.Value) writer.WriteStringValue(errorMessage);
                writer.WriteEndArray();
            }

            writer.WriteEndObject();

            writer.WriteEndObject();
        }
    }
}