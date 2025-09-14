using CollectApp.Models;

namespace CollectApp.Repositories
{
    public interface ICollectRepository
    {
        public Task<Collect?> GetCollectByIdAsync(int? id);
        public Task<List<Collect>> ToCollectListAsync();
        public void AddCollect(Collect collect);
        public void RemoveCollect(Collect collect);
        public Task SaveChangesCollectAsync();
    }
}