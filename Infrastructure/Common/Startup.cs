using Application.Common.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Common;

internal static class Startup
{
    public static IServiceCollection AddServices(this IServiceCollection services) =>
        services
            .AddServices(typeof(ITransientService), ServiceLifetime.Transient)
            .AddServices(typeof(IScopedService), ServiceLifetime.Scoped);


    /// <summary>
    /// Performs reflection-based automatic service discovery:
    ///
    /// Scans all assemblies in the current AppDomain
    ///	Finds all non-abstract classes implementing the specified interfaceType
    ///	Extracts the first matching interface from each class
    ///	Filters out null services and validates they match the interface type
    ///	Delegates actual registration to the third method for each discovered service
    /// </summary>
    /// <param name="services"></param>
    /// <param name="interfaceType"></param>
    /// <param name="lifetime"></param>
    /// <returns></returns>
    internal static IServiceCollection AddServices(this IServiceCollection services, Type interfaceType, ServiceLifetime lifetime)
    {
        var interfaceTypes =
            AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(t => interfaceType.IsAssignableFrom(t)
                            && t.IsClass && !t.IsAbstract)
                .Select(t => new
                {
                    Service = t.GetInterfaces().FirstOrDefault(),
                    Implementation = t
                })
                .Where(t => t.Service is not null
                            && interfaceType.IsAssignableFrom(t.Service));

        foreach (var type in interfaceTypes)
        {
            services.AddService(type.Service!, type.Implementation, lifetime);
        }

        return services;
    }


    /// <summary>
    /// Role: The low-level registration handler that:
    ///
    /// Performs the actual DI container registration
    ///	Uses a switch expression to route to the correct lifetime method(AddTransient, AddScoped, or AddSingleton)
    ///	Throws an exception for invalid lifetimes
    /// </summary>
    /// <param name="services"></param>
    /// <param name="serviceType"></param>
    /// <param name="implementationType"></param>
    /// <param name="lifetime"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    internal static IServiceCollection AddService(this IServiceCollection services, Type serviceType, Type implementationType, ServiceLifetime lifetime) =>
        lifetime switch
        {
            ServiceLifetime.Transient => services.AddTransient(serviceType, implementationType),
            ServiceLifetime.Scoped => services.AddScoped(serviceType, implementationType),
            ServiceLifetime.Singleton => services.AddSingleton(serviceType, implementationType),
            _ => throw new ArgumentException("Invalid lifeTime", nameof(lifetime))
        };
}
