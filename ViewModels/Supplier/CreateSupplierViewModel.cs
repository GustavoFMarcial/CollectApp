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
        [StringLength(18, MinimumLength = 18, ErrorMessage = "Campo CNPJ deve ter 14 dígitos")]
        public string? CNPJ { get; set; }

        [DisplayName("Endereço")]
        [Required(ErrorMessage = "Campo endereço é obrigatório")]
        public string? Address { get; set; }
    }
}