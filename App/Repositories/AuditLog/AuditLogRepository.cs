using CollectApp.Data;
using CollectApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CollectApp.Repositories;

public class AuditLogRepository : IAuditLogRepository
{
    private readonly CollectAppContext _context;

    public AuditLogRepository(CollectAppContext context)
    {
        _context = context;
    }

    public async Task<(List<AuditLog> items, int totalCount)> ToLogListAsync(string entityName, string entityId, int pageNum = 1, int pageSize = 10)
    {
        IQueryable<AuditLog> query = _context.AuditLogs.AsQueryable();

        query = query.Where(a => a.EntityName == entityName && a.EntityId == entityId);

        int totalCount = await query.CountAsync();

        List<AuditLog> items = await query
            .OrderByDescending(a => a.ChangedAt)
            .Skip((pageNum - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}