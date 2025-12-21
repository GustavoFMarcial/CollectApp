using CollectApp.Models;

namespace CollectApp.Services;

public interface IAuditLogTranslatorService
{
    public Task<bool> TranslateAsync<T>(AuditLog log,string fieldName,Func<int, Task<T?>> getById,Func<T, string> getName) where T : class;
}