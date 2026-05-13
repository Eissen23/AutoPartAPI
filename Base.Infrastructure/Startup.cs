using System.Reflection;
using Asp.Versioning;
using Base.Infrastructure.Auth;
using Base.Infrastructure.Common;
using Base.Infrastructure.Cors;
using Base.Infrastructure.FileStorage;
using Base.Infrastructure.Identities;
using Base.Infrastructure.Middlewares;
using Base.Infrastructure.OpenAPI;
using Base.Infrastructure.Persistence;
using Base.Infrastructure.Validator;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class Startup
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        var applicationAssembly = typeof(Application.Startup).GetTypeInfo().Assembly;

        services
            .AddApiVersioning()
            .AddCorsPolicy(config)
            .AddAuth()
            .AddSwagger()
            .AddPersistence()
            .AddBehaviours(applicationAssembly)
            .AddExceptionMiddleware()
            .AddRouting(options => options.LowercaseUrls = true)
            .AddStorage()
            .AddServices();

        return services;
    }

    private static IServiceCollection AddApiVersioning(this IServiceCollection services) =>
        services.AddApiVersioning(option =>
        {
            option.DefaultApiVersion = new ApiVersion(1, 0);
            option.AssumeDefaultVersionWhenUnspecified = true;
            option.ReportApiVersions = true;
        })
        .AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        })
        .Services;


    public static IApplicationBuilder UseInfrastructure (this IApplicationBuilder builder)
    {
        builder
            .UseCorsPolicy()
            .UseExceptionMiddleware()
            .UseAuthentication()
            .UseAuthorization()
            .UseCurrentUser();

        return builder;
    }
}
