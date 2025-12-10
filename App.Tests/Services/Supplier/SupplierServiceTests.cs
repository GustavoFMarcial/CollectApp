using CollectApp.Repositories;
using Moq;

namespace CollectAppTests.Builders;

public class SupplierServiceTests
{
    private readonly Mock<ISupplierRepository> _supplierRepoMock;
    private readonly Mock<ICollectRepository> _collectRepoMock;

    public SupplierServiceTests()
    {
        _supplierRepoMock = new Mock<ISupplierRepository>();
        _collectRepoMock = new Mock<ICollectRepository>();
    }
}