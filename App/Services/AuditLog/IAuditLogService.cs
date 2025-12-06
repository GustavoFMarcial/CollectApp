using CollectApp.ViewModels;

namespace CollectApp.Services;

public interface IAuditLogService
{
    public Task<PagedResultViewModel<AuditLogViewModel, object>> SetPagedResultAuditLogViewModel(string entityName, string entityId, int pageNum = 1, int pageSize = 10);
}