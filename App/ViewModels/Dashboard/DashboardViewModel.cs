using CollectApp.Dtos;

namespace CollectApp.ViewModels;

public class DashboardViewModel
{
    public int TotalCollects { get; set; }
    public int TotalVolume { get; set; }
    public int TotalWeight { get; set; }
    public List<CollectPerStatusDto> CollectPerStatusDto { get; set; } = new();
    public List<CollectPerDayDto> CollectPerDayDto { get; set; } = new();
}