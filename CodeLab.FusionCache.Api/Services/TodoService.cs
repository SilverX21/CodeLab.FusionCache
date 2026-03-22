using CodeLab.FusionCache.Api.Contracts;
using CodeLab.FusionCache.Api.Repository;
using ZiggyCreatures.Caching.Fusion;

namespace CodeLab.FusionCache.Api.Services;

public class TodoService(
    ITodoRepository todoRepository,
    IFusionCache cache,
    ILogger<TodoService> logger) : ITodoService
{
    public async Task<TodoDto> CreateTodoAsync(CreateTodoDto todo, CancellationToken cancellationToken)
    {
        try
        {
            var response = await todoRepository.CreateTodoAsync(todo, cancellationToken);

            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while creating a new todo.");
            return null;
        }
    }

    public async Task<List<TodoDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        try
        {
            List<TodoDto> todos = await cache.GetOrSetAsync(
                "TodosList",
                async token =>
                {
                    var response = await todoRepository.GetAllAsync(token);

                    return response;
                });

            return todos;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while retrieving all todos.");
            return null;
        }
    }
}