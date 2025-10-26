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

    public async Task<IActionResult> GetLogs(string entityName, string entityId)
    {
        Console.WriteLine(entityName);
        Console.WriteLine(entityId);
        List<AuditLogViewModel> alvm = await _auditLogService.GetLogs(entityName, entityId);
        return Json(alvm);
    }
}