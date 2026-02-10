using Application;
using Host.Configurations;
using Infrastructure;
using Infrastructure.Logging.Serilog;
using Serilog;

Log.Information("Starting Autopart Manager API...");

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddControllers();
    builder.AddConfigurations().RegisterSerilog();
    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddOpenApi();
    builder.Services.AddInfrastructure();
    builder.Services.AddApplication();
    
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

        app.UseHttpsRedirection();

        app.UseInfrastructure();

        app.MapControllers();



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

