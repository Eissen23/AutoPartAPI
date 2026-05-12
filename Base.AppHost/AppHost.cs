var builder = DistributedApplication.CreateBuilder(args);

var postgresPort = 1522;
var postgresPortBuilder = builder.AddParameter("postgres-port", $"{postgresPort}");
var postgresPassword = builder.AddParameter("postgres-password", secret: true);
string dbName = "BaseAutopart";
string dbPass = "123123";


var databaseName = builder.AddParameter("database-name", $"{dbName}");

var postgres = builder.AddPostgres("postgres-database", port: postgresPort, password: postgresPassword)
    .WithDataVolume("base-postgres-database")
    .WithLifetime(ContainerLifetime.Session);

var autopartDb = postgres.AddDatabase(dbName, "FREEDB1");

var host = builder.AddProject<Projects.Base_WebApi>("webapi")
    .WithHttpEndpoint(port: 5026, name: "http")
    .WithHttpsEndpoint(port: 7039, name: "https")
    .WithEnvironment("env", "aspire")
    .WithEnvironment("postgres-password", postgresPassword)
    .WithEnvironment("postgres-port", postgresPortBuilder)
    .WithEnvironment("postgres-database", dbName)
    .WithEnvironment("postgres-db-password", dbPass)
    .WithReference(autopartDb)
    .WithExternalHttpEndpoints()
    .WaitFor(autopartDb);

builder.Build().Run();
