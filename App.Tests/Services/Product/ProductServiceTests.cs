using CollectApp.Models;
using CollectApp.Repositories;
using CollectApp.Services;
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
        _productRepoMock.Setup(p => p.AnyProductAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(false);

        var createProductViewModel = new CreateProductViewModelBuilder().Build();

        var service = new ProductService(
            _productRepoMock.Object,
            _collectRepoMock.Object
        );

        var result = await service.CreateProduct(createProductViewModel);

        var expected = new OperationResultBuilder().WithSuccess(true).Build();

        result.Should().BeEquivalentTo(expected);

        _productRepoMock.Verify(p => p.AnyProductAsync(It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        _productRepoMock.Verify(p => p.AddProduct(It.IsAny<Product>()), Times.Once);
        _productRepoMock.Verify(p => p.SaveChangesProductAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateProduct_WhenProductExistIsTrue_ShouldNotCreateProduct()
    {
        _productRepoMock.Setup(p => p.AnyProductAsync(It.IsAny<string>(), It.IsAny<int>()))
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
}