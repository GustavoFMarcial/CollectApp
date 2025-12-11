using CollectApp.Repositories;
using Moq;

namespace CollectAppTests.Services;

public class AuditLogServiceTests
{
    private readonly Mock<IAuditLogRepository> _auditLogRepoMock;

    public AuditLogServiceTests()
    {
        _auditLogRepoMock = new Mock<IAuditLogRepository>();
    }
}