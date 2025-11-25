using CollectApp.Models;
using CollectApp.Repositories;
using CollectApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace CollectApp.Fixtures;

public class CollectServiceFixture
{
    public Mock<ICollectRepository> _collectRepoMock { get; } = new Mock<ICollectRepository>();
    public Mock<ISupplierRepository> _supplierRepoMock { get; } = new Mock<ISupplierRepository>();
    public Mock<IProductRepository> _productRepoMock { get; } = new Mock<IProductRepository>();
    public Mock<IFilialRepository> _filialRepoMock { get; } = new Mock<IFilialRepository>();
    public Mock<IUserStore<ApplicationUser>> _userStoreMock { get; } = new Mock<IUserStore<ApplicationUser>>();
    public Mock<UserManager<ApplicationUser>> _userManagerMock { get; } = new();
    public Mock<ICurrentUserService> _currentUserServiceMock { get; } = new Mock<ICurrentUserService>();
    public Mock<IAuthorizationService> _authorizationServiceMock { get; } = new Mock<IAuthorizationService>();
    public Mock<ILogger<CollectService>> _loggerMock { get; } = new Mock<ILogger<CollectService>>();

    public CollectServiceFixture()
    {
        _userManagerMock = new Mock<UserManager<ApplicationUser>>(_userStoreMock.Object, null!, null!, null!, null!, null!, null!, null!, null!);
    }
}