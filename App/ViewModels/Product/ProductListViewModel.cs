using System.ComponentModel;

namespace CollectApp.ViewModels;

public class ProductListViewModel
{
    public int Id { get; set; }

    [DisplayName("Descrição")]
    public string? Name { get; set; }
}