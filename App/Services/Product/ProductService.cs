using CollectApp.Models;
using CollectApp.Repositories;
using CollectApp.ViewModels;

namespace CollectApp.Services;

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
            Name = productCreate.Name,
        };

        bool productExist = await _productRepository.AnyProductAsync(productCreate.Name, product.Id);

        if (productExist)
        {
            return OperationResult.Fail($"Já existe um produto cadastrado com a descrição fornecida");
        }

        _productRepository.AddProduct(product);
        await _productRepository.SaveChangesProductAsync();

        return OperationResult.Ok();
    }

    public async Task<EditProductViewModel?> SetEditProductViewModel(int id)
    {
        Product? product = await _productRepository.GetProductByIdAsync(id);

        if (product == null)
        {
            return null;
        }

        EditProductViewModel epvm = new EditProductViewModel
        {
            Id = product.Id,
            Name = product.Name,
        };

        return epvm;
    }

    public async Task<OperationResult?> EditProduct(EditProductViewModel productEdit)
    {
        Product? product = await _productRepository.GetProductByIdAsync(productEdit.Id); ;

        if (product == null)
        {
            return null;
        }

        bool productExist = await _productRepository.AnyProductAsync(productEdit.Name, productEdit.Id);

        if (productExist)
        {
            return OperationResult.Fail($"Já existe um produto cadastrado com a descrição fornecida");
        }

        product.Name = productEdit.Name;
        await _productRepository.SaveChangesProductAsync();

        return OperationResult.Ok();
    }

    public async Task<PagedResultViewModel<ProductListViewModel, ProductFilterViewModel>> SetPagedResultProductListViewModel(ProductFilterViewModel filters, int pageNum = 1, int pageSize = 10, string? input = null)
    {
        (List<Product> items, int totalCount) products = await _productRepository.ToProductListAsync(filters, pageNum, pageSize, input);

        List<ProductListViewModel> productListViewModel = products.items.Select(p => new ProductListViewModel
        {
            Id = p.Id,
            Name = p.Name
        }).ToList();

        PagedResultViewModel<ProductListViewModel, ProductFilterViewModel> pagedResultProductListViewModel = new PagedResultViewModel<ProductListViewModel, ProductFilterViewModel>
        {
            Items = productListViewModel,
            TotalPages = (int)Math.Ceiling(products.totalCount / (double)pageSize),
            PageNum = pageNum,
            Filters = filters,
        };

        return pagedResultProductListViewModel;
    }

    public async Task<OperationResult?> DeleteProduct(int id)
    {
        Product? product = await _productRepository.GetProductByIdAsync(id);

        if (product == null)
        {
            return null;
        }

        bool existProductWithCollect = await _collectRepository.AnyCollectAsync("product", product.Id);

        if (existProductWithCollect)
        {
            return OperationResult.Fail("Não foi possível deletar, existe uma coleta vinculada a este produto");
        }

        _productRepository.RemoveProduct(product);
        await _productRepository.SaveChangesProductAsync();

        return OperationResult.Ok();
    }
}