using CollectApp.Models;
using CollectApp.ViewModels;

namespace CollectApp.Services
{
    public interface ICollectService
    {
        public Task<List<Collect>> GetAllCollectsListAsycn();
        public void CreateCollect(CollectCreateViewModel collect);
        public Task<int> SaveChangesCollectsAsync();
    }
}