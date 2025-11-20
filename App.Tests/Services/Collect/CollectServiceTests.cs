using Moq;
using CollectApp.ViewModels;
using CollectApp.Repositories;
using CollectApp.Models;
using CollectApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
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
        var filters = new CollectFilterViewModel
        {
            Id = 1,
            StartCreation = new DateTime(2024, 1, 15, 10, 30, 0),
            EndCreation = new DateTime(2024, 1, 15, 10, 30, 0),
            User = "João Silva",
            Supplier = "Fornecedor ABC Ltda",
            StartCollect = new DateTime(2024, 1, 20, 14, 0, 0),
            EndCollect = new DateTime(2024, 1, 20, 14, 0, 0),
            Product = "Papel Reciclável",
            Status = CollectStatus.PendenteAprovar,
            Volume = 100,
            Weight = 50,
            Filial = "Filial SP Centro",
        };

        var collectList = new List<Collect>
        {
            new Collect
            {
                Id = 1,
                CreatedAt = new DateTime(2024, 1, 15, 10, 30, 0),
                UserId = "user123",
                User = new ApplicationUser
                {
                    Id = "user123",
                    FullName = "João Silva",
                    Role = "Coletor",
                    Status = UserStatus.Ativo,
                    CreatedAt = new DateTime(2023, 6, 1)
                },
                SupplierId = 1,
                Supplier = new Supplier
                {
                    Id = 1,
                    CreatedAt = new DateTime(2023, 5, 10),
                    CNPJ = "12.345.678/0001-90",
                    Name = "Fornecedor ABC Ltda",
                    Street = "Rua das Flores",
                    Neighborhood = "Centro",
                    Number = "100",
                    City = "São Paulo",
                    State = "SP",
                    ZipCode = "01234-567"
                },
                CollectAt = new DateTime(2024, 1, 20, 14, 0, 0),
                ProductId = 1,
                Product = new Product
                {
                    Id = 1,
                    CreatedAt = new DateTime(2023, 3, 15),
                    Name = "Papel Reciclável"
                },
                Volume = 100,
                Weigth = 50,
                FilialId = 1,
                Filial = new Filial
                {
                    Id = 1,
                    Name = "Filial SP Centro"
                },
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

        Assert.Equal(expected.TotalPages, result.TotalPages);
        Assert.Equal(expected.PageNum, result.PageNum);
        Assert.Equal(expected.Items.Count, result.Items.Count);

        for (int i = 0; i < expected.Items.Count; i++)
        {
            Assert.Equal(expected.Items[i].Id, result.Items[i].Id);
            Assert.Equal(expected.Items[i].CreatedAt, result.Items[i].CreatedAt);
            Assert.Equal(expected.Items[i].UserId, result.Items[i].UserId);
            Assert.Equal(expected.Items[i].FullName, result.Items[i].FullName);
            Assert.Equal(expected.Items[i].SupplierName, result.Items[i].SupplierName);
            Assert.Equal(expected.Items[i].CollectAt, result.Items[i].CollectAt);
            Assert.Equal(expected.Items[i].ProductDescription, result.Items[i].ProductDescription);
            Assert.Equal(expected.Items[i].Volume, result.Items[i].Volume);
            Assert.Equal(expected.Items[i].Weigth, result.Items[i].Weigth);
            Assert.Equal(expected.Items[i].Filial, result.Items[i].Filial);
            Assert.Equal(expected.Items[i].Status, result.Items[i].Status);
            Assert.Equal(expected.Items[i].ChangeCollect.Id, result.Items[i].ChangeCollect.Id);
            Assert.Equal(expected.Items[i].ChangeCollect.Status, result.Items[i].ChangeCollect.Status);
            Assert.Equal(expected.Items[i].ChangeCollect.ToOpen, result.Items[i].ChangeCollect.ToOpen);
            Assert.Equal(expected.Items[i].ChangeCollect.UserId, result.Items[i].ChangeCollect.UserId);
            Assert.Equal(expected.Items[i].ChangeCollect.CanChangeCollectStatus, result.Items[i].ChangeCollect.CanChangeCollectStatus);
            Assert.Equal(expected.Items[i].ChangeCollect.CanEditOpenOrDeleteCollect, result.Items[i].ChangeCollect.CanEditOpenOrDeleteCollect);
        }
    }
}
