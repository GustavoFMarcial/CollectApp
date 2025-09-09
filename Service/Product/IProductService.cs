using CollectApp.Models;
using CollectApp.ViewModels;

namespace CollectApp.Services
{
    public interface IProductService
    {
        public Task<List<Product>> GetAllProductsListAsycn();
        public Task<List<Product>> GetFilteredProductsAsync(string input);
        public Task<List<ProductListViewModel>> SetProductListViewModel();
        public Task<Product?> FindProductAsync(int? id);
        public Task<OperationResult> CreateProduct(CreateProductViewModel productCreate);
        public Task<int> SaveChangesProductsAsync();
        public Task DeleteProduct(int? id);
        public Task<EditProductViewModel> SetEditProductViewModel(int? id);
        public Task<OperationResult> EditProduct(EditProductViewModel productEdit);
    }
}