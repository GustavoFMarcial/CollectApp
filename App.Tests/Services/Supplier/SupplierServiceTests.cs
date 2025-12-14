using CollectApp.Models;
using CollectApp.Repositories;
using CollectApp.Services;
using CollectApp.Tests.Builders;
using CollectApp.ViewModels;
using CollectAppTests.Builders;
using FluentAssertions;
using Moq;

namespace CollectAppTests.Services;

public class SupplierServiceTests
{
    private readonly Mock<ISupplierRepository> _supplierRepoMock;
    private readonly Mock<ICollectRepository> _collectRepoMock;

    public SupplierServiceTests()
    {
        _supplierRepoMock = new Mock<ISupplierRepository>();
        _collectRepoMock = new Mock<ICollectRepository>();
    }

    [Fact]
    public async Task SetPagedResultSupplierListViewModel_WhenHasItems_ShouldReturnListWithItems()
    {
        var supplierList = new List<Supplier>
        {
            new SupplierBuilder().Build(),
            new SupplierBuilder()
                .WithId(2)
                .WithName("Catapimbas")
                .Build(),
            new SupplierBuilder()
                .WithId(3)
                .WithName("Joba da Silva")
                .Build(),
        };

        var filters = new SupplierFilterViewModel();

        var supplierListViewModel = supplierList.Select(s =>
            new SupplierListViewModelBuilder()
            .FromSupplier(s)
            .Build())
            .ToList();

        _supplierRepoMock
            .Setup(s => s.ToSupplierListAsync(It.IsAny<SupplierFilterViewModel>(), 1, 10, ""))
            .ReturnsAsync((supplierList, 3));

        var service = new SupplierService(
            _supplierRepoMock.Object,
            _collectRepoMock.Object
        );

        var result = await service.SetPagedResultSupplierListViewModel(filters, 1, 10, "");

        var expected = new PagedResultViewModel<SupplierListViewModel, SupplierFilterViewModel>
        {
            Items = supplierListViewModel,
            TotalPages = 1,
            PageNum = 1,
            Filters = filters,
        };

        result.Should().BeEquivalentTo(expected);

        _supplierRepoMock.Verify(s => s.ToSupplierListAsync(It.IsAny<SupplierFilterViewModel>(), 1, 10, ""), Times.Once);
    }

    [Fact]
    public async Task SetPagedResultSupplierListViewModel_WhenHasNoItems_ShouldReturnEmptyList()
    {
        var filters = new SupplierFilterViewModel();

        _supplierRepoMock
            .Setup(s => s.ToSupplierListAsync(It.IsAny<SupplierFilterViewModel>(), 1, 10, ""))
            .ReturnsAsync(([], 0));

        var service = new SupplierService(
            _supplierRepoMock.Object,
            _collectRepoMock.Object
        );

        var result = await service.SetPagedResultSupplierListViewModel(filters, 1, 10, "");

        var expected = new PagedResultViewModel<SupplierListViewModel, SupplierFilterViewModel>
        {
            Items = [],
            TotalPages = 0,
            PageNum = 1,
            Filters = filters,
        };

        result.Should().BeEquivalentTo(expected);

        _supplierRepoMock.Verify(s => s.ToSupplierListAsync(It.IsAny<SupplierFilterViewModel>(), 1, 10, ""), Times.Once);
    }

