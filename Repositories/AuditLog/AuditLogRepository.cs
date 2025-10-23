using CollectApp.Data;

namespace CollectApp.Repositories;

public class AuditLogRepository : IAuditLogRepository
{
    private readonly CollectAppContext _context;

    public AuditLogRepository( CollectAppContext context)
    {
        _context = context;
    }
}