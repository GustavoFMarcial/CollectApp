using CollectApp.Models;
using CollectApp.Repositories;
using CollectApp.Services;
using CollectApp.ViewModels;
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
}