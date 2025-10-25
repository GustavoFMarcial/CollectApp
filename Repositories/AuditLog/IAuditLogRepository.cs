using CollectApp.Models;

namespace CollectApp.Repositories;

public interface IAuditLogRepository
{
    public Task<List<AuditLog>> GetLogs(string entityName, int entityId);
}