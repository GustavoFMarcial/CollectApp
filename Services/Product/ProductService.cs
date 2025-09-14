using CollectApp.Models;
using CollectApp.Repositories;
using CollectApp.ViewModels;

namespace CollectApp.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<OperationResult> CreateProduct(CreateProductViewModel productCreate)
        {
            Product product = new Product
            {
                Description = productCreate.Description,
            };

            bool productExist = await _productRepository.AnyProductAsync(productCreate.Description, product.Id);

            if (productExist)
            {
                return OperationResult.Fail($"Já existe um produto cadastrado com a descrição fornecida");
            }

            _productRepository.AddProduct(product);
            await _productRepository.SaveChangesProductAsync();

            return OperationResult.Ok();
        }

        public async Task<EditProductViewModel> SetEditProductViewModel(int? id)
        {
            if (id == null)
            {
                EditProductViewModel NotFound = new EditProductViewModel();
                return NotFound;
            }

            Product? product = await _productRepository.GetProductByIdAsync(id);

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
            Product? product = await _productRepository.GetProductByIdAsync(productEdit.Id); ;

            if (product == null)
            {
                OperationResult NotFound = new OperationResult();
                return NotFound;
            }

            bool productExist = await _productRepository.AnyProductAsync(productEdit.Description, productEdit.Id);

            if (productExist)
            {
                return OperationResult.Fail($"Já existe um produto cadastrado com a descrição fornecida");
            }

            product.Description = productEdit.Description;
            await _productRepository.SaveChangesProductAsync();

            return OperationResult.Ok();
        }

        public async Task<List<Product>> GetAllProductsListAsycn()
        {
            return await _productRepository.ToProductListAsync();
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
            return await _productRepository.WhereProductAsync(input);
        }

        public async Task DeleteProduct(int? id)
        {
            if (id == null)
            {
                return;
            }

            Product? product = await _productRepository.GetProductByIdAsync(id);

            if (product == null)
            {
                return;
            }

            _productRepository.RemoveProduct(product);
            await _productRepository.SaveChangesProductAsync();
        }
    }
}