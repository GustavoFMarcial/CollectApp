using CollectApp.Models;
using CollectApp.Repositories;
using CollectApp.Services;
using CollectApp.Tests.Builders;
using CollectApp.ViewModels;
using CollectAppTests.Builders;
using FluentAssertions;
using Moq;

namespace CollectAppTests.Services;

public class AuditLogServiceTests
{
    private readonly Mock<IAuditLogRepository> _auditLogRepoMock;

    public AuditLogServiceTests()
    {
        _auditLogRepoMock = new Mock<IAuditLogRepository>();
    }

    [Fact]
    public async Task SetPagedResultAuditLogViewModel_WhenHasItems_ShouldReturnListWithItems()
    {
        var auditLogList = new List<AuditLog>
        {
            new AuditLogBuilder().Build(),
            new AuditLogBuilder()
                .WithField("Volume")
                .WithOldValue("50")
                .WithNewValue("100")
                .Build(),
            new AuditLogBuilder()
                .WithField("Weight")
                .WithOldValue("100")
                .WithNewValue("50")
                .Build(),
        };

        var auditLogViewModel = auditLogList.Select(a =>
            new AuditLogViewModelBuilder()
            .FromAuditLog(a)
            .Build())
            .ToList();

        _auditLogRepoMock
            .Setup(a => a.ToLogListAsync(It.IsAny<string>(), It.IsAny<string>(), 1, 10))
            .ReturnsAsync((auditLogList, 3));

        var service = new AuditLogService(
            _auditLogRepoMock.Object
        );

        var result = await service.SetPagedResultAuditLogViewModel("Supplier", "1", 1, 10);

        var expected = new PagedResultViewModel<AuditLogViewModel, object>
        {
            Items = auditLogViewModel,
            TotalPages = 1,
            PageNum = 1,
        };

        result.Should().BeEquivalentTo(expected);

        _auditLogRepoMock.Verify(a => a.ToLogListAsync(It.IsAny<string>(), It.IsAny<string>(), 1, 10), Times.Once);
    }

    [Fact]
    public async Task SetPagedResultAuditLogViewModel_WhenHasNoItems_ShouldReturnEmptyList()
    {
        _auditLogRepoMock
            .Setup(a => a.ToLogListAsync(It.IsAny<string>(), It.IsAny<string>(), 1, 10))
            .ReturnsAsync(([], 0));

        var service = new AuditLogService(
            _auditLogRepoMock.Object
        );

        var result = await service.SetPagedResultAuditLogViewModel("Supplier", "1", 1, 10);

        var expected = new PagedResultViewModel<AuditLogViewModel, object>
        {
            Items = [],
            TotalPages = 0,
            PageNum = 1,
        };

        result.Should().BeEquivalentTo(expected);

        _auditLogRepoMock.Verify(a => a.ToLogListAsync(It.IsAny<string>(), It.IsAny<string>(), 1, 10), Times.Once);
    }
}