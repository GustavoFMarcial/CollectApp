using CollectApp.Dtos;
using CollectApp.Models;
using CollectApp.ViewModels;

namespace CollectApp.Repositories;

public interface ICollectRepository
{
    public Task<Collect?> GetCollectByIdAsync(int? id);
    public Task<(List<Collect> items, int totalCount)> ToCollectListAsync(CollectFilterViewModel filters, int pageNum = 1, int pageSize = 10);
    public Task<bool> AnyCollectAsync(string type, int? id);
    public void AddCollect(Collect collect);
    public Task SaveChangesCollectAsync();
    public Task<int> GetTotalCollects(DateTime startDate, DateTime endDate);
    public Task<int> GetTotalVolume(DateTime startDate, DateTime endDate);
    public Task<int> GetTotalWeight(DateTime startDate, DateTime endDate);
    public Task<List<CollectPerStatusDto>> GetCollectsPerStatus(DateTime startDate, DateTime endDate);
    public Task<List<CollectPerDayDto>> GetCollectsPerDay(DateTime startDate, DateTime endDate);
}