using CollectApp.Models;
using CollectApp.Repositories;
using CollectApp.ViewModels;

namespace CollectApp.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;

        public SupplierService(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public async Task<List<Supplier>> GetAllSuppliersListAsycn()
        {
            return await _supplierRepository.ToSupplierListAsync();
        }

        public async Task<List<Supplier>> GetFilteredSuppliersAsync(string input)
        {
            return await _supplierRepository.WhereSupplierAsync(input);
        }

        public async Task<List<SupplierListViewModel>> SetSupplierListViewModel()
        {
            List<Supplier> suppliers = await GetAllSuppliersListAsycn();

            List<SupplierListViewModel> slvm = suppliers.Select(s => new SupplierListViewModel
            {
                Id = s.Id,
                Name = s.Name,
                CNPJ = s.CNPJ,
                Address = $"Rua {s.Street}, Bairro {s.Neighborhood}, nº {s.Number}, {s.City}/{s.State} - CEP {s.ZipCode}",
            }).ToList();

            return slvm;
        }

        public async Task<OperationResult> CreateSupplier(CreateSupplierViewModel supplierCreate)
        {
            if (supplierCreate == null)
            {
                OperationResult NotFound = new OperationResult();
                return NotFound;
            }

            Supplier supplier = new Supplier
            {
                Name = supplierCreate.Name,
                CNPJ = supplierCreate.CNPJ,
                Street = supplierCreate.Street,
                Neighborhood = supplierCreate.Neighborhood,
                Number = supplierCreate.Number,
                City = supplierCreate.City,
                State = supplierCreate.State,
                ZipCode = supplierCreate.ZipCode,
            };

            bool supplierExist = await _supplierRepository.AnySupplierAsync(supplierCreate.CNPJ, null);

            if (supplierExist)
            {
                return OperationResult.Fail("Já existe um fornecedor cadastrado com o CNPJ fornecido");
            }

            await _supplierRepository.AddSupplier(supplier);

            return OperationResult.Ok();
        }

        public async Task<EditSupplierViewModel> SetEditSupplierViewModel(int? id)
        {
            if (id == null)
            {
                EditSupplierViewModel NotFound = new EditSupplierViewModel();
                return NotFound;
            }

            Supplier? supplier = await _supplierRepository.GetSupplierByIdAsync(id);

            if (supplier == null)
            {
                EditSupplierViewModel NotFound = new EditSupplierViewModel();
                return NotFound;
            }

            EditSupplierViewModel esvm = new EditSupplierViewModel
            {
                Id = supplier.Id,
                Name = supplier.Name,
                CNPJ = supplier.CNPJ,
                Street = supplier.Street,
                Neighborhood = supplier.Neighborhood,
                Number = supplier.Number,
                City = supplier.City,
                State = supplier.State,
                ZipCode = supplier.ZipCode,
            };

            return esvm;
        }

        public async Task<OperationResult> EditSupplier(EditSupplierViewModel supplierEdit)
        {
            Supplier? supplier = await _supplierRepository.GetSupplierByIdAsync(supplierEdit.Id);

            if (supplier == null)
            {
                OperationResult NotFound = new OperationResult();
                return NotFound;
            }

            bool supplierExist = await _supplierRepository.AnySupplierAsync(supplierEdit.CNPJ, supplier.Id);

            if (supplierExist)
            {
                return OperationResult.Fail("Já existe um fornecedor cadastrado com o CNPJ fornecido");
            }

            supplier.Name = supplierEdit.Name;
            supplier.CNPJ = supplierEdit.CNPJ;
            supplier.Street = supplierEdit.Street;
            supplier.Neighborhood = supplierEdit.Neighborhood;
            supplier.Number = supplierEdit.Number;
            supplier.City = supplierEdit.City;
            supplier.State = supplierEdit.State;
            supplier.ZipCode = supplierEdit.ZipCode;

            await _supplierRepository.SaveChangesSupplierAsync();

            return OperationResult.Ok();
        }

        public async Task DeleteSupplier(int? id)
        {
            if (id == null)
            {
                return;
            }

            Supplier? supplier = await _supplierRepository.GetSupplierByIdAsync(id);

            if (supplier == null)
            {
                return;
            }

            await _supplierRepository.RemoveSupplier(supplier);
        }
    }
}