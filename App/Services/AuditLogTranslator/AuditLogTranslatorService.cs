using CollectApp.Models;

namespace CollectApp.Services;

public class AuditLogTranslatorService : IAuditLogTranslatorService
{
    public async Task<bool> TranslateAsync<T>(AuditLog log, string fieldName, Func<int, Task<T?>> getById, Func<T, string> getName) where T : class
    {
        if (log.Field != fieldName)
            return false;

        if (!int.TryParse(log.OldValue, out var oldId) ||
            !int.TryParse(log.NewValue, out var newId))
            return true;

        var oldEntity = await getById(oldId);
        var newEntity = await getById(newId);

        if (oldEntity == null || newEntity == null)
            return true;

        log.Field = fieldName switch
        {
            "SupplierId" => "Fornecedor",
            "ProductId" => "Produto",
            "FilialId" => "Filial",
            _ => log.Field
        };

        log.OldValue = getName(oldEntity);
        log.NewValue = getName(newEntity);

        return true;
    }
}