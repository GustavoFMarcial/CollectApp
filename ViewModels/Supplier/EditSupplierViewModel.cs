using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CollectApp.Attributes;

namespace CollectApp.ViewModels
{
    public class EditSupplierViewModel
    {
        [Required]
        public int Id { get; set; }

        [DisplayName("Fornecedor")]
        [Required(ErrorMessage = "Campo fornecedor é obrigatório")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Campo fornecedor deve ter no mínimo 3 caracteres")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Campo CNPJ é obrigatório")]
        [StringLength(18, MinimumLength = 18, ErrorMessage = "Campo CNPJ deve ter 14 dígitos")]
        public string? CNPJ { get; set; }

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
        public string? City { get; set; }

        [DisplayName("UF")]
        [Required(ErrorMessage = "Campo UF é obrigatório")]
        [RegularExpression(@"^[A-Za-z]{2}$", ErrorMessage = "Campo UF deve conter 2 caracteres (exempo \"SP\").")]
        [StringLength(2, MinimumLength = 2)]
        public string? State { get; set; }

        [DisplayName("CEP")]
        [Required(ErrorMessage = "Campo CEP é obrigatório")]
        public string? ZipCode { get; set; }
    }
}