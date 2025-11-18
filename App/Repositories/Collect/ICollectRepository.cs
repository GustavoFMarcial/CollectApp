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
}