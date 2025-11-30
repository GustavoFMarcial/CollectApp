using CollectApp.Models;
using CollectApp.ViewModels;

namespace CollectApp.Services;

public interface IProductService
{
    public Task<PagedResultViewModel<ProductListViewModel, ProductFilterViewModel>> SetPagedResultProductListViewModel(ProductFilterViewModel filters, int pageNum = 1, int pageSize = 10, string? input = null);
    public Task<OperationResult> CreateProduct(CreateProductViewModel productCreate);
    public Task<OperationResult?> DeleteProduct(int id);
    public Task<EditProductViewModel?> SetEditProductViewModel(int id);
    public Task<OperationResult?> EditProduct(EditProductViewModel productEdit);
}