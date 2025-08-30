using CollectApp.Models;
using CollectApp.ViewModels;

namespace CollectApp.Services
{
    public interface ICollectService
    {
        public Task<List<Collect>> GetAllCollectsListAsycn();
        public Task<Collect?> FindCollectAsync(int? id);
        public void AddCollect(Collect collect);
        public Task<int> SaveChangesCollectsAsync();
        public void UpdateCollectStatus(Collect collect);
        public void DeleteCollect(Collect collect);
        public Task<List<Product>> GetRegisteredProductsAsync();
    }
}