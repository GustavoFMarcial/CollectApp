using CollectApp.Data;
using Microsoft.EntityFrameworkCore;
using CollectApp.Models;
using CollectApp.ViewModels;
using CollectApp.Controllers;

namespace CollectApp.Services
{
    public class CollectService : ICollectService
    {
        private readonly CollectAppContext _context;

        public CollectService(CollectAppContext context)
        {
            _context = context;
        }

        public async Task<List<Collect>> GetAllCollectsListAsycn()
        {
            return await _context.Collects.ToListAsync();
        }

        public void CreateCollect(CreateCollectViewModel collect)
        {
            Collect c = new Collect
            {
                Company = collect.Company,
                CollectAt = collect.CollectAt,
            };

            _context.Collects.Add(c);
        }

        public async Task<int> SaveChangesCollectsAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<Collect?> FindCollectAsync(int? id)
        {
            return await _context.Collects.FindAsync(id);
        }

        public void UpdateCollectStatus(Collect collect, string status)
        {
            if (status == "collected")
            {
                status = "Coletado";
            }
            else
            {
                status = "Deletado";
            }

            collect.Status = status;
            _context.Attach(collect);
            _context.Entry(collect).Property(c => c.Status).IsModified = true;
        }

        public void UpdateCollectFields(Collect collect, EditCollectViewModel collectEdit)
        {
            collect.CollectAt = collectEdit.CollectAt;
            collect.Company = collectEdit.Company;
            collect.Volume = collectEdit.Volume;
            collect.Weigth = collectEdit.Weight;
            collect.Filial = collectEdit.Filial;
        }
    }
}