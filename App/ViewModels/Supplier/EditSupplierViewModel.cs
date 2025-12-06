using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CollectApp.ViewModels;

public class EditSupplierViewModel
{
    [Required]
    public int Id { get; set; }

    [DisplayName("Fornecedor")]
    [Required(ErrorMessage = "Campo fornecedor é obrigatório")]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "Campo fornecedor deve ter no mínimo 3 caracteres")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Campo CNPJ é obrigatório")]
    [RegularExpression(@"^\d{2}\.\d{3}\.\d{3}/\d{4}-\d{2}$", ErrorMessage = "CNPJ inválido. Formato esperado: 00.000.000/0000-00")]
    public string CNPJ { get; set; } = string.Empty;

    [DisplayName("Rua")]
    [Required(ErrorMessage = "Campo rua é obrigatório")]
    public string? Street { get; set; }

    [DisplayName("Bairro")]
    [Required(ErrorMessage = "Campo bairro é obrigatório")]
    public string? Neighborhood { get; set; }

    [DisplayName("Número")]
    [Required(ErrorMessage = "Campo número é obrigatório")]
    public string? Number { get; set; }

    [DisplayName("Cidade")]
    [Required(ErrorMessage = "Campo cidade é obrigatório")]
    public string City { get; set; } = string.Empty;

    [DisplayName("UF")]
    [Required(ErrorMessage = "Campo UF é obrigatório")]
    [RegularExpression(@"^[A-Za-z]{2}$", ErrorMessage = "Campo UF deve conter 2 caracteres (exempo \"SP\").")]
    public string? State { get; set; }

    [DisplayName("CEP")]
    [Required(ErrorMessage = "Campo CEP é obrigatório")]
    [RegularExpression(@"^\d{5}-\d{3}$", ErrorMessage = "CEP inválido. Formato esperado: 00000-000")]
    public string? ZipCode { get; set; }
}