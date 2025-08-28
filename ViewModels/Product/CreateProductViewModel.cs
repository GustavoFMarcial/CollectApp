using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CollectApp.ViewModels
{
    public class CreateProductViewModel
    {
        [Required]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "Produto deve ter entre 3 e 15 caracteres")]
        [DisplayName("Descrição")]
        public string? Description { get; set; }
    }
}