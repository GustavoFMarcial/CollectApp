using CollectApp.Models;

namespace CollectApp.Repositories
{
    public interface ICollectRepository
    {
        public Task<Collect?> GetCollectByIdAsync(int? id);
        public Task<(List<Collect> items, int totalCount)> ToCollectListAsync(int pageNum = 1, int pageSize = 10);
        public Task<bool> AnyCollectAsync(string type, int? id);
        public void AddCollect(Collect collect);
        public void RemoveCollect(Collect collect);
        public Task SaveChangesCollectAsync();
    }
}