using Moq;
using CollectApp.ViewModels;
using CollectApp.Repositories;
using CollectApp.Models;
using CollectApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using FluentAssertions;
using CollectAppTests.Builders;
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
    public async Task SetPagedResultCollectListViewModel_WhenHasItems_ReturnsCorrectValue()
    {

        CollectFilterViewModel filters = new CollectFilterViewModelBuilder().Build();

        var collectList = new List<Collect>
        {
            new Collect
            {
                Id = 1,
                CreatedAt = new DateTime(2024, 1, 15, 10, 30, 0),
                UserId = "user123",
                User = new UserBuilder().Build(),
                SupplierId = 1,
                Supplier = new SupplierBuilder().Build(),
                CollectAt = new DateTime(2024, 1, 20, 14, 0, 0),
                ProductId = 1,
                Product = new ProductBuilder().Build(),
                Volume = 100,
                Weigth = 50,
                FilialId = 1,
                Filial = new FilialBuilder().Build(),
                Status = CollectStatus.PendenteAprovar
            },
        };

        var collectListViewModel = new List<CollectListViewModel>
        {
            new CollectListViewModel
            {
                Id = 1,
                CreatedAt = new DateTime(2024, 1, 15, 10, 30, 0),
                UserId = "user123",
                FullName = "João Silva",
                SupplierName = "Fornecedor ABC Ltda",
                CollectAt = new DateTime(2024, 1, 20, 14, 0, 0),
                ProductDescription = "Papel Reciclável",
                Volume = 100,
                Weigth = 50,
                Filial = "Filial SP Centro",
                Status = CollectStatus.PendenteAprovar,
                ChangeCollect = new ChangeCollectViewModel
                {
                    Id = 1,
                    Status = CollectStatus.PendenteAprovar,
                    ToOpen = false,
                    UserId = "user123",
                    CanChangeCollectStatus = true,
                    CanEditOpenOrDeleteCollect = true
                }
            }
        };

        _collectRepoMock.Setup(c => c.ToCollectListAsync(It.IsAny<CollectFilterViewModel>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync((collectList, 1));

        var _userManagerMock = new Mock<UserManager<ApplicationUser>>(_userStoreMock.Object, null!, null!, null!, null!, null!, null!, null!, null!);

        _currentUserServiceMock.Setup(c => c.User).Returns(new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, "user123") })));

        _authorizationServiceMock.Setup(a => a.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), "CanChangeCollectStatus"))
            .ReturnsAsync(AuthorizationResult.Success());

        _authorizationServiceMock.Setup(a => a.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), "MustBeCollectOwner"))
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
            Filters = filters,
        };

        result.Should().BeEquivalentTo(expected);
    }
}
