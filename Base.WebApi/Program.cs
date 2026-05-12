using Application;
using Host.Configurations;
using Host.Extensions;
using Infrastructure;
using Base.Infrastructure.Extensions;
using Base.Infrastructure.Logging.Serilog;
using Microsoft.AspNetCore.Authentication;
using Serilog;

Log.Information("Starting Autopart Manager API...");

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddControllers();
    builder.AddConfigurations().RegisterSerilog();

    builder.Services.AddOpenApi();
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddApplication();
    builder.Services.AddDatabaseSeeder();
    
    try
    {
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();

        //app.UseHttpsRedirection();

        app.UseInfrastructure(); 

        app.MapControllers();

        // Initialize database: apply migrations and seed
        await app.InitializeDatabaseAsync();
        await app.SeedDatabaseAsync();

        app.Run();
    }
    catch (Exception buildEx)
    {
        Log.Fatal(buildEx, "Error during application build");
        if (buildEx is AggregateException aggEx)
        {
            foreach (var innerEx in aggEx.InnerExceptions)
            {
                Log.Fatal(innerEx, "Inner exception: {Message}", innerEx.Message);
            }
        }
        throw;
    }

}
catch (Exception ex) when (!ex.GetType().Name.Equals("StopTheHostException", StringComparison.Ordinal))
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Server Shutting down...");
    Log.CloseAndFlush();
}

