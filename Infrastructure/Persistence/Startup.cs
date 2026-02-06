using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Common;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Serilog;

namespace Infrastructure.Persistence;

public static class Startup
{
    private static readonly ILogger _logger = Log.ForContext(typeof(Startup));

    public static IServiceCollection AddPersistence(this IServiceCollection services) // Change IServiceProvider to IServiceCollection
    {

        services.AddOptions<DatabaseSettings>()
           .BindConfiguration(nameof(DatabaseSettings))
           .PostConfigure(databaseSettings =>
           {
               _logger.Information("Current DB Provider: {dbProvider}", databaseSettings.DBProvider);
               _logger.Information("Current Schema Name: {schemaName}", databaseSettings.SchemaName);

               // Initialize static schema name for configurations
               SchemaNames.Initialize(databaseSettings.SchemaName);
           })
           .ValidateDataAnnotations()
           .ValidateOnStart();

        // Register persistence services here, e.g.:
        services.AddDbContext<ApplicationDbContext>((p, m) => {
            var databaseSettings = p.GetRequiredService<IOptions<DatabaseSettings>>().Value;
            m.UseDatabase(databaseSettings.DBProvider, databaseSettings.ConnectionString);
        });

        return services;
    }

    internal static DbContextOptionsBuilder UseDatabase(this DbContextOptionsBuilder builder, string dbProvider, string connectionString)
    {
        return dbProvider.ToLowerInvariant() switch
        {
            DbProviderKeys.Npgsql => builder.UseNpgsql(connectionString, e =>
                                 e.MigrationsAssembly("Migrators.PostgreSQL")),
            DbProviderKeys.MySql => builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), e =>
                                 e.MigrationsAssembly("Migrators.MySQL")
                                  .SchemaBehavior(MySqlSchemaBehavior.Ignore)),
            _ => throw new InvalidOperationException($"DB Provider {dbProvider} is not supported."),
        };
    }
}
