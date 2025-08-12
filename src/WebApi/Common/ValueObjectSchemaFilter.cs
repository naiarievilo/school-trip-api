using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SchoolTripApi.WebApi.Common;

public class ValueObjectSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        // Check if the type is a ValueObject
        if (IsValueObject(context.Type))
        {
            // Get the Value property (assuming single-property value objects)
            var valueProperty = GetValueProperty(context.Type);
            if (valueProperty != null)
            {
                // Get the schema for the underlying type
                var underlyingSchema = context.SchemaGenerator.GenerateSchema(
                    valueProperty.PropertyType,
                    context.SchemaRepository
                );

                // Replace the complex object schema with the simple underlying type schema
                schema.Type = underlyingSchema.Type;
                schema.Format = underlyingSchema.Format;
                schema.Properties?.Clear();
                schema.Required?.Clear();

                // Copy other relevant properties from the underlying schema
                schema.Example = underlyingSchema.Example;
                schema.Default = underlyingSchema.Default;
                schema.Pattern = underlyingSchema.Pattern;
                schema.MinLength = underlyingSchema.MinLength;
                schema.MaxLength = underlyingSchema.MaxLength;
                schema.Minimum = underlyingSchema.Minimum;
                schema.Maximum = underlyingSchema.Maximum;

                // Add any custom validation attributes if needed
                ApplyCustomValidation(schema, context.Type);
            }
        }
    }

    private static bool IsValueObject(Type type)
    {
        // Check if the type inherits from ValueObject
        return type.BaseType != null &&
               (type.BaseType.Name == "ValueObject" ||
                IsValueObject(type.BaseType));
    }

    private static PropertyInfo? GetValueProperty(Type type)
    {
        // Look for a single public readable property (typically "Value")
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanRead && p.GetIndexParameters().Length == 0)
            .ToArray();

        return properties.Length == 1 ? properties[0] : null;
    }

    private static void ApplyCustomValidation(OpenApiSchema schema, Type valueObjectType)
    {
        // Apply custom validation based on your value object's constraints
        // This is where you can add specific validation rules for each value object

        /*
        // For Example:
        if (valueObjectType.Name == "FullName")
        {
            // Get the MaxLength constant from FullName if it exists
            var maxLengthField = valueObjectType.GetField("MaxLength", BindingFlags.Public | BindingFlags.Static);
            if (maxLengthField != null && maxLengthField.GetValue(null) is int maxLength)
            {
                schema.MaxLength = maxLength;
            }

            // Add pattern if you want to show the regex in Swagger
            schema.Pattern = @"^[A-Za-zÀ-ÿ\s\-']+$";
            schema.Example = new Microsoft.OpenApi.Any.OpenApiString("John Doe");
        }

        // Add more value object specific validations here
        // Example for other value objects:

        if (valueObjectType.Name == "Email")
        {
            schema.Format = "email";
            schema.Example = new Microsoft.OpenApi.Any.OpenApiString("user@example.com");
        }
        else if (valueObjectType.Name == "PhoneNumber")
        {
            schema.Pattern = @"^\+?[1-9]\d{1,14}$";
            schema.Example = new Microsoft.OpenApi.Any.OpenApiString("+1234567890");
        }
        */
    }
}