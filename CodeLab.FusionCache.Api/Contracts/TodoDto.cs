namespace CodeLab.FusionCache.Api.Contracts;

public record TodoDto(Guid Id, string Title, string Description, bool IsCompleted);