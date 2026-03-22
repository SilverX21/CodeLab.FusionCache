using CodeLab.FusionCache.Api.Contracts;
using CodeLab.FusionCache.Api.Data;
using CodeLab.FusionCache.Api.Exceptions;
using CodeLab.FusionCache.Api.Repository;
using CodeLab.FusionCache.Api.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using ZiggyCreatures.Caching.Fusion;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ValidationExceptionHandler>();

builder.Services.AddOpenApi();
var connectionString = builder.Configuration.GetConnectionString("fusiondb");
builder.AddNpgsqlDbContext<AppDbContext>("fusiondb");

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddFusionCache()
    .WithOptions(options =>
    {
        options.DefaultEntryOptions.Duration = TimeSpan.FromMinutes(5);
        options.DefaultEntryOptions.DistributedCacheDuration = TimeSpan.FromMinutes(5);
    })
    .WithDefaultEntryOptions(options =>
    {
        options.Duration = TimeSpan.FromMinutes(5);
        options.DistributedCacheDuration = TimeSpan.FromMinutes(5);
    });

builder.Services.AddScoped<ITodoService, TodoService>();
builder.Services.AddScoped<ITodoRepository, TodoRepository>();

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}

app.UseExceptionHandler();
app.UseHttpsRedirection();

app.MapPost("/todos", async (
    CreateTodoDto request,
    [FromServices] ITodoService service,
    [FromServices] IValidator<CreateTodoDto> validator,
    CancellationToken cancellationToken) =>
{
    await validator.ValidateAndThrowAsync(request, cancellationToken);

    var response = await service.CreateTodoAsync(request, cancellationToken);

    return response == null ? Results.Problem("An error occurred while creating the todo.")
        : Results.Created($"/todos/{response.Id}", response);
})
.WithName("CreateTodo")
.WithTags("Todos");

app.MapGet("/todos", async ([FromServices] ITodoService service, CancellationToken cancellationToken) =>
{
    var todos = await service.GetAllAsync(cancellationToken);

    return todos == null ? Results.Problem("An error occurred while retrieving the todos.")
        : Results.Ok(todos);
})
.WithName("GetTodos")
.WithTags("Todos");

app.Run();