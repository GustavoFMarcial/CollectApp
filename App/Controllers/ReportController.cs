using CollectApp.Services;
using CollectApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CollectApp.Controllers;

public class ReportController : Controller
{
    private readonly IReportService _reportService;

    public ReportController(IReportService reportService)
    {
        _reportService = reportService;
    }

    public async Task<IActionResult> GetXLSXReport([FromQuery] CollectFilterViewModel filters)
    {
        var stream = await _reportService.GetCollects(filters);

        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "relatório.xlsx");
    }
}