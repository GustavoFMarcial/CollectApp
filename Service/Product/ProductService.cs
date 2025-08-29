using CollectApp.Data;
using CollectApp.Models;
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

        public async Task<bool> ProductExist(Product product)
        {
            return await _context.Products.AnyAsync(p => p.Description == product.Description);
        }

        public async Task<OperationResult> AddProduct(Product product)
        {
            bool productExist = await ProductExist(product);

            if (productExist)
            {
                return OperationResult.Fail(0, $"Já existe um produto cadastrado com a descrição fornecida");
            }

            _context.Products.Add(product);
            return OperationResult.Ok();
        }

        public async Task<OperationResult> EditProduct(Product product)
        {
            bool productExist = await ProductExist(product);

            if (productExist)   
            {
                Product? p = await _context.Products.FindAsync(product.Id);
                // if (p == null)
                // {
                //     return
                // }
                return OperationResult.Fail(p.Id, $"Já existe um produto cadastrado com a descrição fornecida");
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