using CollectApp.ViewModels;

namespace CollectApp.Services;

public interface IAuditLogService
{
    public Task<List<AuditLogViewModel>> GetLogs(string entityName, string entityId);
}