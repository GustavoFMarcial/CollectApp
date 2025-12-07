using CollectApp.Models;
using CollectApp.Repositories;
using CollectApp.Services;
using CollectApp.ViewModels;
using CollectAppTests.Builders;
using FluentAssertions;
using Moq;

namespace CollectAppTests.Services;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _productRepoMock;
    private readonly Mock<ICollectRepository> _collectRepoMock;

    public ProductServiceTests()
    {
        _productRepoMock = new Mock<IProductRepository>();
        _collectRepoMock = new Mock<ICollectRepository>();
    }

    [Fact]
    public async Task CreateProduct_WhenProductExistIsFalse_ShouldCreateProduct()
    {
        Product? productSent = null;

        _productRepoMock
            .Setup(p => p.AnyProductAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(false);

        _productRepoMock
            .Setup(p => p.AddProduct(It.IsAny<Product>()))
            .Callback<Product>(p => productSent = p);

        var createProductViewModel = new CreateProductViewModelBuilder().Build();

        var service = new ProductService(
            _productRepoMock.Object,
            _collectRepoMock.Object
        );

        var result = await service.CreateProduct(createProductViewModel);

        var expected = new OperationResultBuilder()
            .WithSuccess(true)
            .Build();

        result.Should().BeEquivalentTo(expected);
        productSent.Should().NotBeNull();
        productSent.Name.Should().Be(createProductViewModel.Name);

        _productRepoMock.Verify(p => p.AnyProductAsync(It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        _productRepoMock.Verify(p => p.AddProduct(It.IsAny<Product>()), Times.Once);
        _productRepoMock.Verify(p => p.SaveChangesProductAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateProduct_WhenProductExistIsTrue_ShouldNotCreateProduct()
    {
        _productRepoMock
            .Setup(p => p.AnyProductAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(true);

        var createProductViewModel = new CreateProductViewModelBuilder().Build();

        var service = new ProductService(
            _productRepoMock.Object,
            _collectRepoMock.Object
        );

        var result = await service.CreateProduct(createProductViewModel);

        var expected = new OperationResultBuilder()
            .WithSuccess(false)
            .WithMessage("Já existe um produto cadastrado com a descrição fornecida")
            .Build();

        result.Should().BeEquivalentTo(expected);

        _productRepoMock.Verify(p => p.AnyProductAsync(It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        _productRepoMock.Verify(p => p.AddProduct(It.IsAny<Product>()), Times.Never);
        _productRepoMock.Verify(p => p.SaveChangesProductAsync(), Times.Never);
    }

    [Fact]
    public async Task EditProduct_WhenProductIsNotNullAndProductExistIsFalse_ShouldEditProduct()
    {
        var product = new ProductBuilder().Build();

        var EditProductViewModel = new EditProductViewModelBuilder().Build();

        _productRepoMock
            .Setup(p => p.GetProductByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(product);

        _productRepoMock
            .Setup(p => p.AnyProductAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(false);

        var service = new ProductService(
            _productRepoMock.Object,
            _collectRepoMock.Object
        );

        var result = await service.EditProduct(EditProductViewModel);

        var expected = new OperationResultBuilder()
            .WithSuccess(true)
            .Build();

        result.Should().BeEquivalentTo(expected);
        product.Name.Should().Be(EditProductViewModel.Name);

        _productRepoMock.Verify(p => p.GetProductByIdAsync(It.IsAny<int>()), Times.Once);
        _productRepoMock.Verify(p => p.AnyProductAsync(It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        _productRepoMock.Verify(p => p.SaveChangesProductAsync(), Times.Once);
    }

    [Fact]
    public async Task EditProduct_WhenProductIsNull_ShouldNotEditProduct()
    {
        var product = new ProductBuilder().Build();

        var EditProductViewModel = new EditProductViewModelBuilder().Build();

        _productRepoMock
            .Setup(p => p.GetProductByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Product?)null);

        var service = new ProductService(
            _productRepoMock.Object,
            _collectRepoMock.Object
        );

        var result = await service.EditProduct(EditProductViewModel);

        result.Should().BeNull();
        product.Name.Should().NotBe(EditProductViewModel.Name);

        _productRepoMock.Verify(p => p.GetProductByIdAsync(It.IsAny<int>()), Times.Once);
        _productRepoMock.Verify(p => p.AnyProductAsync(It.IsAny<string>(), It.IsAny<int>()), Times.Never);
        _productRepoMock.Verify(p => p.SaveChangesProductAsync(), Times.Never);
    }

    [Fact]
    public async Task EditProduct_WhenProductExistIsTrue_ShouldNotEditProduct()
    {
        var product = new ProductBuilder().Build();

        var EditProductViewModel = new EditProductViewModelBuilder().Build();

        _productRepoMock
            .Setup(p => p.GetProductByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(product);

        _productRepoMock
            .Setup(p => p.AnyProductAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(true);

        var service = new ProductService(
            _productRepoMock.Object,
            _collectRepoMock.Object
        );

        var result = await service.EditProduct(EditProductViewModel);

        var expected = new OperationResultBuilder()
            .WithSuccess(false)
            .WithMessage("Já existe um produto cadastrado com a descrição fornecida")
            .Build();

        result.Should().BeEquivalentTo(expected);
        product.Name.Should().NotBe(EditProductViewModel.Name);

        _productRepoMock.Verify(p => p.GetProductByIdAsync(It.IsAny<int>()), Times.Once);
        _productRepoMock.Verify(p => p.AnyProductAsync(It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        _productRepoMock.Verify(p => p.SaveChangesProductAsync(), Times.Never);
    }


    [Fact]
    public async Task SetPagedResultProductListViewModel_WhenHasItems_ShouldReturnListWithItems()
    {
        var productList = new List<Product>
        {
            new ProductBuilder().Build(),
            new ProductBuilder()
                .WithId(2)
                .WithName("Manga")
                .Build(),
            new ProductBuilder()
                .WithId(3)
                .WithName("Mussarela")
                .Build(),
        };

        var productListViewModel = productList.Select(p => 
            new ProductListViewModelBuilder()
            .FromProduct(p)
            .Build())
            .ToList();

        var filters = new ProductFilterViewModel();

        _productRepoMock
            .Setup(p => p.ToProductListAsync(It.IsAny<ProductFilterViewModel>(), 1, 10, ""))
            .ReturnsAsync((productList, 3));

        var service = new ProductService(
            _productRepoMock.Object,
            _collectRepoMock.Object
        );

        var result = await service.SetPagedResultProductListViewModel(filters, 1, 10, "");

        var expected = new PagedResultViewModel<ProductListViewModel, ProductFilterViewModel>
        {
            Items = productListViewModel,
            TotalPages = 1,
            PageNum = 1,
            Filters = filters,
        };

        result.Should().BeEquivalentTo(expected);
        result.Items.Should().HaveCount(3);

        _productRepoMock.Verify(p => p.ToProductListAsync(It.IsAny<ProductFilterViewModel>(), 1, 10, ""), Times.Once);
    }

    [Fact]
    public async Task SetPagedResultProductListViewModel_WhenHasNoItems_ShouldReturnEmptyList()
    {
        var filters = new ProductFilterViewModel();

        _productRepoMock
            .Setup(p => p.ToProductListAsync(It.IsAny<ProductFilterViewModel>(), 1, 10, ""))
            .ReturnsAsync(([], 0));

        var service = new ProductService(
            _productRepoMock.Object,
            _collectRepoMock.Object
        );

        var result = await service.SetPagedResultProductListViewModel(filters, 1, 10, "");

        var expected = new PagedResultViewModel<ProductListViewModel, ProductFilterViewModel>
        {
            Items = [],
            TotalPages = 0,
            PageNum = 1,
            Filters = filters,
        };

        result.Should().BeEquivalentTo(expected);
        result.Items.Should().HaveCount(0);

        _productRepoMock.Verify(p => p.ToProductListAsync(It.IsAny<ProductFilterViewModel>(), 1, 10, ""), Times.Once);
    }

    [Fact]
    public async Task DeleteProduct_WhenProductIsNull_ShouldNotDeleteProduct()
    {
        _productRepoMock
            .Setup(p => p.GetProductByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Product?)null);

        var service = new ProductService(
            _productRepoMock.Object,
            _collectRepoMock.Object
        );

        var result = await service.DeleteProduct(1);

        result.Should().BeNull();

        _productRepoMock.Verify(p => p.GetProductByIdAsync(It.IsAny<int>()), Times.Once);
        _collectRepoMock.Verify(c => c.AnyCollectAsync(It.IsAny<string>(), It.IsAny<int>()), Times.Never);
        _productRepoMock.Verify(p => p.RemoveProduct(It.IsAny<Product>()), Times.Never);
        _productRepoMock.Verify(p => p.SaveChangesProductAsync(), Times.Never);
    }

    [Fact]
    public async Task DeleteProduct_WhenExistProductWithCollectIsTrue_ShouldNotDeleteProduct()
    {
        var product = new ProductBuilder().Build();

        _productRepoMock
            .Setup(p => p.GetProductByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(product);

        _collectRepoMock
            .Setup(c => c.AnyCollectAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(true);

        var service = new ProductService(
            _productRepoMock.Object,
            _collectRepoMock.Object
        );

        var result = await service.DeleteProduct(1);

        var expected = 
            new OperationResultBuilder()
            .WithSuccess(false)
            .WithMessage("Não foi possível deletar, existe uma coleta vinculada a este produto")
            .Build();

        result.Should().BeEquivalentTo(expected);

        _productRepoMock.Verify(p => p.GetProductByIdAsync(It.IsAny<int>()), Times.Once);
        _collectRepoMock.Verify(c => c.AnyCollectAsync(It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        _productRepoMock.Verify(p => p.RemoveProduct(It.IsAny<Product>()), Times.Never);
        _productRepoMock.Verify(p => p.SaveChangesProductAsync(), Times.Never);
    }
}