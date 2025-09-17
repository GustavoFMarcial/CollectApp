using CollectApp.Models;
using CollectApp.Repositories;
using CollectApp.ViewModels;

namespace CollectApp.Services
{
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

            bool productExist = await _filialRepository.AnyFilialAsync(filialCreate.Name, filial.Id);

            if (productExist)
            {
                return OperationResult.Fail($"Já existe uma filial cadastrada com o nome fornecido");
            }

            _filialRepository.AddFilial(filial);
            await _filialRepository.SaveChangesFilialAsync();

            return OperationResult.Ok();
        }

        public async Task<EditFilialViewModel> SetEditFilialViewModel(int? id)
        {
            Filial? filial = await _filialRepository.GetFilialByIdAsync(id);

            if (filial == null)
            {
                EditFilialViewModel NotFound = new EditFilialViewModel();
                return NotFound;
            }

            EditFilialViewModel epvm = new EditFilialViewModel
            {
                Id = filial.Id,
                Name = filial.Name
            };

            return epvm;
        }

        public async Task<OperationResult> EditFilial(EditFilialViewModel filialEdit)
        {
            Filial? filial = await _filialRepository.GetFilialByIdAsync(filialEdit.Id);

            if (filial == null)
            {
                OperationResult NotFound = new OperationResult();
                return NotFound;
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

        public async Task<(List<Filial>, int)> GetAllFilialsListAsycn()
        {
            return await _filialRepository.ToFilialListAsync();
        }

        public async Task<List<Filial>> GetFilteredFilialsAsync(string input)
        {
            return await _filialRepository.WhereFilialAsync(input);
        }

        public async Task<PagedResultViewModel<FilialListViewModel>> SetPagedResultFilialListViewModel(int pageNum = 1, int pageSize = 10)
        {
            (List<Filial> Items, int TotalCount) filials = await _filialRepository.ToFilialListAsync(pageNum, pageSize);

            List<FilialListViewModel> filialListViewModel = filials.Items.Select(f => new FilialListViewModel
            {
                Id = f.Id,
                Name = f.Name,
            }).ToList();

            PagedResultViewModel<FilialListViewModel> pagedResultFilialListViewModel = new PagedResultViewModel<FilialListViewModel>
            {
                Items = filialListViewModel,
                TotalPages = (int)Math.Ceiling(filials.TotalCount / (double)pageSize),
                PageNum = pageNum,
            };

            return pagedResultFilialListViewModel;
        }

        public async Task<OperationResult> DeleteFilial(int? id)
        {
            if (id == null)
            {
                OperationResult NotFound = new OperationResult();
                return NotFound;
            }

            Filial? filial = await _filialRepository.GetFilialByIdAsync(id);

            if (filial == null)
            {
                OperationResult NotFound = new OperationResult();
                return NotFound;
            }

            bool existFilialWithCollect = await _collectRepository.AnyCollectAsync("filial", filial.Id);

            if (existFilialWithCollect)
            {
                return OperationResult.Fail("Não é possível deletar, existe uma coleta vinculada a esta loja");
            }

            _filialRepository.RemoveFilial(filial);
            await _filialRepository.SaveChangesFilialAsync();

            return OperationResult.Ok();
        }
    }
}