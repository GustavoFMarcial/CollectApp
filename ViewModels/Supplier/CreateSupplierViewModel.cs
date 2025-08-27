using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CollectApp.Attributes;
using CollectApp.Models;

namespace CollectApp.ViewModels
{
    public class CreateSupplierViewModel
    {
        [DisplayName("Fornecedor")]
        [Required(ErrorMessage = "Campo fornecedor é obrigatório")]
        [StringLength(5, ErrorMessage = "Campo fornecedor deve ter no mínimo 5 caracteres")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Campo CNPJ é obrigatório")]
        [RegularExpression(@"^\d{14}$", ErrorMessage = "O CNPJ deve conter exatamente 14 dígitos numéricos.")]
        public string? CNPJ { get; set; }

        [DisplayName("Endereço")]
        [Required(ErrorMessage = "Campo endereço é obrigatório")]
        public string? Address { get; set; }
    }
}