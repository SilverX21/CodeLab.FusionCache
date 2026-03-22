using CodeLab.FusionCache.Api.Contracts;

namespace CodeLab.FusionCache.Api.Repository;

public interface ITodoRepository
{
    Task<TodoDto> CreateTodoAsync(CreateTodoDto todo, CancellationToken cancellationToken);

    Task<List<TodoDto>> GetAllAsync(CancellationToken cancellationToken);
}