using CollectApp.Data;
using CollectApp.Models;
using CollectApp.ViewModels;
using Microsoft.CodeAnalysis.Differencing;
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

        public async Task<OperationResult> CreateProduct(CreateProductViewModel productCreate)
        {
            Product product = new Product
            {
                Description = productCreate.Description,
            };

            bool productExist = await _context.Products.AnyAsync(p => p.Description == product.Description && p.Id != product.Id);

            if (productExist)
            {
                return OperationResult.Fail($"Já existe um produto cadastrado com a descrição fornecida");
            }

            _context.Products.Add(product);
            await SaveChangesProductsAsync();

            return OperationResult.Ok();
        }

        public async Task<EditProductViewModel> SetEditProductViewModel(int? id)
        {
            if (id == null)
            {
                EditProductViewModel NotFound = new EditProductViewModel();
                return NotFound;
            }

            Product? product = await FindProductAsync(id);

            if (product == null)
            {
                EditProductViewModel NotFound = new EditProductViewModel();
                return NotFound;
            }

            EditProductViewModel epvm = new EditProductViewModel
            {
                Id = product.Id,
                Description = product.Description,
            };

            return epvm;
        }

        public async Task<OperationResult> EditProduct(EditProductViewModel productEdit)
        {
            Product? product = await FindProductAsync(productEdit.Id); ;

            if (product == null)
            {
                OperationResult NotFound = new OperationResult();
                return NotFound;
            }

            bool productExist = await _context.Products.AnyAsync(p => p.Description == productEdit.Description && p.Id != productEdit.Id);

            if (productExist)
            {
                return OperationResult.Fail($"Já existe um produto cadastrado com a descrição fornecida");
            }

            product.Description = productEdit.Description;
            await SaveChangesProductsAsync();

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

        public async Task<List<ProductListViewModel>> SetProductListViewModel()
        {
            List<Product> products = await GetAllProductsListAsycn();

            List<ProductListViewModel> plvm = products.Select(p => new ProductListViewModel
            {
                Id = p.Id,
                Description = p.Description
            }).ToList();

            return plvm;
        }

        public async Task<List<Product>> GetFilteredProductsAsync(string input)
        {
            return await _context.Products.Where(p => p.Description.Contains(input)).ToListAsync();
        }

        public async Task<int> SaveChangesProductsAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task DeleteProduct(int? id)
        {
            if (id == null)
            {
                return;
            }

            Product? product = await FindProductAsync(id);

            if (product == null)
            {
                return;
            }

            _context.Products.Remove(product);
            await SaveChangesProductsAsync();
        }
    }
}