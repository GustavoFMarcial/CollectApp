using Moq;
using CollectApp.ViewModels;
using CollectApp.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using FluentAssertions;
using CollectAppTests.Builders;
using CollectApp.Fixtures;
namespace CollectAppTests.Services;

public class CollectServiceTests  : IClassFixture<CollectServiceFixture>
{
    private readonly CollectServiceFixture _fx;

    public CollectServiceTests(CollectServiceFixture fx)
    {
        _fx = fx;
    }

    [Fact]
    public async Task SetPagedResultCollectListViewModel_ShouldReturnPagedResultWithMappedItems()
    {
        var collect = new CollectBuilder().Build();

        var collectListViewModel = new CollectListViewModelBuilder().FromCollect(collect).Build();

        var filters = new CollectFilterViewModelBuilder().Build();

        _fx._collectRepoMock.Setup(c => c.ToCollectListAsync(It.IsAny<CollectFilterViewModel>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(([collect], 1));

        _fx._currentUserServiceMock.Setup(c => c.User).Returns(new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, "user123") })));

        _fx._authorizationServiceMock.Setup(a => a.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), It.IsAny<string>()))
            .ReturnsAsync(AuthorizationResult.Success());

        var service = new CollectService(
            _fx._collectRepoMock.Object,
            _fx._supplierRepoMock.Object,
            _fx._productRepoMock.Object,
            _fx._filialRepoMock.Object,
            _fx._userManagerMock.Object,
            _fx._currentUserServiceMock.Object,
            _fx._authorizationServiceMock.Object,
            _fx._loggerMock.Object);

        var result = await service.SetPagedResultCollectListViewModel(filters, 1, 10);

        var expected = new PagedResultViewModel<CollectListViewModel, CollectFilterViewModel>
        {
            Items = [collectListViewModel],
            TotalPages = 1,
            PageNum = 1,
            Filters = filters
        };

        result.Should().BeEquivalentTo(expected);

        _fx._collectRepoMock.Verify(c => c.ToCollectListAsync(It.IsAny<CollectFilterViewModel>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        _fx._authorizationServiceMock.Verify(a => a.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), It.IsAny<string>()), Times.Exactly(2));
    }
}
