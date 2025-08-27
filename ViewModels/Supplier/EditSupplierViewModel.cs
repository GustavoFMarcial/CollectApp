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
        public string? Name { get; set; }

        [Required(ErrorMessage = "Campo CNPJ é obrigatório")]
        [StringLength(18, MinimumLength = 18, ErrorMessage = "Campo CNPJ deve ter 14 dígitos")]
        public string? CNPJ { get; set; }

        [DisplayName("Endereço")]
        [Required(ErrorMessage = "Campo endereço é obrigatório")]
        public string? Address { get; set; }
    }
}