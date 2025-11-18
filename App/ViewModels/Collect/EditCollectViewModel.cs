using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CollectApp.ViewModels;

public class EditCollectViewModel
{
    [Required]
    public int Id { get; set; }

    [Required]
    public int SupplierId { get; set; }

    [DisplayName("Fornecedor")]
    [Required(ErrorMessage = "Campo fornecedor é obrigatório")]
    [StringLength(20, ErrorMessage = "Empresa para coleta dever ter no máximo 20 caracteres")]
    public string? Supplier { get; set; }

    [DisplayName("Data coleta")]
    [Required(ErrorMessage = "Campo data de coleta é obrigatório")]
    [DataType(DataType.Date, ErrorMessage = "Insira uma data válida")]
    public DateTime CollectAt { get; set; }

    [Required]
    public int ProductId { get; set; }

    [DisplayName("Produto")]
    [Required(ErrorMessage = "Campo fornecedor é obrigatório")]
    public string? Product { get; set; }

    [Range(1, 1000, ErrorMessage = "Volume deve ser entre 1 e 1000")]
    public int? Volume { get; set; }

    [DisplayName("Peso")]
    [Range(1, 20000, ErrorMessage = "Peso deve ser entre 1 e 20000")]
    public int? Weight { get; set; }

    [Required]
    public int FilialId { get; set; }

    [DisplayName("Loja")]
    [Required(ErrorMessage = "Campo fornecedor é obrigatório")]
    public string? Filial { get; set; }
}