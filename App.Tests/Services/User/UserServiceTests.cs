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
}