using System.Threading.Tasks;
using CollectApp.Data;
using CollectApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CollectApp.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly CollectAppContext _context;

        public SupplierService(CollectAppContext context)
        {
            _context = context;
        }

        public async Task<List<Supplier>> GetAllSuppliersListAsycn()
        {
            return await _context.Suppliers.ToListAsync();
        }

        public async Task<bool> SupplierExist(Supplier supplier)
        {
            return await _context.Suppliers.AnyAsync(s => s.CNPJ == supplier.CNPJ);
        }

        public async Task<OperationResult> AddSupplier(Supplier supplier)
        {
            bool supplierExist = await SupplierExist(supplier);

            if (supplierExist)
            {
                return OperationResult.Fail(0,"Já existe um fornecedor cadastrado com o CNPJ fornecido");
            }

            _context.Suppliers.Add(supplier);
            return OperationResult.Ok();
        }

        public async Task<OperationResult> EditSupplier(Supplier supplier)
        {
            bool supplierExist = await SupplierExist(supplier);

            if (supplierExist)
            {
                return OperationResult.Fail(0,"Já existe um fornecedor cadastrado com o CNPJ fornecido");
            }

            return OperationResult.Ok();
        }

        public async Task<int> SaveChangesSuppliersAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<Supplier?> FindSupplierAsync(int? id)
        {
            return await _context.Suppliers.FindAsync(id);
        }

        public void DeleteSupplier(Supplier supplier)
        {
            _context.Suppliers.Remove(supplier);
        }
    }
}