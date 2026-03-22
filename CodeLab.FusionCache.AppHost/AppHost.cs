var builder = DistributedApplication.CreateBuilder(args);

var db = builder.AddPostgres("pgsql")
    .WithPgAdmin()
    .WithLifetime(ContainerLifetime.Persistent)
    .AddDatabase("fusiondb");

builder.AddProject<Projects.CodeLab_FusionCache_Api>("codelab-fusioncache-api")
    .WithReference(db)
    .WaitFor(db);

builder.Build().Run();