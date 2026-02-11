using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Elasticsearch.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace Infrastructure.OpenAPI;

internal static class Startup
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services
            .AddEndpointsApiExplorer()
            .CustomAddSwaggerGen();
        return services;
    }

    private static IServiceCollection CustomAddSwaggerGen(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "AutoPart API",
                Description = "An ASP.NET Core Web API for managing AutoPart warehouse and sales",
                Contact = new OpenApiContact
                {
                    Name = "Pham Duc Phuc",
                    Email = "phuc.ducit@gmail.com"
                }
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Input your Bearer token to access this API",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = "JWT",
            });
        });

        return services;
    }
}
