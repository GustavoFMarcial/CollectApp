using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CollectApp.ViewModels
{
    public class EditProductViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo descrição é obrigatório")]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "Descrição deve ter entre 3 e 15 caracteres")]
        [DisplayName("Descrição")]
        public string? Description { get; set; }
    }
}