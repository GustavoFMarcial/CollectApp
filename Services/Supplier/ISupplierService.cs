using CollectApp.Models;
using CollectApp.ViewModels;

namespace CollectApp.Services
{
    public interface ISupplierService
    {
        // public Task<List<Supplier>> GetFilteredSuppliersAsync(string input);
        public Task<PagedResultViewModel<SupplierListViewModel>> SetPagedResultSupplierListViewModel(int pageNum = 1, int pageSize = 10, string? input = null);
        public Task<OperationResult> CreateSupplier(CreateSupplierViewModel supplierCreate);
        public Task<EditSupplierViewModel> SetEditSupplierViewModel(int? id);
        public Task<OperationResult> EditSupplier(EditSupplierViewModel supplierEdit);
        public Task<OperationResult> DeleteSupplier(int? id);
    }
}