using CollectApp.Models;
using CollectApp.Repositories;
using CollectApp.ViewModels;

namespace CollectApp.Services;

public class AuditLogService : IAuditLogService
{
    private readonly IAuditLogRepository _auditLogRepository;

    public AuditLogService(IAuditLogRepository auditLogRepository)
    {
        _auditLogRepository = auditLogRepository;
    }

    public async Task<List<AuditLogViewModel>> GetLogs(string entityName, string entityId)
    {
        List<AuditLog> logs = await _auditLogRepository.GetLogs(entityName, entityId);

        List<AuditLogViewModel> lalvm = logs.Select(l => new AuditLogViewModel
        {
            Field = l.Field,
            OldValue = l.OldValue,
            NewValue = l.NewValue,
            UserName = l.UserName,
            ChangedAt = TimeZoneInfo.ConvertTimeFromUtc(l.ChangedAt,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToString("dd/MM/yyyy HH:mm"),
        }).ToList();

        return lalvm;
    }
}