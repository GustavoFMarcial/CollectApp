using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CollectApp.ViewModels;

public class CreateProductViewModel
{
    [Required(ErrorMessage = "Campo descrição é obrigatório")]
    [StringLength(15, MinimumLength = 3, ErrorMessage = "Descrição deve ter entre 3 e 15 caracteres")]
    [DisplayName("Descrição")]
    public string Name { get; set; } = string.Empty;
}