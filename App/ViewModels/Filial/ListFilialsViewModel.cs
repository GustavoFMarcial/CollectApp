using System.ComponentModel;

namespace CollectApp.ViewModels;

public class FilialListViewModel
{
    public int Id { get; set; }

    [DisplayName("Nome")]
    public string Name { get; set; } = string.Empty;
}