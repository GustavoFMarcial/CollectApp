using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CollectApp.ViewModels
{
    public class CreateSupplierViewModel
    {
        [DisplayName("Fornecedor")]
        [Required(ErrorMessage = "Campo fornecedor é obrigatório")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Campo CNPJ é obrigatório")]
        public string? CNPJ { get; set; }

        [DisplayName("Endereço")]
        [Required(ErrorMessage = "Campo endereço é obrigatório")]
        public string? Address { get; set; }
    }
}