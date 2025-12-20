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
        DateTime previousStartDate = startDate.AddMonths(-1);
        DateTime previousEndDate = endDate.AddMonths(-1);

        DashboardViewModel dashboardViewModel = new DashboardViewModel
        {
            CurrentTotalCollects = await _collectRepository.GetTotalCollects(startDate, endDate),
            PreviousTotalCollects = await _collectRepository.GetTotalCollects(previousStartDate, previousEndDate),
            CurrentTotalVolume = await _collectRepository.GetTotalVolume(startDate, endDate),
            PreviousTotalVolume = await _collectRepository.GetTotalVolume(previousStartDate, previousEndDate),
            CurrentTotalWeight = await _collectRepository.GetTotalWeight(startDate, endDate),
            PreviousTotalWeight = await _collectRepository.GetTotalWeight(previousStartDate, previousEndDate),
            CurrentTopProduct = await _collectRepository.GetTopProduct(startDate, endDate),
            PreviousTopProduct = await _collectRepository.GetTopProduct(previousStartDate, previousEndDate),
            CurrentTopSupplier = await _collectRepository.GetTopSupplier(startDate, endDate),
            PreviousTopSupplier = await _collectRepository.GetTopSupplier(previousStartDate, previousEndDate),
            CurrentTopFilial = await _collectRepository.GetTopFilial(startDate, endDate),
            PreviousTopFilial = await _collectRepository.GetTopFilial(previousStartDate, previousEndDate),
            CollectPerStatusDto = await _collectRepository.GetCollectsPerStatus(startDate, endDate),
            CollectPerDayDto = await _collectRepository.GetCollectsPerDay(startDate, endDate),
        };

        return dashboardViewModel;
    }
}