namespace CodeLab.FusionCache.Api.Helpers;

internal static class CacheKeysHelper
{
    internal static string GetTodoKey(Guid id) => $"todos_{id}";
}