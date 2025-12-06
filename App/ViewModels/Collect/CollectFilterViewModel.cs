using CollectApp.Models;

namespace CollectApp.ViewModels;

public class CollectFilterViewModel
{
    public int? Id { get; set; }
    public DateTime? StartCreation { get; set; }
    public DateTime? EndCreation { get; set; }
    public string? User { get; set; }
    public string? Supplier { get; set; }
    public DateTime? StartCollect { get; set; }
    public DateTime? EndCollect { get; set; }
    public string? Product { get; set; }
    public CollectStatus? Status { get; set; }
    public int? Volume { get; set; }
    public int? Weight { get; set; }
    public string? Filial { get; set; }
}