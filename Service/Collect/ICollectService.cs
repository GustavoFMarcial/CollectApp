using CollectApp.Models;
using CollectApp.ViewModels;

namespace CollectApp.Services
{
    public interface ICollectService
    {
        public Task<List<Collect>> GetAllCollectsListAsycn();
        public Task<Collect?> FindCollectAsync(int? id);
        public Task<Supplier?> FindSupplierAsync(int? id);
        public Task<Product?> FindProductAsync(int? id);
        public Task<Filial?> FindFilialAsync(int? id);
        public void AddCollect(Collect collect);
        public Task<int> SaveChangesCollectsAsync();
        public void UpdateCollectStatus(Collect collect);
        public void DeleteCollect(Collect collect);
        public Task<List<Product>> GetFilteredProductsAsync(string input);
        public Task<List<Supplier>> GetFilteredSuppliersAsync(string input);
        public Task<List<Filial>> GetFilteredFilialsAsync(string input);
        public Task<List<Supplier>> GetRegisteredSuppliersAsync();
        public Task<List<Product>> GetRegisteredProductsAsync();
        public Task<List<Filial>> GetRegisteredFilialsAsync();
    }
}