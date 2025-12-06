using System.ComponentModel.DataAnnotations;

namespace CollectApp.ViewModels;

public class EditFilialViewModel
{
    [Required]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;
}