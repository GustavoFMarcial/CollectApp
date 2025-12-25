using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CollectApp.Dtos;

public class CollectReportDto
{
    public int Id { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    [DisplayName("Criação coleta")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [DisplayName("Usuário")]
    public string? FullName { get; set; }

    [DisplayName("Fornecedor")]
    public string? SupplierName { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    [DisplayName("Data coleta")]
    public DateTime CollectAt { get; set; }

    [DisplayName("Produto")]
    public string? Product { get; set; }

    public string? Status { get; set; }

    public int? Volume { get; set; }

    [DisplayName("Peso")]
    public int? Weigth { get; set; }

    [DisplayName("Loja")]
    public string? Filial { get; set; }
}