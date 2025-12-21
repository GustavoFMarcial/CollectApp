using CollectApp.Helpers;
using CollectApp.Models;
using CollectApp.Repositories;
using CollectApp.ViewModels;

namespace CollectApp.Services;

public class AuditLogService : IAuditLogService
{
    private readonly IAuditLogRepository _auditLogRepository;
    private readonly ISupplierRepository _supplierRepository;
    private readonly IProductRepository _productRepository;
    private readonly IFilialRepository _filialRepository;
    private readonly IAuditLogTranslatorService _auditLogTranslatorService;

    public AuditLogService(IAuditLogRepository auditLogRepository, ISupplierRepository supplierRepository, IProductRepository productRepository, IFilialRepository filialRepository, IAuditLogTranslatorService auditLogTranslatorService)
    {
        _auditLogRepository = auditLogRepository;
        _supplierRepository = supplierRepository;
        _productRepository = productRepository;
        _filialRepository = filialRepository;
        _auditLogTranslatorService = auditLogTranslatorService;
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

        foreach (var log in logs.items)
        {
            if (await _auditLogTranslatorService.TranslateAsync(log, "SupplierId", id => _supplierRepository.GetSupplierByIdAsync(id), s => s.Name))
            {
                continue;
            }

            if (await _auditLogTranslatorService.TranslateAsync( log, "ProductId", id => _productRepository.GetProductByIdAsync(id), p => p.Name))
            {
                continue;
            }

            if (await _auditLogTranslatorService.TranslateAsync( log, "FilialId", id => _filialRepository.GetFilialByIdAsync(id), f => f.Name))
            {
                continue;
            }
        }

        List<AuditLogViewModel> lalvm = logs.items.Select(l =>
        {
            var utcDate = DateTime.SpecifyKind(l.ChangedAt, DateTimeKind.Utc);
            var localDate = TimeZoneInfo.ConvertTimeFromUtc(utcDate, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));

            return new AuditLogViewModel
            {
                Field = l.Field,
                OldValue = l.OldValue,
                NewValue = l.NewValue,
                UserName = l.UserName,
                ChangedAt = localDate.ToString("dd/MM/yyyy HH:mm")
            };
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