using CollectApp.Models;
using CollectApp.Repositories;
using CollectApp.Services;
using CollectApp.ViewModels;
using CollectAppTests.Builders;
using FluentAssertions;
using Moq;

namespace CollectAppTests.Services;

public class FilialServiceTests
{
    private readonly Mock<IFilialRepository> _filialRepoMock;
    private readonly Mock<ICollectRepository> _collectRepoMock;

    public FilialServiceTests()
    {
        _filialRepoMock = new Mock<IFilialRepository>();
        _collectRepoMock = new Mock<ICollectRepository>();
    }

    [Fact]
    public async Task CreateFilial_WhenFilialExistIsFalse_ShouldCreateFilial()
    {
        Filial? filialSent = null;

        var createFilialViewModel = new CreateFilialViewModelBuilder().Build();

        _filialRepoMock
            .Setup(f => f.AnyFilialAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(false);

        _filialRepoMock
            .Setup(f => f.AddFilial(It.IsAny<Filial>()))
            .Callback<Filial>(f => filialSent = f);

        var service = new FilialService(
            _filialRepoMock.Object,
            _collectRepoMock.Object
        );

        var result = await service.CreateFilial(createFilialViewModel);

        var expected = 
            new OperationResultBuilder()
            .WithSuccess(true)
            .Build();

        result.Should().BeEquivalentTo(expected);
        filialSent.Should().NotBeNull();
        filialSent.Name.Should().Be(createFilialViewModel.Name);

        _filialRepoMock.Verify(f => f.AnyFilialAsync(It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        _filialRepoMock.Verify(f => f.AddFilial(It.IsAny<Filial>()), Times.Once);
        _filialRepoMock.Verify(f => f.SaveChangesFilialAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateFilial_WhenFilialExistIsTrue_ShouldNotCreateFilial()
    {
        var createFilialViewModel = new CreateFilialViewModelBuilder().Build();

        _filialRepoMock
            .Setup(f => f.AnyFilialAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(true);

        var service = new FilialService(
            _filialRepoMock.Object,
            _collectRepoMock.Object
        );

        var result = await service.CreateFilial(createFilialViewModel);

        var expected = 
            new OperationResultBuilder()
            .WithSuccess(false)
            .WithMessage("Já existe uma filial cadastrada com o nome fornecido")
            .Build();

        result.Should().BeEquivalentTo(expected);

        _filialRepoMock.Verify(f => f.AnyFilialAsync(It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        _filialRepoMock.Verify(f => f.AddFilial(It.IsAny<Filial>()), Times.Never);
        _filialRepoMock.Verify(f => f.SaveChangesFilialAsync(), Times.Never);
    }

    [Fact]
    public async Task EditFilial_WhenFilialIsNotNullAndFilialExistIsFalse_ShouldEditFilial()
    {
        var filial = new FilialBuilder().Build();
        var editFilialViewModel = new EditFilialViewModelBuilder().Build();

        _filialRepoMock
            .Setup(f => f.GetFilialByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(filial);

        _filialRepoMock
            .Setup(f => f.AnyFilialAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(false);

        var service = new FilialService(
            _filialRepoMock.Object,
            _collectRepoMock.Object
        );

        var result = await service.EditFilial(editFilialViewModel);

        var expected = 
            new OperationResultBuilder()
            .WithSuccess(true)
            .Build();

        result.Should().BeEquivalentTo(expected);
        filial.Name.Should().Be(editFilialViewModel.Name);

        _filialRepoMock.Verify(f => f.GetFilialByIdAsync(It.IsAny<int>()), Times.Once);
        _filialRepoMock.Verify(f => f.AnyFilialAsync(It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        _filialRepoMock.Verify(f => f.SaveChangesFilialAsync(), Times.Once);
    }

    [Fact]
    public async Task EditFilial_WhenFilialIsNull_ShouldNotEditFilial()
    {
        var editFilialViewModel = new EditFilialViewModelBuilder().Build();

        _filialRepoMock
            .Setup(f => f.GetFilialByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Filial?)null);

        _filialRepoMock
            .Setup(f => f.AnyFilialAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(false);

        var service = new FilialService(
            _filialRepoMock.Object,
            _collectRepoMock.Object
        );

        var result = await service.EditFilial(editFilialViewModel);

        result.Should().BeNull();

        _filialRepoMock.Verify(f => f.GetFilialByIdAsync(It.IsAny<int>()), Times.Once);
        _filialRepoMock.Verify(f => f.AnyFilialAsync(It.IsAny<string>(), It.IsAny<int>()), Times.Never);
        _filialRepoMock.Verify(f => f.SaveChangesFilialAsync(), Times.Never);
    }

    [Fact]
    public async Task EditFilial_WhenFilialExistIsTrue_ShouldNotEditFilial()
    {
        var filial = new FilialBuilder().Build();
        var editFilialViewModel = new EditFilialViewModelBuilder().Build();

        _filialRepoMock
            .Setup(f => f.GetFilialByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(filial);

        _filialRepoMock
            .Setup(f => f.AnyFilialAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(true);

        var service = new FilialService(
            _filialRepoMock.Object,
            _collectRepoMock.Object
        );

        var result = await service.EditFilial(editFilialViewModel);

        var expected = 
            new OperationResultBuilder()
            .WithSuccess(false)
            .WithMessage("Já existe uma filial cadastrada com o nome fornecido")
            .Build();

        result.Should().BeEquivalentTo(expected);
        filial.Name.Should().NotBe(editFilialViewModel.Name);

        _filialRepoMock.Verify(f => f.GetFilialByIdAsync(It.IsAny<int>()), Times.Once);
        _filialRepoMock.Verify(f => f.AnyFilialAsync(It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        _filialRepoMock.Verify(f => f.SaveChangesFilialAsync(), Times.Never);
    }

    [Fact]
    public async Task SetPagedResultFilialListViewModel_WhenHasFilters_ShouldReturnFilteredList()
    {
        var filial = new FilialBuilder().Build();

        var filters = 
            new FilialFilterViewModelBuilder()
            .WithId(1)
            .WithName("Filial SP Centro")
            .Build();

        var filialListViewModel = 
            new FilialListViewModelBuilder()
            .FromFilial(filial)
            .Build();

        _filialRepoMock
            .Setup(f => f.ToFilialListAsync(It.IsAny<FilialFilterViewModel>(), 1, 10, ""))
            .ReturnsAsync(([filial], 1));

        var service = new FilialService(
            _filialRepoMock.Object,
            _collectRepoMock.Object
        );

        var result = await service.SetPagedResultFilialListViewModel(filters, 1, 10, "");

        var expected = new PagedResultViewModel<FilialListViewModel, FilialFilterViewModel>
        {
            Items = [filialListViewModel],
            TotalPages = 1,
            PageNum = 1,
            Filters = filters,
        };

        result.Should().BeEquivalentTo(expected);
        result.Items.Should().HaveCount(1);

        _filialRepoMock.Verify(f => f.ToFilialListAsync(filters, 1, 10, ""), Times.Once);
    }

    [Fact]
    public async Task SetPagedResultFilialListViewModel_WhenHasNoFilters_ShouldReturnNotFilteredList()
    {
        var filialList = new List<Filial>
        {
            new FilialBuilder().Build(),
            new FilialBuilder()
                .WithId(2)
                .WithName("Mega Bom")
                .Build(),
            new FilialBuilder()
                .WithId(3)
                .WithName("ACB")
                .Build(),
        };

        var filters = new FilialFilterViewModel();

        var filialListViewModel = filialList
            .Select(f => new FilialListViewModelBuilder().FromFilial(f).Build())
            .ToList();

        _filialRepoMock
            .Setup(f => f.ToFilialListAsync(It.IsAny<FilialFilterViewModel>(), 1, 10, ""))
            .ReturnsAsync((filialList, 3));

        var service = new FilialService(
            _filialRepoMock.Object,
            _collectRepoMock.Object
        );

        var result = await service.SetPagedResultFilialListViewModel(filters, 1, 10, "");

        var expected = new PagedResultViewModel<FilialListViewModel, FilialFilterViewModel>
        {
            Items = filialListViewModel,
            TotalPages = 1,
            PageNum = 1,
            Filters = filters,
        };

        result.Should().BeEquivalentTo(expected);
        result.Items.Should().HaveCount(3);

        _filialRepoMock.Verify(f => f.ToFilialListAsync(filters, 1, 10, ""), Times.Once);
    }

    [Fact]
    public async Task DeleteFilial_WhenFilialIsNull_ShouldNotDeleteFilial()
    {
        _filialRepoMock
            .Setup(f => f.GetFilialByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Filial?)null);

        var service = new FilialService(
            _filialRepoMock.Object,
            _collectRepoMock.Object
        );

        var result = await service.DeleteFilial(1);

        result.Should().BeNull();

        _filialRepoMock.Verify(f => f.GetFilialByIdAsync(It.IsAny<int>()), Times.Once);
        _collectRepoMock.Verify(c => c.AnyCollectAsync(It.IsAny<string>(), It.IsAny<int>()), Times.Never);
        _filialRepoMock.Verify(f => f.RemoveFilial(It.IsAny<Filial>()), Times.Never);
        _filialRepoMock.Verify(f => f.SaveChangesFilialAsync(), Times.Never);
    }

    [Fact]
    public async Task DeleteFilial_WhenExistFilialWithCollectIsTrue_ShouldNotDeleteFilial()
    {
        var filial = new FilialBuilder().Build();

        _filialRepoMock
            .Setup(f => f.GetFilialByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(filial);

        _collectRepoMock
            .Setup(c => c.AnyCollectAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(true);

        var service = new FilialService(
            _filialRepoMock.Object,
            _collectRepoMock.Object
        );

        var result = await service.DeleteFilial(1);

        var expected = 
            new OperationResultBuilder()
            .WithSuccess(false)
            .WithMessage("Não foi possível deletar, existe uma coleta vinculada a esta loja")
            .Build();

        result.Should().BeEquivalentTo(expected);

        _filialRepoMock.Verify(f => f.GetFilialByIdAsync(It.IsAny<int>()), Times.Once);
        _collectRepoMock.Verify(c => c.AnyCollectAsync(It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        _filialRepoMock.Verify(f => f.RemoveFilial(It.IsAny<Filial>()), Times.Never);
        _filialRepoMock.Verify(f => f.SaveChangesFilialAsync(), Times.Never);
    }

    [Fact]
    public async Task DeleteFilial_WhenFilialIsNotNullAndExistFilialWithCollectIsFalse_ShouldDeleteFilial()
    {
        var filial = new FilialBuilder().Build();

        _filialRepoMock
            .Setup(f => f.GetFilialByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(filial);

        _collectRepoMock
            .Setup(c => c.AnyCollectAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(false);

        var service = new FilialService(
            _filialRepoMock.Object,
            _collectRepoMock.Object
        );

        var result = await service.DeleteFilial(1);

        var expected = 
            new OperationResultBuilder()
            .WithSuccess(true)
            .Build();

        result.Should().BeEquivalentTo(expected);

        _filialRepoMock.Verify(f => f.GetFilialByIdAsync(It.IsAny<int>()), Times.Once);
        _collectRepoMock.Verify(c => c.AnyCollectAsync(It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        _filialRepoMock.Verify(f => f.RemoveFilial(It.IsAny<Filial>()), Times.Once);
        _filialRepoMock.Verify(f => f.SaveChangesFilialAsync(), Times.Once);
    }
}