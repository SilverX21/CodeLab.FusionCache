using CodeLab.FusionCache.Api.Contracts;
using CodeLab.FusionCache.Api.Data;
using CodeLab.FusionCache.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace CodeLab.FusionCache.Api.Repository;

public class TodoRepository(AppDbContext dbContext, ILogger<TodoRepository> logger) : ITodoRepository
{
    public async Task<TodoDto> CreateTodoAsync(CreateTodoDto todo, CancellationToken cancellationToken)
    {
        var entity = new Todo
        {
            Id = Guid.CreateVersion7(),
            Title = todo.Title,
            Description = todo.Description,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };

        await dbContext.Todos.AddAsync(entity, cancellationToken);
        var result = await dbContext.SaveChangesAsync(cancellationToken);

        if (result == 0)
        {
            logger.LogError("Failed to create todo item: {Title}", todo.Title);
            return null;
        }

        return new TodoDto(entity.Id, entity.Title, entity.Description, entity.IsCompleted);
    }

    public async Task<List<TodoDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Todos
            .Select(x => new TodoDto(x.Id, x.Title, x.Description, x.IsCompleted))
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}