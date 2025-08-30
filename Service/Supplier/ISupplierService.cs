using CollectApp.Models;
using CollectApp.ViewModels;

namespace CollectApp.Services
{
    public interface ISupplierService
    {
        public Task<List<Supplier>> GetAllSuppliersListAsycn();
        public Task<Supplier?> FindSupplierAsync(int? id);
        public Task<OperationResult> AddSupplier(Supplier supplier);
        public Task<OperationResult> EditSupplier(EditSupplierViewModel supplierEdit);
        public Task<int> SaveChangesSuppliersAsync();
        public void DeleteSupplier(Supplier supplier);
        // public Task<bool> SupplierExist(Supplier supplier);
    }
}