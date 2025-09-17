using CollectApp.Models;
using CollectApp.ViewModels;

namespace CollectApp.Services
{
    public interface IProductService
    {
        public Task<(List<Product> items, int totalCount)> GetAllProductsListAsycn(int pageNum = 1, int pageSize = 10);
        public Task<List<Product>> GetFilteredProductsAsync(string input);
        public Task<PagedResultViewModel<ProductListViewModel>> SetPagedResultProductListViewModel(int pageNum = 1, int pageSize = 10);
        public Task<OperationResult> CreateProduct(CreateProductViewModel productCreate);
        public Task<OperationResult> DeleteProduct(int? id);
        public Task<EditProductViewModel> SetEditProductViewModel(int? id);
        public Task<OperationResult> EditProduct(EditProductViewModel productEdit);
    }
}