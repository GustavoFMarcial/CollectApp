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

        _collectRepoMock.Setup(c => c.ToCollectListAsync(It.IsAny<CollectFilterViewModel>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(([collect], 1));

        _currentUserServiceMock.Setup(c => c.User).Returns(new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, "user123") })));

        _authorizationServiceMock.Setup(a => a.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), It.IsAny<string>()))
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

        result.Should().BeEquivalentTo(expected);

        _collectRepoMock.Verify(c => c.ToCollectListAsync(It.IsAny<CollectFilterViewModel>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        _authorizationServiceMock.Verify(a => a.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), It.IsAny<string>()), Times.Exactly(2));
    }

    [Fact]
    public async Task SetPagedResultCollectListViewModel_WhenHasNoFilters_ShouldReturnNotFilteredList()
    {
        var collectList = new List<Collect>
        {
            new CollectBuilder().Build(),
            new CollectBuilder()
                .WithId(2)
                .WithCreatedAt(new DateTime(2025, 11, 21))
                .WithUserId("user1")
                .WithUser(u => u
                    .WithId("user1")
                    .WithFullName("Gustavo F. Marcial")
                    .WithRole("Gestor")
                    .WithCreatedAt(new DateTime(2025, 10, 14)))
                .WithSupplierId(2)
                .WithSupplier(s => s
                    .WithId(2)
                    .WithCNPJ("41192029000108")
                    .WithName("GMS")
                    .WithCity("Cuiabá")
                    .WithNumber("123")
                    .WithState("MT"))
                .WithCollectAt(new DateTime(2025, 11, 25))
                .WithProductId(2)
                .WithProduct(p => p
                    .WithId(2)
                    .WithCreatedAt(new DateTime(2024, 1, 20))
                    .WithName("Ferragem")
                    )
                .WithVolume(20)
                .WithWeight(40)
                .WithFilialId(2)
                .WithFilial(f => f
                    .WithId(2)
                    .WithName("Gênesis 08"))
                .WithStatus(CollectStatus.Coletado)
                .Build(),
            new CollectBuilder()
                .WithId(2)
                .WithCreatedAt(new DateTime(2025, 11, 22))
                .WithUserId("user2")
                .WithUser(u => u
                    .WithId("user2")
                    .WithFullName("Guilherme F. F. Marcial")
                    .WithRole("Admin")
                    .WithCreatedAt(new DateTime(2025, 9, 12)))
                .WithSupplierId(3)
                .WithSupplier(s => s
                    .WithId(3)
                    .WithCNPJ("41192029000280")
                    .WithName("Texa")
                    .WithCity("Várzea Grande")
                    .WithNumber("321")
                    .WithState("MT"))
                .WithCollectAt(new DateTime(2025, 11, 28))
                .WithProductId(3)
                .WithProduct(p => p
                    .WithId(3)
                    .WithCreatedAt(new DateTime(2024, 2, 12))
                    .WithName("Alumínio")
                    )
                .WithVolume(500)
                .WithWeight(999)
                .WithFilialId(3)
                .WithFilial(f => f
                    .WithId(3)
                    .WithName("Gênesis 80"))
                .WithStatus(CollectStatus.Cancelado)
                .Build(),
        };

        var collectListViewModel = new List<CollectListViewModel>
        {
            new CollectListViewModelBuilder().FromCollect(collectList[0]).Build(),
            new CollectListViewModelBuilder().FromCollect(collectList[1]).Build(),
            new CollectListViewModelBuilder().FromCollect(collectList[2]).Build(),
        };

        var filters = new CollectFilterViewModel();

        _collectRepoMock.Setup(c => c.ToCollectListAsync(It.IsAny<CollectFilterViewModel>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync((collectList, 3));

        _currentUserServiceMock.Setup(c => c.User).Returns(new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, "user123") })));

        _authorizationServiceMock.Setup(a => a.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), It.IsAny<string>()))
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
            Items = collectListViewModel,
            TotalPages = 1,
            PageNum = 1,
            Filters = filters
        };

        result.Should().BeEquivalentTo(expected);

        _collectRepoMock.Verify(c => c.ToCollectListAsync(It.IsAny<CollectFilterViewModel>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        _authorizationServiceMock.Verify(a => a.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), It.IsAny<string>()), Times.Exactly(6));
    }
}
