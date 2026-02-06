using Infrastructure.Persistence;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

namespace Migrators.PostgreSQL;

public class PostgreSqlDbContextFactory
    : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        var connectionString = "Host=localhost;Port=5432;Database=basedb;Username=postgres;Password=postgres;";

        optionsBuilder.UseNpgsql(connectionString);

        return new ApplicationDbContext(
            optionsBuilder.Options
        );
    }
}