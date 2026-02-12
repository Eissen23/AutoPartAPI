using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Common.Event;
using Application.Common.Interface;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using Shared.Events;

namespace Migrators.PostgreSQL;

public class PostgreSqlDbContextFactory
    : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        var connectionString = "Host=localhost;Port=5432;Database=basedb;Username=postgres;Password=postgres;";

        var services = new ServiceCollection();
        services.AddSingleton<ISerializerService, MockSerializerService>();
        services.AddSingleton<IEventPublisher, MockEventPublisher>();
        services.AddSingleton<ICurrentUser, MockCurrentUser>();

        optionsBuilder.UseNpgsql(connectionString);
        var serviceProvider = services.BuildServiceProvider();

        return new ApplicationDbContext(
            optionsBuilder.Options,
            serviceProvider.GetRequiredService<ISerializerService>(),
            serviceProvider.GetRequiredService<ICurrentUser>(),
            serviceProvider.GetRequiredService<IEventPublisher>()
        );
    }
}

public class MockSerializerService : ISerializerService
{
    public string Serialize<T>(T obj) => string.Empty;
    public T Deserialize<T>(string serialized) => default;

    public string Serialize<T>(T obj, Type type) => string.Empty;
}

public class MockEventPublisher : IEventPublisher
{
    public Task PublishAsync(IEvent @event) => Task.CompletedTask;
}

public class MockCurrentUser : ICurrentUser
{
    public string Name => "System";

    public IEnumerable<Claim> GetUserClaims() => new List<Claim>();

    public string GetUserEmail() => "system123@gmail.com";

    public Guid GetUserId() => Guid.NewGuid();

    public bool IsAuthenticated() => true;
}