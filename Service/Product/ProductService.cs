using CollectApp.Data;
using CollectApp.Models;
using CollectApp.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CollectApp.Services
{
    public class ProductService : IProductService
    {
        private readonly CollectAppContext _context;

        public ProductService(CollectAppContext context)
        {
            _context = context;
        }

        public async Task<OperationResult> AddProduct(Product product)
        {
            bool productExist = await _context.Products.AnyAsync(p => p.Description == product.Description && p.Id != product.Id);

            if (productExist)
            {
                return OperationResult.Fail($"Já existe um produto cadastrado com a descrição fornecida");
            }

            _context.Products.Add(product);

            return OperationResult.Ok();
        }

        public async Task<OperationResult> EditProduct(EditProductViewModel productEdit)
        {
            bool productExist = await _context.Products.AnyAsync(p => p.Description == productEdit.Description && p.Id != productEdit.Id);

            if (productExist)   
            {
                return OperationResult.Fail($"Já existe um produto cadastrado com a descrição fornecida");
            }

            return OperationResult.Ok();
        }

        public async Task<Product?> FindProductAsync(int? id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<List<Product>> GetAllProductsListAsycn()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<int> SaveChangesProductsAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void DeleteProduct(Product product)
        {
            _context.Products.Remove(product);
        }
    }
}