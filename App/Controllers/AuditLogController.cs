using CollectApp.Services;
using CollectApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CollectApp.Controllers;

public class AuditLogController : Controller
{
    private readonly ILogger<AuditLogController> _logger;
    private readonly IAuditLogService _auditLogService;

    public AuditLogController(ILogger<AuditLogController> logger, IAuditLogService auditLogService)
    {
        _logger = logger;
        _auditLogService = auditLogService;
    }

    public async Task<IActionResult> GetLogs(string entityName, string entityId, int pageNum = 1)
    {
        Console.WriteLine(entityName);
        Console.WriteLine(entityId);
        PagedResultViewModel<AuditLogViewModel, object> alvm = await _auditLogService.SetPagedResultAuditLogViewModel(entityName, entityId, pageNum);

        return Json(alvm);
    }
}