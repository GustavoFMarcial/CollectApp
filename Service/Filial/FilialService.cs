using CollectApp.Data;
using CollectApp.Models;
using CollectApp.ViewModels;
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

        public async Task<OperationResult> AddFilial(Filial filial)
        {
            bool productExist = await _context.Filials.AnyAsync(f => f.Name == filial.Name && f.Id != filial.Id);

            if (productExist)
            {
                return OperationResult.Fail($"Já existe uma filial cadastrada com o nome fornecido");
            }

            _context.Filials.Add(filial);

            return OperationResult.Ok();
        }

        public async Task<OperationResult> EditFilial(EditFilialViewModel filialEdit)
        {
            bool productExist = await _context.Filials.AnyAsync(f => f.Name == filialEdit.Name && f.Id != filialEdit.Id);

            if (productExist)   
            {
                return OperationResult.Fail($"Já existe uma filial cadastrada com o nome fornecido");
            }

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

        public async Task<int> SaveChangesFilialsAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void DeleteFilial(Filial filial)
        {
            _context.Filials.Remove(filial);
        }
    }
}