using Asp.Versioning;
using Infrastructure.Auth;
using Infrastructure.Common;
using Infrastructure.Identities;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class Startup
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {

        services
            .AddApiVersioning()
            .AddSwaggerGen()
            .AddPersistence()
            .AddAuth()
            .AddIdentities()
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
            .UseAuthentication()
            .UseAuthorization();

        return builder;
    }
}
