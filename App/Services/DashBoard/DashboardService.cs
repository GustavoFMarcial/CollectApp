using CollectApp.Repositories;
using CollectApp.ViewModels;

namespace CollectApp.Services;

public class DashboardService : IDashboardService
{
    private readonly ICollectRepository _collectRepository;

    public DashboardService(ICollectRepository collectRepository)
    {
        _collectRepository = collectRepository;
    }

    public async Task<DashboardViewModel> GetDashboardData(DateTime startDate, DateTime endDate)
    {
        DashboardViewModel dashboardViewModel = new DashboardViewModel
        {
            TotalCollects = await _collectRepository.GetTotalCollects(startDate, endDate),
            TotalVolume = await _collectRepository.GetTotalVolume(startDate, endDate),
            TotalWeight = await _collectRepository.GetTotalWeight(startDate, endDate),
            CollectPerStatusDto = await _collectRepository.GetCollectsPerStatus(startDate, endDate),
            CollectPerDayDto = await _collectRepository.GetCollectsPerDay(startDate, endDate),
        };

        return dashboardViewModel;
    }
}