using Azure;
using CollectApp.Models;
using CollectApp.Repositories;
using CollectApp.ViewModels;

namespace CollectApp.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICollectRepository _collectRepository;

        public ProductService(IProductRepository productRepository, ICollectRepository collectRepository)
        {
            _productRepository = productRepository;
            _collectRepository = collectRepository;
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

        public async Task<PagedResultViewModel<ProductListViewModel>> SetPagedResultProductListViewModel(int pageNum = 1, int pageSize = 10)
        {
            (List<Product> items, int totalCount) products = await _productRepository.ToProductListAsync(pageNum);

            List<ProductListViewModel> productListViewModel = products.items.Select(p => new ProductListViewModel
            {
                Id = p.Id,
                Description = p.Description
            }).ToList();

            PagedResultViewModel<ProductListViewModel> pagedResultProductListViewModel = new PagedResultViewModel<ProductListViewModel>
            {
                Items = productListViewModel,
                TotalPages = (int)Math.Ceiling(products.totalCount / (double)pageSize),
                PageNum = pageNum,
            };

            return pagedResultProductListViewModel;
        }

        public async Task<List<Product>> GetFilteredProductsAsync(string input)
        {
            return await _productRepository.WhereProductAsync(input);
        }

        public async Task<OperationResult> DeleteProduct(int? id)
        {
            if (id == null)
            {
                OperationResult NotFound = new OperationResult();
                return NotFound;
            }

            Product? product = await _productRepository.GetProductByIdAsync(id);

            if (product == null)
            {
                OperationResult NotFound = new OperationResult();
                return NotFound;
            }

            bool existProductWithCollect = await _collectRepository.AnyCollectAsync("product", product.Id);

            if (existProductWithCollect)
            {
                return OperationResult.Fail("Não é possível deletar, existe uma coleta vinculada a este produto");
            }

            _productRepository.RemoveProduct(product);
            await _productRepository.SaveChangesProductAsync();

            return OperationResult.Ok();
        }
    }
}