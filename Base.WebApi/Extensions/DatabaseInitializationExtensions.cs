using Base.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Host.Extensions;

/// <summary>
/// Extension methods for initializing the database with migrations and seeding.
/// </summary>
public static class DatabaseInitializationExtensions
{
    private static readonly Serilog.ILogger Logger = Log.ForContext(typeof(DatabaseInitializationExtensions));

    /// <summary>
    /// Applies pending migrations and seeds the database on application startup.
    /// This should be called after building the IHost but before running the application.
    /// </summary>
    public static async Task InitializeDatabaseAsync(this IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            try
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                Logger.Information("Applying pending migrations to the database...");

                // Apply any pending migrations
                var pendingMigrations = (await dbContext.Database.GetPendingMigrationsAsync()).ToList();
                if (pendingMigrations.Count > 0)
                {
                    Logger.Information("Found {MigrationCount} pending migration(s)", pendingMigrations.Count);
                    foreach (var migration in pendingMigrations)
                    {
                        Logger.Information("Applying migration: {MigrationName}", migration);
                    }
                    await dbContext.Database.MigrateAsync();
                    Logger.Information("Migrations applied successfully");
                }
                else
                {
                    Logger.Information("No pending migrations found");
                }

                Logger.Information("Database initialization completed successfully");
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex, "An error occurred while initializing the database");
                throw;
            }
        }
    }
}
