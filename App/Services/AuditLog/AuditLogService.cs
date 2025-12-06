using CollectApp.Helpers;
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

    public async Task<PagedResultViewModel<AuditLogViewModel, object>> SetPagedResultAuditLogViewModel(string entityName, string entityId, int pageNum = 1, int pageSize = 10)
    {
        (List<AuditLog> items, int totalCount) logs = await _auditLogRepository.ToLogListAsync(entityName, entityId, pageNum);

        foreach (var log in logs.items)
        {
            log.Field = LogFieldTranslations.Translate(entityName, log.Field);

            if (log.OldValue == "PendenteAprovar" || log.OldValue == "PendenteColetar")
            {
                log.OldValue = LogStatusValue.LogStatusValueSpaceBetween(log.OldValue);
            }

            if (log.NewValue == "PendenteAprovar" || log.NewValue == "PendenteColetar")
            {
                log.NewValue = LogStatusValue.LogStatusValueSpaceBetween(log.NewValue);
            }
        }

        List<AuditLogViewModel> lalvm = logs.items.Select(l => new AuditLogViewModel
        {
            Field = l.Field,
            OldValue = l.OldValue,
            NewValue = l.NewValue,
            UserName = l.UserName,
            ChangedAt = TimeZoneInfo.ConvertTimeFromUtc(l.ChangedAt, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToString("dd/MM/yyyy HH:mm"),
        }).ToList();

        PagedResultViewModel<AuditLogViewModel, object> prvm = new PagedResultViewModel<AuditLogViewModel, object>
        {
            Items = lalvm,
            TotalPages = (int)Math.Ceiling(logs.totalCount / (double)pageSize),
            PageNum = pageNum,
        };

        return prvm;
    }
}