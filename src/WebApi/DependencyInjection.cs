using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using SchoolTripApi.WebApi.Accounts.Filters;
using SchoolTripApi.WebApi.Common;
using SchoolTripApi.WebApi.Common.JsonConverters;

namespace SchoolTripApi.WebApi;

public static class DependencyInjection
{
    public static void AddWebApiConfiguration(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMediator(options => options.ServiceLifetime = ServiceLifetime.Scoped);
        services.AddWebApiSwaggerGen();
        services.AddExceptionFilters();
        services.AddControllers(opts => opts.Filters.Add<ValueObjectExceptionFilter>())
            .AddJsonOptions(opts =>
            {
                opts.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
                opts.JsonSerializerOptions.Converters.Add(new SnakeCaseValidationProblemDetailsConverter());
                opts.JsonSerializerOptions.Converters.Add(new ValueObjectJsonConverterFactory());
            });
    }

    private static void AddWebApiSwaggerGen(this IServiceCollection services)
    {
        services.AddSwaggerGen(opts =>
        {
            opts.SwaggerDoc("v1", new OpenApiInfo { Title = "School Trip API", Version = "v1" });

            var securityScheme = new OpenApiSecurityScheme
            {
                Name = HeaderNames.Authorization,
                Description = "JWT Authorization header: \"Authorization: Bearer {token}\"",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };
            opts.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);
            opts.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { securityScheme, new List<string>() }
            });

            opts.EnableAnnotations();
            opts.SchemaFilter<ValueObjectSchemaFilter>();
        });
    }

    private static void AddExceptionFilters(this IServiceCollection services)
    {
        services.AddScoped<ValueObjectExceptionFilter>();
    }
}