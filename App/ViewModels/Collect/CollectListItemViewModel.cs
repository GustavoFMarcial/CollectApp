using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CollectApp.Models;

namespace CollectApp.ViewModels;

public class CollectListViewModel
{
    public int Id { get; set; }

    [DisplayName("Criação coleta")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public string UserId { get; set; } = string.Empty;

    [DisplayName("Usuário")]
    public string? FullName { get; set; }

    [DisplayName("Fornecedor")]
    public string? SupplierName { get; set; }

    [DisplayName("Data coleta")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime CollectAt { get; set; }

    [DisplayName("Produto")]
    public string? ProductDescription { get; set; }

    public int? Volume { get; set; }

    [DisplayName("Peso")]
    public int? Weigth { get; set; }

    [DisplayName("Loja")]
    public string? Filial { get; set; }

    public CollectStatus Status { get; set; }

    public ChangeCollectViewModel ChangeCollect { get; set; } = new();
}