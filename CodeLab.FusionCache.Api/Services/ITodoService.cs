using CodeLab.FusionCache.Api.Contracts;

namespace CodeLab.FusionCache.Api.Services;

public interface ITodoService
{
    Task<TodoDto> CreateTodoAsync(CreateTodoDto todo, CancellationToken cancellationToken);

    Task<List<TodoDto>> GetAllAsync(CancellationToken cancellationToken);

    Task<TodoDto> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<bool> DeleteTodoAsync(Guid id, CancellationToken cancellationToken);
}