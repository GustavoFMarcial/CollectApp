using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using ClosedXML.Excel;
using CollectApp.Dtos;
using CollectApp.Repositories;
using CollectApp.ViewModels;

namespace CollectApp.Services;

public class ReportService : IReportService
{
    private readonly ICollectRepository _collectRepository;

    public ReportService(ICollectRepository collectRepository)
    {
        _collectRepository = collectRepository;
    }

    public async Task<MemoryStream> GetCollects(CollectFilterViewModel filters)
    {
        var collects = await _collectRepository.ToCollectListAsync(filters);

        var items = collects.Select(c => new CollectReportDto
        {
            Id = c.Id,
            CreatedAt = c.CreatedAt,
            FullName = c.User.FullName,
            SupplierName = c.Supplier.Name,
            CollectAt = c.CollectAt,
            Product = c.Product.Name,
            Status = c.Status.ToString(),
            Volume = c.Volume,
            Weigth = c.Weight,
            Filial = c.Filial.Name
        }).ToList();

        using var workbook = new XLWorkbook();
        var worksheet = workbook.AddWorksheet("Relatório");

        var props = typeof(CollectReportDto).GetProperties();

        for (int i = 0; i < props.Length; i++)
        {
            string headerName = GetHeaderName(props[i]);
            worksheet.Cell(1, i + 1).Value = headerName;
        }

        worksheet.Cell(2, 1).InsertData(items);

        var lastColumn = props.Length;
        var lastRow = items.Count + 1;

        worksheet.Range(1, 1, lastRow, lastColumn)
                 .CreateTable("Collects");

        worksheet.Columns().AdjustToContents();

        var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Position = 0;

        return stream;
    }

    private static string GetHeaderName(PropertyInfo property)
    {
        var display = property.GetCustomAttribute<DisplayAttribute>();
        if (!string.IsNullOrWhiteSpace(display?.Name))
        {
            return display.Name!;
        }

        var displayName = property.GetCustomAttribute<DisplayNameAttribute>();
        if (!string.IsNullOrWhiteSpace(displayName?.DisplayName))
        {
            return displayName.DisplayName;
        }

        return property.Name;
    }
}
