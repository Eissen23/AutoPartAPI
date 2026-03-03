using System;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Seeding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Extensions;

public static class SeedingExtensions
{
    /// <summary>
    /// Registers the database seeder in the dependency injection container.
    /// </summary>
    public static IServiceCollection AddDatabaseSeeder(this IServiceCollection services)
    {
        services.AddScoped<ApplicationDbContextSeeder>();
        return services;
    }

    /// <summary>
    /// Seeds the database on application startup.
    /// Call this in Program.cs after building the host.
    /// </summary>
    public static async Task SeedDatabaseAsync(this IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            var seeder = scope.ServiceProvider.GetRequiredService<ApplicationDbContextSeeder>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContextSeeder>>();

            try
            {
                await seeder.SeedAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }
    }
}
