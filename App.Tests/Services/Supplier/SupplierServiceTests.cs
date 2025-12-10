using CollectApp.Models;
using CollectApp.Repositories;
using CollectApp.Services;
using CollectApp.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Filters;
using Moq;

namespace CollectAppTests.Builders;

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
}