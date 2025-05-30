using System.Collections.Generic;
using System.Text.Json;
using DotNetFiveApiDemo.WebApi.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;

namespace DotNetFiveApiDemo.WebApi
{
    public static class WebApiExtensions
    {
        public static IServiceCollection AddWebApiConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddWebApiSwaggerGen();
            services.AddControllers()
                .AddJsonOptions(opts =>
                {
                    opts.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    opts.JsonSerializerOptions.Converters.Add(new CamelCaseValidationProblemDetailsConverter());
                });

            return services;
        }

        private static void AddWebApiSwaggerGen(this IServiceCollection services)
        {
            services.AddSwaggerGen(opts =>
            {
                opts.SwaggerDoc("v1", new OpenApiInfo { Title = "DotNetFiveApiDemo", Version = "v1" });

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
            });
        }
    }
}