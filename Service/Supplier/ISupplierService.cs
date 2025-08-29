using CollectApp.Models;

namespace CollectApp.Services
{
    public interface ISupplierService
    {
        public Task<List<Supplier>> GetAllSuppliersListAsycn();
        public Task<Supplier?> FindSupplierAsync(int? id);
        public void AddSupplier(Supplier supplier);
        public Task<int> SaveChangesSuppliersAsync();
        public void DeleteSupplier(Supplier supplier);
    }
}