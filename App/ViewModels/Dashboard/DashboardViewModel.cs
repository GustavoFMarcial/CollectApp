using CollectApp.Dtos;
using CollectApp.Models;

namespace CollectApp.ViewModels;

public class DashboardViewModel
{
    public int CurrentTotalCollects { get; set; }
    public int PreviousTotalCollects { get; set; }
    public int CurrentTotalVolume { get; set; }
    public int PreviousTotalVolume { get; set; }
    public int CurrentTotalWeight { get; set; }
    public int PreviousTotalWeight { get; set; }
    public TopProductDto CurrentTopProduct { get; set; } = new();
    public TopProductDto PreviousTopProduct { get; set; } = new();
    public TopSupplierDto CurrentTopSupplier { get; set; } = new();
    public TopSupplierDto PreviousTopSupplier { get; set; } = new();
    public TopFilialDto CurrentTopFilial { get; set; } = new();
    public TopFilialDto PreviousTopFilial { get; set; } = new();
    public List<CollectPerStatusDto> CollectPerStatusDto { get; set; } = new();
    public List<CollectPerDayDto> CollectPerDayDto { get; set; } = new();
}