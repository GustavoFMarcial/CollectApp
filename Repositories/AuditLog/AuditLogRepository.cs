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

    public async Task<List<AuditLog>> GetLogs(string entityName, int entityId)
    {
        IQueryable<AuditLog> query = _context.AuditLogs.AsQueryable();

        query = query
            .OrderByDescending(a => a.ChangedAt)
            .Where(a => a.EntityName == entityName && a.EntityId == entityId);

        List<AuditLog> logs = await query.ToListAsync();

        return logs;
    }
}