using CollectApp.Models;
using CollectApp.ViewModels;

namespace CollectApp.Services
{
    public interface IProductService
    {
        public Task<List<Product>> GetAllProductsListAsycn();
        public Task<Product?> FindProductAsync(int? id);
        public Task<OperationResult> AddProduct(Product product);
        public Task<int> SaveChangesProductsAsync();
        public void DeleteProduct(Product product);
        public Task<OperationResult> EditProduct(EditProductViewModel productEdit);
    }
}