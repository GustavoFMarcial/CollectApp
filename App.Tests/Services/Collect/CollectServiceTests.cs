using Moq;
using CollectApp.ViewModels;
namespace CollectAppTests.Services;

public class CollectServiceTests
{
    [Fact]
    public void SetPagedResultCollectListViewModel_WhenHasItems_ReturnsCorrectValue()
    {
        var mockService = new Mock<CollectFilterViewModel>();

        // Mock.Setup(s => s.SetPagedResultCollectListViewModel)
    }
}
