using CodeLab.FusionCache.Api.Contracts;

namespace CodeLab.FusionCache.Api.Repository;

public interface ITodoRepository
{
    Task<TodoDto> CreateTodoAsync(CreateTodoDto todo, CancellationToken cancellationToken);

    Task<List<TodoDto>> GetAllAsync(CancellationToken cancellationToken);

    Task<TodoDto> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<bool> DeleteTodoAsync(Guid id, CancellationToken cancellationToken);
}