using CollectApp.Models;
using CollectApp.Repositories;
using CollectApp.Services;
using CollectApp.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using Moq;

namespace CollectAppTests.Builders;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepoMock;

    public UserServiceTests()
    {
        _userRepoMock = new Mock<IUserRepository>();
    }

    [Fact]
    public async Task SetPagedResultUserListViewModel_WhenHasItems_ShouldReturnListWithItems()
    {
        var userList = new List<ApplicationUser>
        {
            new UserBuilder().Build(),
            new UserBuilder()
                .WithId("2")
                .WithFullName("Joba")
                .WithRole("Admin")
                .Build(),
            new UserBuilder()
                .WithId("3")
                .WithFullName("Jatobina")
                .WithRole("Gestor")
                .Build(),
        };

        var filters = new UserFilterViewModel();

        var userListViewModel = userList.Select(u =>
            new UserListViewModelBuilder()
            .FromUser(u)
            .Build())
            .ToList();

        _userRepoMock
            .Setup(u => u.ToUserListAsync(It.IsAny<UserFilterViewModel>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync((userList, 3));

        var service = new UserService(
            _userRepoMock.Object
        );

        var result = await service.SetPagedResultUserListViewModel(filters, 1, 10);

        var expected = new PagedResultViewModel<UserListViewModel, UserFilterViewModel>
        {
            Items = userListViewModel,
            TotalPages = 1,
            PageNum = 1,
            Filters = filters,
        };

        result.Should().BeEquivalentTo(expected);

        _userRepoMock.Verify(u => u.ToUserListAsync(It.IsAny<UserFilterViewModel>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task SetPagedResultUserListViewModel_WhenHasNoItems_ShouldReturnEmptyList()
    {
        var filters = new UserFilterViewModel();

        _userRepoMock
            .Setup(u => u.ToUserListAsync(It.IsAny<UserFilterViewModel>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(([], 0));

        var service = new UserService(
            _userRepoMock.Object
        );

        var result = await service.SetPagedResultUserListViewModel(filters, 1, 10);

        var expected = new PagedResultViewModel<UserListViewModel, UserFilterViewModel>
        {
            Items = [],
            TotalPages = 0,
            PageNum = 1,
            Filters = filters,
        };

        result.Should().BeEquivalentTo(expected);

        _userRepoMock.Verify(u => u.ToUserListAsync(It.IsAny<UserFilterViewModel>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task ChangeUserStatus_WhenUserIsNull_ShouldNotChangeUserStatus()
    {
        var user = new UserBuilder().Build();

        _userRepoMock
            .Setup(u => u.GetUserByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((ApplicationUser?)null);

        var service = new UserService(
            _userRepoMock.Object
        );

        await service.ChangeUserStatus(user.Id);

        user.Status.Should().Be(UserStatus.Ativo);

        _userRepoMock.Verify(u => u.GetUserByIdAsync(It.IsAny<string>()), Times.Once);
        _userRepoMock.Verify(u => u.UnlockOutUserAsync(It.IsAny<ApplicationUser>()), Times.Never);
        _userRepoMock.Verify(u => u.LockOutUserAsync(It.IsAny<ApplicationUser>()), Times.Never);
        _userRepoMock.Verify(u => u.SaveChangesUserAsync(It.IsAny<ApplicationUser>()), Times.Never);
    }

    [Fact]
    public async Task ChangeUserStatus_WhenUserStatusIsAtivo_ShouldChangeUserStatusToInativo()
    {
        var user = new UserBuilder().Build();

        _userRepoMock
            .Setup(u => u.GetUserByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        var service = new UserService(
            _userRepoMock.Object
        );

        await service.ChangeUserStatus(user.Id);

        user.Status.Should().Be(UserStatus.Inativo);

        _userRepoMock.Verify(u => u.GetUserByIdAsync(It.IsAny<string>()), Times.Once);
        _userRepoMock.Verify(u => u.UnlockOutUserAsync(It.IsAny<ApplicationUser>()), Times.Never);
        _userRepoMock.Verify(u => u.LockOutUserAsync(It.IsAny<ApplicationUser>()), Times.Once);
        _userRepoMock.Verify(u => u.SaveChangesUserAsync(It.IsAny<ApplicationUser>()), Times.Once);
    }
}