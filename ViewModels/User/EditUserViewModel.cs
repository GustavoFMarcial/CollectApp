using System.ComponentModel;

namespace CollectApp.ViewModels;

public class EditUserViewModel
{
    public string Id { get; set; } = string.Empty;

    [DisplayName("Nome completo")]
    public string FullName { get; set; } = string.Empty;

    [DisplayName("Cargo")]
    public string Role { get; set; } = string.Empty;
}