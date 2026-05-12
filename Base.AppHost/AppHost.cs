var builder = DistributedBase.Application.CreateBuilder(args);

builder.AddPostgres("postgres");

builder.Build().Run();
