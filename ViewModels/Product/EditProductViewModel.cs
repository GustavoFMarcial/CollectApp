using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CollectApp.ViewModels
{
    public class EditProductViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "Descrição deve ter entre 3 e 15 caracteres")]
        [DisplayName("Descrição")]
        public string? Description { get; set; }
    }
}