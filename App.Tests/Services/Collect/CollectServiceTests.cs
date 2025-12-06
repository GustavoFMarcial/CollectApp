using Moq;
using CollectApp.ViewModels;
using CollectApp.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using FluentAssertions;
using CollectAppTests.Builders;
using CollectApp.Models;
using CollectApp.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
namespace CollectAppTests.Services;

public class CollectServiceTests
{
    private readonly Mock<ICollectRepository> _collectRepoMock;
    private readonly Mock<ISupplierRepository> _supplierRepoMock;
    private readonly Mock<IProductRepository> _productRepoMock;
    private readonly Mock<IFilialRepository> _filialRepoMock;
    private readonly Mock<IUserStore<ApplicationUser>> _userStoreMock;
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly Mock<IAuthorizationService> _authorizationServiceMock;
    private readonly Mock<ILogger<CollectService>> _loggerMock;

    public CollectServiceTests()
    {
        _collectRepoMock = new Mock<ICollectRepository>();
        _supplierRepoMock = new Mock<ISupplierRepository>();
        _productRepoMock = new Mock<IProductRepository>();
        _filialRepoMock = new Mock<IFilialRepository>();
        _userStoreMock = new Mock<IUserStore<ApplicationUser>>();
        _userManagerMock = new Mock<UserManager<ApplicationUser>>(_userStoreMock.Object, null!, null!, null!, null!, null!, null!, null!, null!);
        _currentUserServiceMock = new Mock<ICurrentUserService>();
        _authorizationServiceMock = new Mock<IAuthorizationService>();
        _loggerMock = new Mock<ILogger<CollectService>>();
    }

