var builder = DistributedApplication.CreateBuilder(args);

var db = builder.AddPostgres("postgres")
    .WithPgAdmin()
    .WithLifetime(ContainerLifetime.Persistent)
    .AddDatabase("fusiondb");

builder.AddProject<Projects.CodeLab_FusionCache_Api>("api")
    .WithReference(db)
    .WaitFor(db);

builder.Build().Run();