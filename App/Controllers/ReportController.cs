using ClosedXML.Excel;
using CollectApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CollectApp.Controllers;

public class ReportController : Controller
{
    public async Task<IActionResult> GetXLSXReport(CollectFilterViewModel filters)
    {
        Console.WriteLine("A");
        using MemoryStream stream = new MemoryStream();
        using var workbook = new XLWorkbook();
        var worksheet = workbook.AddWorksheet("Sample Sheet");
        worksheet.Cell("A1").Value = "Do Patch";
        worksheet.Cell("A2").Value = "Do Bomba";
        workbook.SaveAs(stream);
        stream.Position = 0;

        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "relatório.xlsx");
    }
}