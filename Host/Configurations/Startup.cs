namespace Host.Configurations;

public static class Startup
{
    internal static WebApplicationBuilder AddConfigurations(this WebApplicationBuilder builder)
    {
        const string configPath = "Configurations";
        var env = builder.Environment;

        builder.Configuration
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"{configPath}/logger.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"{configPath}/logger.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"{configPath}/database.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"{configPath}/database.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

        return builder;
    }
}
