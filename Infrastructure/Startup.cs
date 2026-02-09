using Infrastructure.Auth;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class Startup
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Register infrastructure services here
        services
            .AddPersistence()
            .AddAuth();

        return services;
    }

    public static IApplicationBuilder UseInfrastructure (this IApplicationBuilder builder)
    {
        builder
            .UseAuthentication()
            .UseAuthorization();

        return builder;
    }
}