    [Fact]
    public async Task SetPagedResultCollectListViewModel_WhenHasFilters_ShouldReturnFilteredList()
    {
        var collect = new CollectBuilder().Build();

        var collectListViewModel = new CollectListViewModelBuilder().FromCollect(collect).Build();

        var filters = new CollectFilterViewModelBuilder().Build();

        _collectRepoMock
            .Setup(c => c.ToCollectListAsync(filters, 1, 10))
            .ReturnsAsync(([collect], 1));

        _currentUserServiceMock
            .Setup(c => c.User)
            .Returns(new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, "1") })));

        _authorizationServiceMock
            .Setup(a => a.AuthorizeAsync(_currentUserServiceMock.Object.User, null, "CanChangeCollectStatus"))
            .ReturnsAsync(AuthorizationResult.Success());

        _authorizationServiceMock
            .Setup(a => a.AuthorizeAsync(_currentUserServiceMock.Object.User, collect.UserId, "MustBeCollectOwner"))
            .ReturnsAsync(AuthorizationResult.Success());

        var service = new CollectService(
            _collectRepoMock.Object,
            _supplierRepoMock.Object,
            _productRepoMock.Object,
            _filialRepoMock.Object,
            _userManagerMock.Object,
            _currentUserServiceMock.Object,
            _authorizationServiceMock.Object,
            _loggerMock.Object);

        var result = await service.SetPagedResultCollectListViewModel(filters, 1, 10);

        var expected = new PagedResultViewModel<CollectListViewModel, CollectFilterViewModel>
        {
            Items = [collectListViewModel],
            TotalPages = 1,
            PageNum = 1,
            Filters = filters
        };

        result.Should().BeEquivalentTo(expected, opt => opt.ExcludingMissingMembers());


        _collectRepoMock.Verify(c => c.ToCollectListAsync(filters, 1, 10), Times.Once);
        _authorizationServiceMock.Verify(a => a.AuthorizeAsync(_currentUserServiceMock.Object.User, null, "CanChangeCollectStatus"), Times.Exactly(1));
        _authorizationServiceMock.Verify(a => a.AuthorizeAsync(_currentUserServiceMock.Object.User, collect.UserId, "MustBeCollectOwner"), Times.Exactly(1));
    }

    [Fact]
    public async Task SetPagedResultCollectListViewModel_WhenHasNoFilters_ShouldReturnNotFilteredList()
    {
        var collectList = new List<Collect>
        {
            new CollectBuilder().Build(),
            new CollectBuilder()
                .WithUserId("2")
                .WithUser(u => u.WithId("2"))
                .Build(),
            new CollectBuilder()
                .WithUserId("3")
                .WithUser(u => u.WithId("3"))
                .Build(),
        };

        List<CollectListViewModel> collectListViewModel = collectList
            .Select(c => new CollectListViewModelBuilder().FromCollect(c).Build())
            .ToList();

        var filters = new CollectFilterViewModel();

        _collectRepoMock
            .Setup(c => c.ToCollectListAsync(filters, 1, 10))
            .ReturnsAsync((collectList, 3));

        _currentUserServiceMock
            .Setup(c => c.User).Returns(new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, "1") })));

        _authorizationServiceMock
            .Setup(a => a.AuthorizeAsync(_currentUserServiceMock.Object.User, null, "CanChangeCollectStatus"))
            .ReturnsAsync(AuthorizationResult.Success());

        foreach (Collect c in collectList)
        {
            _authorizationServiceMock.Setup(a => a.AuthorizeAsync(_currentUserServiceMock.Object.User, c.UserId, "MustBeCollectOwner"))
            .ReturnsAsync(AuthorizationResult.Success());
        }

        var service = new CollectService(
            _collectRepoMock.Object,
            _supplierRepoMock.Object,
            _productRepoMock.Object,
            _filialRepoMock.Object,
            _userManagerMock.Object,
            _currentUserServiceMock.Object,
            _authorizationServiceMock.Object,
            _loggerMock.Object);

        var result = await service.SetPagedResultCollectListViewModel(filters, 1, 10);

        var expected = new PagedResultViewModel<CollectListViewModel, CollectFilterViewModel>
        {
            Items = collectListViewModel,
            TotalPages = 1,
            PageNum = 1,
            Filters = filters
        };

        result.Should().BeEquivalentTo(expected);
        result.Items.Should().HaveCount(3);

        _authorizationServiceMock.Verify(a => a.AuthorizeAsync(_currentUserServiceMock.Object.User, null, "CanChangeCollectStatus"), Times.Exactly(3));
        foreach (Collect c in collectList)
        {
            _authorizationServiceMock.Verify(a => a.AuthorizeAsync(_currentUserServiceMock.Object.User, c.UserId, "MustBeCollectOwner"), Times.Exactly(1));
        }
    }

    [Fact]
    public async Task CreateCollect_WhenHasAllEntities_ShouldCreateCollect()
    {
        var createCollectViewModel = new CreateCollectViewModelBuilder().Build();
        var supplier = new SupplierBuilder().Build();
        var product = new ProductBuilder().Build();
        var filial = new FilialBuilder().Build();
        var userId = "1";
        var user = new UserBuilder().Build();
        Collect? collectSent = null;

        _supplierRepoMock
            .Setup(s => s.GetSupplierByIdAsync(createCollectViewModel.SupplierId))
            .ReturnsAsync(supplier);

        _productRepoMock
            .Setup(p => p.GetProductByIdAsync(createCollectViewModel.ProductId))
            .ReturnsAsync(product);

        _filialRepoMock
            .Setup(f => f.GetFilialByIdAsync(createCollectViewModel.FilialId))
            .ReturnsAsync(filial);

        _userManagerMock
            .Setup(u => u.FindByIdAsync(userId))
            .ReturnsAsync(user);

        _collectRepoMock
            .Setup(c => c.AddCollect(It.IsAny<Collect>()))
            .Callback<Collect>(c => collectSent = c);

        var service = new CollectService(
            _collectRepoMock.Object,
            _supplierRepoMock.Object,
            _productRepoMock.Object,
            _filialRepoMock.Object,
            _userManagerMock.Object,
            _currentUserServiceMock.Object,
            _authorizationServiceMock.Object,
            _loggerMock.Object);

        await service.CreateCollect(createCollectViewModel, userId);

        var expected = new CollectBuilder().Build();

        collectSent.Should().BeEquivalentTo(expected, options =>
            options.Excluding(c => c.CreatedAt)
                   .Excluding(c => c.User.SecurityStamp)
                   .Excluding(c => c.User.ConcurrencyStamp));

        _supplierRepoMock.Verify(s => s.GetSupplierByIdAsync(createCollectViewModel.SupplierId), Times.Once);
        _productRepoMock.Verify(p => p.GetProductByIdAsync(createCollectViewModel.ProductId), Times.Once);
        _filialRepoMock.Verify(f => f.GetFilialByIdAsync(createCollectViewModel.FilialId), Times.Once);
        _userManagerMock.Verify(u => u.FindByIdAsync(userId), Times.Once);
        _collectRepoMock.Verify(c => c.AddCollect(It.IsAny<Collect>()), Times.Once);
        _collectRepoMock.Verify(c => c.SaveChangesCollectAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateCollect_WhenHasNoEntities_ShouldNotCreateCollect()
    {
        var createCollectViewModel = new CreateCollectViewModelBuilder().Build();
        var supplier = new SupplierBuilder().Build();
        var product = new ProductBuilder().Build();
        var filial = new FilialBuilder().Build();
        var userId = "1";
        var user = new UserBuilder().Build();
        Collect? collectSent = null;

        _supplierRepoMock
            .Setup(s => s.GetSupplierByIdAsync(createCollectViewModel.SupplierId))
            .ReturnsAsync((Supplier?)null);

        _productRepoMock
            .Setup(p => p.GetProductByIdAsync(createCollectViewModel.ProductId))
            .ReturnsAsync(product);

        _filialRepoMock
            .Setup(f => f.GetFilialByIdAsync(createCollectViewModel.FilialId))
            .ReturnsAsync(filial);

        _userManagerMock
            .Setup(u => u.FindByIdAsync(userId))
            .ReturnsAsync(user);

        _collectRepoMock
            .Setup(c => c.AddCollect(It.IsAny<Collect>()))
            .Callback<Collect>(c => collectSent = c);

        var service = new CollectService(
            _collectRepoMock.Object,
            _supplierRepoMock.Object,
            _productRepoMock.Object,
            _filialRepoMock.Object,
            _userManagerMock.Object,
            _currentUserServiceMock.Object,
            _authorizationServiceMock.Object,
            _loggerMock.Object);

        await service.CreateCollect(createCollectViewModel, userId);

        var expected = new CollectBuilder().Build();

        collectSent.Should().BeNull();

        _supplierRepoMock.Verify(s => s.GetSupplierByIdAsync(createCollectViewModel.SupplierId), Times.Once);
        _productRepoMock.Verify(p => p.GetProductByIdAsync(createCollectViewModel.ProductId), Times.Once);
        _filialRepoMock.Verify(f => f.GetFilialByIdAsync(createCollectViewModel.FilialId), Times.Once);
        _userManagerMock.Verify(u => u.FindByIdAsync(userId), Times.Once);
        _collectRepoMock.Verify(c => c.AddCollect(It.IsAny<Collect>()), Times.Never);
        _collectRepoMock.Verify(c => c.SaveChangesCollectAsync(), Times.Never);
    }

    [Fact]
    public async Task SetEditCollectViewModel_WhenCollectFound_ShouldReturnCollectToEdit()
    {
        var collect = new CollectBuilder().Build();

        _collectRepoMock.Setup(c => c.GetCollectByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(collect);

        var service = new CollectService(
            _collectRepoMock.Object,
            _supplierRepoMock.Object,
            _productRepoMock.Object,
            _filialRepoMock.Object,
            _userManagerMock.Object,
            _currentUserServiceMock.Object,
            _authorizationServiceMock.Object,
            _loggerMock.Object);

        var result = await service.SetEditCollectViewModel(1);

        var expected = new EditCollectViewModelBuilder().FromCollect(collect).Build();

        result.Should().BeEquivalentTo(expected);

        _collectRepoMock.Verify(c => c.GetCollectByIdAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task SetEditCollectViewModel_WhenCollectNotFound_ShouldNotReturnCollectToEdit()
    {
        var collect = new CollectBuilder().Build();

        _collectRepoMock.Setup(c => c.GetCollectByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Collect?)null);

        var service = new CollectService(
            _collectRepoMock.Object,
            _supplierRepoMock.Object,
            _productRepoMock.Object,
            _filialRepoMock.Object,
            _userManagerMock.Object,
            _currentUserServiceMock.Object,
            _authorizationServiceMock.Object,
            _loggerMock.Object);

        var result = await service.SetEditCollectViewModel(1);

        result.Should().BeNull();

        _collectRepoMock.Verify(c => c.GetCollectByIdAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task EditCollect_WhenEntitiesFound_ShouldEditCollect()
    {
        var collect = new CollectBuilder().Build();
        var supplier = new SupplierBuilder().Build();
        var product = new ProductBuilder().Build();
        var filial = new FilialBuilder().Build();
        var editCollectViewModel = new EditCollectViewModelBuilder()
            .WithId(1)
            .WithVolume(50)
            .WithWeight(100)
            .WithSupplierId(1)
            .WithProductId(1)
            .WithFilialId(1)
            .Build();

        _collectRepoMock.Setup(c => c.GetCollectByIdAsync(1))
            .ReturnsAsync(collect);

        _supplierRepoMock.Setup(s => s.GetSupplierByIdAsync(1))
            .ReturnsAsync(supplier);

        _productRepoMock.Setup(p => p.GetProductByIdAsync(1))
            .ReturnsAsync(product);

        _filialRepoMock.Setup(f => f.GetFilialByIdAsync(1))
            .ReturnsAsync(filial);

        var service = new CollectService(
            _collectRepoMock.Object,
            _supplierRepoMock.Object,
            _productRepoMock.Object,
            _filialRepoMock.Object,
            _userManagerMock.Object,
            _currentUserServiceMock.Object,
            _authorizationServiceMock.Object,
            _loggerMock.Object);

        await service.EditCollect(editCollectViewModel);

        collect.Volume.Should().Be(50);
        collect.Weight.Should().Be(100);

        _collectRepoMock.Verify(c => c.GetCollectByIdAsync(1), Times.Once);
        _supplierRepoMock.Verify(s => s.GetSupplierByIdAsync(1), Times.Once);
        _productRepoMock.Verify(p => p.GetProductByIdAsync(1), Times.Once);
        _filialRepoMock.Verify(f => f.GetFilialByIdAsync(1), Times.Once);
        _collectRepoMock.Verify(c => c.SaveChangesCollectAsync(), Times.Once);
    }

    [Fact]
    public async Task EditCollect_WhenEntitiesNotFound_ShouldNotEditCollect()
    {
        var collect = new CollectBuilder().Build();
        var supplier = new SupplierBuilder().Build();
        var product = new ProductBuilder().Build();
        var filial = new FilialBuilder().Build();
        var editCollectViewModel = new EditCollectViewModelBuilder()
            .WithId(1)
            .WithVolume(50)
            .WithWeight(100)
            .WithSupplierId(1)
            .WithProductId(1)
            .WithFilialId(1)
            .Build();

        _collectRepoMock.Setup(c => c.GetCollectByIdAsync(1))
            .ReturnsAsync((Collect?)null);

        _supplierRepoMock.Setup(s => s.GetSupplierByIdAsync(1))
            .ReturnsAsync(supplier);

        _productRepoMock.Setup(p => p.GetProductByIdAsync(1))
            .ReturnsAsync(product);

        _filialRepoMock.Setup(f => f.GetFilialByIdAsync(1))
            .ReturnsAsync(filial);

        var service = new CollectService(
            _collectRepoMock.Object,
            _supplierRepoMock.Object,
            _productRepoMock.Object,
            _filialRepoMock.Object,
            _userManagerMock.Object,
            _currentUserServiceMock.Object,
            _authorizationServiceMock.Object,
            _loggerMock.Object);

        await service.EditCollect(editCollectViewModel);

        collect.Volume.Should().Be(100);
        collect.Weight.Should().Be(50);

        _collectRepoMock.Verify(c => c.GetCollectByIdAsync(1), Times.Once);
        _supplierRepoMock.Verify(s => s.GetSupplierByIdAsync(1), Times.Never);
        _productRepoMock.Verify(p => p.GetProductByIdAsync(1), Times.Never);
        _filialRepoMock.Verify(f => f.GetFilialByIdAsync(1), Times.Never);
        _collectRepoMock.Verify(c => c.SaveChangesCollectAsync(), Times.Never);
    }

    [Fact]
    public async Task UpdateCollectStatus_WhenCollectStatusIsPendenterAprovarAndToOpenIsFalse_ShouldChangeCollectStatusToPendenteColetar()
    {
        var collect = new CollectBuilder().Build();
        var changeCollectViewModel = new ChangeCollectViewModelBuilder().FromCollect(collect).Build();

        _collectRepoMock.Setup(c => c.GetCollectByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(collect);

        var service = new CollectService(
            _collectRepoMock.Object,
            _supplierRepoMock.Object,
            _productRepoMock.Object,
            _filialRepoMock.Object,
            _userManagerMock.Object,
            _currentUserServiceMock.Object,
            _authorizationServiceMock.Object,
            _loggerMock.Object);

        await service.UpdateCollectStatus(changeCollectViewModel);

        collect.Status.Should().Be(CollectStatus.PendenteColetar);

        _collectRepoMock.Verify(c => c.GetCollectByIdAsync(It.IsAny<int>()), Times.Once);
        _collectRepoMock.Verify(c => c.SaveChangesCollectAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateCollectStatus_WhenCollectStatusIsPendenterColetarAndToOpenIsTrue_ShouldChangeCollectStatusToPendenteAprovar()
    {
        var collect = new CollectBuilder().WithStatus(CollectStatus.PendenteColetar).Build();
        var changeCollectViewModel = new ChangeCollectViewModelBuilder().WithToOpen(true).FromCollect(collect).Build();

        _collectRepoMock.Setup(c => c.GetCollectByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(collect);

        var service = new CollectService(
            _collectRepoMock.Object,
            _supplierRepoMock.Object,
            _productRepoMock.Object,
            _filialRepoMock.Object,
            _userManagerMock.Object,
            _currentUserServiceMock.Object,
            _authorizationServiceMock.Object,
            _loggerMock.Object);

        await service.UpdateCollectStatus(changeCollectViewModel);

        collect.Status.Should().Be(CollectStatus.PendenteAprovar);

        _collectRepoMock.Verify(c => c.GetCollectByIdAsync(It.IsAny<int>()), Times.Once);
        _collectRepoMock.Verify(c => c.SaveChangesCollectAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateCollectStatus_WhenCollectStatusIsColetado_ShouldNotChangeCollectStatus()
    {
        var collect = new CollectBuilder().WithStatus(CollectStatus.Coletado).Build();
        var changeCollectViewModel = new ChangeCollectViewModelBuilder().FromCollect(collect).Build();

        _collectRepoMock.Setup(c => c.GetCollectByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(collect);

        var service = new CollectService(
            _collectRepoMock.Object,
            _supplierRepoMock.Object,
            _productRepoMock.Object,
            _filialRepoMock.Object,
            _userManagerMock.Object,
            _currentUserServiceMock.Object,
            _authorizationServiceMock.Object,
            _loggerMock.Object);

        await service.UpdateCollectStatus(changeCollectViewModel);

        collect.Status.Should().Be(CollectStatus.Coletado);

        _collectRepoMock.Verify(c => c.GetCollectByIdAsync(It.IsAny<int>()), Times.Once);
        _collectRepoMock.Verify(c => c.SaveChangesCollectAsync(), Times.Never);
    }

    [Fact]
    public async Task CancelCollect_WhenCollectStatusIsPendenteAprovar_ShouldChangeCollectStatusToCancelado()
    {
        var collect = new CollectBuilder().Build();

        _collectRepoMock.Setup(c => c.GetCollectByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(collect);

        var service = new CollectService(
            _collectRepoMock.Object,
            _supplierRepoMock.Object,
            _productRepoMock.Object,
            _filialRepoMock.Object,
            _userManagerMock.Object,
            _currentUserServiceMock.Object,
            _authorizationServiceMock.Object,
            _loggerMock.Object);

        await service.CancelCollect(1);

        collect.Status.Should().Be(CollectStatus.Cancelado);

        _collectRepoMock.Verify(c => c.GetCollectByIdAsync(It.IsAny<int>()), Times.Once);
        _collectRepoMock.Verify(c => c.SaveChangesCollectAsync(), Times.Once);
    }

    [Fact]
    public async Task CancelCollect_WhenCollectStatusIsNotPendenteAprovar_ShouldNotChangeCollectStatusToCancelado()
    {
        var collect = new CollectBuilder().WithStatus(CollectStatus.PendenteColetar).Build();

        _collectRepoMock.Setup(c => c.GetCollectByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(collect);

        var service = new CollectService(
            _collectRepoMock.Object,
            _supplierRepoMock.Object,
            _productRepoMock.Object,
            _filialRepoMock.Object,
            _userManagerMock.Object,
            _currentUserServiceMock.Object,
            _authorizationServiceMock.Object,
            _loggerMock.Object);

        await service.CancelCollect(1);

        collect.Status.Should().NotBe(CollectStatus.Cancelado);

        _collectRepoMock.Verify(c => c.GetCollectByIdAsync(It.IsAny<int>()), Times.Once);
        _collectRepoMock.Verify(c => c.SaveChangesCollectAsync(), Times.Never);
    }
}