    [Fact]
    public async Task CreateSupplier_WhenSupplierExistIsFalse_ShouldCreateSupplier()
    {
        _supplierRepoMock
            .Setup(s => s.AnySupplierAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(false);

        var createSupplierViewModel = new CreateSupplierViewModelBuilder().Build();

        var service = new SupplierService(
            _supplierRepoMock.Object,
            _collectRepoMock.Object
        );

        var result = await service.CreateSupplier(createSupplierViewModel);

        var expected = 
            new OperationResultBuilder()
            .WithSuccess(true)
            .Build();

        result.Should().BeEquivalentTo(expected);

        _supplierRepoMock.Verify(s => s.AnySupplierAsync(It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        _supplierRepoMock.Verify(s => s.AddSupplier(It.IsAny<Supplier>()), Times.Once);
        _supplierRepoMock.Verify(s => s.SaveChangesSupplierAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateSupplier_WhenSupplierExistIsTrue_ShouldNotCreateSupplier()
    {
        _supplierRepoMock
            .Setup(s => s.AnySupplierAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(true);

        var createSupplierViewModel = new CreateSupplierViewModelBuilder().Build();

        var service = new SupplierService(
            _supplierRepoMock.Object,
            _collectRepoMock.Object
        );

        var result = await service.CreateSupplier(createSupplierViewModel);

        var expected = 
            new OperationResultBuilder()
            .WithSuccess(false)
            .WithMessage("Já existe um fornecedor cadastrado com o CNPJ fornecido")
            .Build();

        result.Should().BeEquivalentTo(expected);

        _supplierRepoMock.Verify(s => s.AnySupplierAsync(It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        _supplierRepoMock.Verify(s => s.AddSupplier(It.IsAny<Supplier>()), Times.Never);
        _supplierRepoMock.Verify(s => s.SaveChangesSupplierAsync(), Times.Never);
    }

    [Fact]
    public async Task EditSupplier_WhenSupplierIsNull_ShouldNotEditSupplier()
    {
        _supplierRepoMock
            .Setup(s => s.GetSupplierByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Supplier?)null);

        var editSupplierViewModel = new EditSupplierViewModelBuilder().Build();

        var service = new SupplierService(
            _supplierRepoMock.Object,
            _collectRepoMock.Object
        );

        var result = await service.EditSupplier(editSupplierViewModel);

        result.Should().BeNull();

        _supplierRepoMock.Verify(s => s.GetSupplierByIdAsync(It.IsAny<int>()), Times.Once);
        _supplierRepoMock.Verify(s => s.SaveChangesSupplierAsync(), Times.Never);
    }

    [Fact]
    public async Task EditSupplier_WhenSupplierExistIsTrue_ShouldNotEditSupplier()
    {
        var supplier = new SupplierBuilder().Build();

        _supplierRepoMock
            .Setup(s => s.GetSupplierByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(supplier);

        _supplierRepoMock
            .Setup(s => s.AnySupplierAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(true);

        var editSupplierViewModel = new EditSupplierViewModelBuilder().Build();

        var service = new SupplierService(
            _supplierRepoMock.Object,
            _collectRepoMock.Object
        );

        var result = await service.EditSupplier(editSupplierViewModel);

        var expected = 
            new OperationResultBuilder()
            .WithSuccess(false)
            .WithMessage("Já existe um fornecedor cadastrado com o CNPJ fornecido")
            .Build();

        result.Should().BeEquivalentTo(expected);

        _supplierRepoMock.Verify(s => s.GetSupplierByIdAsync(It.IsAny<int>()), Times.Once);
        _supplierRepoMock.Verify(s => s.SaveChangesSupplierAsync(), Times.Never);
    }

    [Fact]
    public async Task EditSupplier_WhenSupplierIsNotNullAndSupplierExistIsFalse_ShouldNotEditSupplier()
    {
        var supplier = new SupplierBuilder().Build();

        _supplierRepoMock
            .Setup(s => s.GetSupplierByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(supplier);

        _supplierRepoMock
            .Setup(s => s.AnySupplierAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(false);

        var editSupplierViewModel = new EditSupplierViewModelBuilder().Build();

        var service = new SupplierService(
            _supplierRepoMock.Object,
            _collectRepoMock.Object
        );

        var result = await service.EditSupplier(editSupplierViewModel);

        var expected = 
            new OperationResultBuilder()
            .WithSuccess(true)
            .Build();

        result.Should().BeEquivalentTo(expected);
        supplier.Name.Should().Be(editSupplierViewModel.Name);
        supplier.CNPJ.Should().Be(editSupplierViewModel.CNPJ);
        supplier.Street.Should().Be(editSupplierViewModel.Street);
        supplier.Neighborhood.Should().Be(editSupplierViewModel.Neighborhood);
        supplier.Number.Should().Be(editSupplierViewModel.Number);
        supplier.City.Should().Be(editSupplierViewModel.City);
        supplier.State.Should().Be(editSupplierViewModel.State);
        supplier.ZipCode.Should().Be(editSupplierViewModel.ZipCode);

        _supplierRepoMock.Verify(s => s.GetSupplierByIdAsync(It.IsAny<int>()), Times.Once);
        _supplierRepoMock.Verify(s => s.SaveChangesSupplierAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteSupplier_WhenSupplierIsNull_ShouldNotDeleteSupplier()
    {
        _supplierRepoMock
            .Setup(s => s.GetSupplierByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Supplier?) null);

        var service = new SupplierService(
            _supplierRepoMock.Object,
            _collectRepoMock.Object
        );

        var result = await service.DeleteSupplier(1);

        result.Should().BeNull();

        _supplierRepoMock.Verify(s => s.GetSupplierByIdAsync(It.IsAny<int>()), Times.Once);
        _supplierRepoMock.Verify(s => s.RemoveSupplier(It.IsAny<Supplier>()), Times.Never);
        _supplierRepoMock.Verify(s => s.SaveChangesSupplierAsync(), Times.Never);
    }

    [Fact]
    public async Task DeleteSupplier_WhenExistSupplierWithCollectIsTrue_ShouldNotDeleteSupplier()
    {
        var supplier = new SupplierBuilder().Build();

        _supplierRepoMock
            .Setup(s => s.GetSupplierByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(supplier);

        _collectRepoMock
            .Setup(c => c.AnyCollectAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(true);

        var service = new SupplierService(
            _supplierRepoMock.Object,
            _collectRepoMock.Object
        );

        var result = await service.DeleteSupplier(1);

        var expected =
            new OperationResultBuilder()
            .WithSuccess(false)
            .WithMessage("Não foi possível deletar, existe uma coleta vinculada a este fornecedor")
            .Build();

        result.Should().BeEquivalentTo(expected);

        _supplierRepoMock.Verify(s => s.GetSupplierByIdAsync(It.IsAny<int>()), Times.Once);
        _supplierRepoMock.Verify(s => s.RemoveSupplier(It.IsAny<Supplier>()), Times.Never);
        _supplierRepoMock.Verify(s => s.SaveChangesSupplierAsync(), Times.Never);
    }

    [Fact]
    public async Task DeleteSupplier_WhenSupplierIsNotNullAndExistSupplierWithCollectIsFalse_ShouldNotDeleteSupplier()
    {
        var supplier = new SupplierBuilder().Build();

        _supplierRepoMock
            .Setup(s => s.GetSupplierByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(supplier);

        _collectRepoMock
            .Setup(c => c.AnyCollectAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(false);

        var service = new SupplierService(
            _supplierRepoMock.Object,
            _collectRepoMock.Object
        );

        var result = await service.DeleteSupplier(1);

        var expected =
            new OperationResultBuilder()
            .WithSuccess(true)
            .Build();

        result.Should().BeEquivalentTo(expected);

        _supplierRepoMock.Verify(s => s.GetSupplierByIdAsync(It.IsAny<int>()), Times.Once);
        _supplierRepoMock.Verify(s => s.RemoveSupplier(It.IsAny<Supplier>()), Times.Once);
        _supplierRepoMock.Verify(s => s.SaveChangesSupplierAsync(), Times.Once);
    }
}