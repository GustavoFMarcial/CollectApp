using CollectApp.Models;

namespace CollectApp.Repositories
{
    public interface ICollectRepository
    {
        public Task<Collect?> GetCollectByIdAsync(int? id);
        public Task<List<Collect>> ToCollectListAsync();
        // public Task<bool> AnyCollectAsync(string productDescription, int? productId);
        // public Task<List<Collect>> WhereCollectAsync(string input);
        public Task AddCollect(Collect collect);
        public Task RemoveCollect(Collect collect);
        public Task SaveChangesCollectAsync();
    }
}