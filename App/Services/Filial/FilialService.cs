using CollectApp.Models;
using CollectApp.Repositories;
using CollectApp.ViewModels;

namespace CollectApp.Services;

public class FilialService : IFilialService
{
    private readonly IFilialRepository _filialRepository;
    private readonly ICollectRepository _collectRepository;

    public FilialService(IFilialRepository filialRepository, ICollectRepository collectRepository)
    {
        _filialRepository = filialRepository;
        _collectRepository = collectRepository;
    }

    public async Task<OperationResult> CreateFilial(CreateFilialViewModel filialCreate)
    {
        Filial filial = new Filial
        {
            Name = filialCreate.Name,
        };

        bool filialExist = await _filialRepository.AnyFilialAsync(filialCreate.Name, filial.Id);

        if (filialExist)
        {
            return OperationResult.Fail($"Já existe uma filial cadastrada com o nome fornecido");
        }

        _filialRepository.AddFilial(filial);
        await _filialRepository.SaveChangesFilialAsync();

        return OperationResult.Ok();
    }

    public async Task<EditFilialViewModel?> SetEditFilialViewModel(int id)
    {
        Filial? filial = await _filialRepository.GetFilialByIdAsync(id);

        if (filial == null)
        {
            return null;
        }

        EditFilialViewModel epvm = new EditFilialViewModel
        {
            Id = filial.Id,
            Name = filial.Name
        };

        return epvm;
    }

    public async Task<OperationResult?> EditFilial(EditFilialViewModel filialEdit)
    {
        Filial? filial = await _filialRepository.GetFilialByIdAsync(filialEdit.Id);

        if (filial == null)
        {
            return null;
        }

        bool filialExist = await _filialRepository.AnyFilialAsync(filialEdit.Name, filialEdit.Id);

        if (filialExist)
        {
            return OperationResult.Fail($"Já existe uma filial cadastrada com o nome fornecido");
        }

        filial.Name = filialEdit.Name;
        await _filialRepository.SaveChangesFilialAsync();

        return OperationResult.Ok();
    }

    public async Task<PagedResultViewModel<FilialListViewModel, FilialFilterViewModel>> SetPagedResultFilialListViewModel(FilialFilterViewModel filters, int pageNum = 1, int pageSize = 10, string? input = null)
    {
        (List<Filial> Items, int TotalCount) filials = await _filialRepository.ToFilialListAsync(filters, pageNum, pageSize, input);

        List<FilialListViewModel> filialListViewModel = filials.Items.Select(f => new FilialListViewModel
        {
            Id = f.Id,
            Name = f.Name,
        }).ToList();

        PagedResultViewModel<FilialListViewModel, FilialFilterViewModel> pagedResultFilialListViewModel = new PagedResultViewModel<FilialListViewModel, FilialFilterViewModel>
        {
            Items = filialListViewModel,
            TotalPages = (int)Math.Ceiling(filials.TotalCount / (double)pageSize),
            PageNum = pageNum,
            Filters = filters,
        };

        return pagedResultFilialListViewModel;
    }

    public async Task<OperationResult?> DeleteFilial(int id)
    {
        Filial? filial = await _filialRepository.GetFilialByIdAsync(id);

        if (filial == null)
        {
            return null;
        }

        bool existFilialWithCollect = await _collectRepository.AnyCollectAsync("filial", filial.Id);

        if (existFilialWithCollect)
        {
            return OperationResult.Fail("Não foi possível deletar, existe uma coleta vinculada a esta loja");
        }

        _filialRepository.RemoveFilial(filial);
        await _filialRepository.SaveChangesFilialAsync();

        return OperationResult.Ok();
    }
}