using System.Threading.Tasks;
using CollectApp.Data;
using CollectApp.Models;
using CollectApp.ViewModels;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;

namespace CollectApp.Services
{
    public class FilialService : IFilialService
    {
        private readonly CollectAppContext _context;

        public FilialService(CollectAppContext context)
        {
            _context = context;
        }

        public async Task<OperationResult> CreateFilial(CreateFilialViewModel filialCreate)
        {
            Filial filial = new Filial
            {
                Name = filialCreate.Name,
            };

            bool productExist = await _context.Filials.AnyAsync(f => f.Name == filial.Name && f.Id != filial.Id);

            if (productExist)
            {
                return OperationResult.Fail($"Já existe uma filial cadastrada com o nome fornecido");
            }

            _context.Filials.Add(filial);
            await SaveChangesFilialsAsync();

            return OperationResult.Ok();
        }

        public async Task<EditFilialViewModel> SetEditFilialViewModel(int? id)
        {
            Filial? filial = await FindFilialAsync(id);

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
            Filial? filial = await FindFilialAsync(filialEdit.Id);

            if (filial == null)
            {
                OperationResult NotFound = new OperationResult();
                return NotFound;
            }

            bool filialExist = await _context.Filials.AnyAsync(f => f.Name == filialEdit.Name && f.Id != filialEdit.Id);

            if (filialExist)
            {
                return OperationResult.Fail($"Já existe uma filial cadastrada com o nome fornecido");
            }

            filial.Name = filialEdit.Name;
            await SaveChangesFilialsAsync();

            return OperationResult.Ok();
        }

        public async Task<Filial?> FindFilialAsync(int? id)
        {
            return await _context.Filials.FindAsync(id);
        }

        public async Task<List<Filial>> GetAllFilialsListAsycn()
        {
            return await _context.Filials.ToListAsync();
        }

        public async Task<List<Filial>> GetFilteredFilialsAsync(string input)
        {
            return await _context.Filials.Where(f => f.Name.Contains(input)).ToListAsync();
        }

        public async Task<List<FilialListViewModel>> SetFilialListViewModel()
        {
            List<Filial> filials = await GetAllFilialsListAsycn();

            List<FilialListViewModel> flvm = filials.Select(f => new FilialListViewModel
            {
                Id = f.Id,
                Name = f.Name
            }).ToList();

            return flvm;
        }

        public async Task<int> SaveChangesFilialsAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task DeleteFilial(int? id)
        {
            if (id == null)
            {
                return;
            }

            Filial? filial = await FindFilialAsync(id);

            if (filial == null)
            {
                return;
            }

            _context.Filials.Remove(filial);
            await SaveChangesFilialsAsync();
        }
    }
}