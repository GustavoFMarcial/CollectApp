using CollectApp.Models;

namespace CollectApp.Repositories;

public interface IAuditLogRepository
{
    public Task<(List<AuditLog> items, int totalCount)> ToLogListAsync(string entityName, string entityId, int pageNum = 1, int pageSize = 10);
}