using CollectApp.Models;
using CollectApp.Repositories;
using CollectApp.ViewModels;

namespace CollectApp.Services
{
    public class FilialService : IFilialService
    {
        private readonly IFilialRepository _filialRepository;

        public FilialService(IFilialRepository filialRepository)
        {
            _filialRepository = filialRepository;
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
                Name = filial.Name,
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

        public async Task<List<Filial>> GetAllFilialsListAsycn()
        {
            return await _filialRepository.ToFilialListAsync();
        }

        public async Task<List<Filial>> GetFilteredFilialsAsync(string input)
        {
            return await _filialRepository.WhereFilialAsync(input);
        }

        public async Task<List<FilialListViewModel>> SetFilialListViewModel()
        {
            List<Filial> filials = await _filialRepository.ToFilialListAsync();

            List<FilialListViewModel> flvm = filials.Select(f => new FilialListViewModel
            {
                Id = f.Id,
                Name = f.Name
            }).ToList();

            return flvm;
        }

        public async Task DeleteFilial(int? id)
        {
            if (id == null)
            {
                return;
            }

            Filial? filial = await _filialRepository.GetFilialByIdAsync(id);

            if (filial == null)
            {
                return;
            }

            _filialRepository.RemoveFilial(filial);
            await _filialRepository.SaveChangesFilialAsync();
        }
    }
}