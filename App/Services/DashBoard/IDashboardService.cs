using CollectApp.ViewModels;

namespace CollectApp.Services;

public interface IDashboardService
{
    public Task<DashboardViewModel> GetDashboardData(DateTime startDate, DateTime endDate);
}