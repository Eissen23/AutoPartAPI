using Infrastructure.Auth;
using Infrastructure.Persistence;
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
}
