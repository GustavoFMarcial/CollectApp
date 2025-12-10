using CollectApp.Models;
using CollectApp.Repositories;
using CollectApp.ViewModels;

namespace CollectApp.Services;

public class SupplierService : ISupplierService
{
    private readonly ISupplierRepository _supplierRepository;
    private readonly ICollectRepository _collectRepository;

    public SupplierService(ISupplierRepository supplierRepository, ICollectRepository collectRepository)
    {
        _supplierRepository = supplierRepository;
        _collectRepository = collectRepository;
    }

    public async Task<PagedResultViewModel<SupplierListViewModel, SupplierFilterViewModel>> SetPagedResultSupplierListViewModel(SupplierFilterViewModel filters, int pageNum = 1, int pageSize = 10, string? input = null)
    {
        (List<Supplier> items, int totalCount) suppliers = await _supplierRepository.ToSupplierListAsync(filters, pageNum, pageSize, input);

        List<SupplierListViewModel> supplierListViewModel = suppliers.items.Select(s => new SupplierListViewModel
        {
            Id = s.Id,
            Name = s.Name,
            CNPJ = s.CNPJ,
            Address = $"Rua {s.Street}, Bairro {s.Neighborhood}, nº {s.Number}, {s.City}/{s.State} - CEP {s.ZipCode}",
        }).ToList();

        PagedResultViewModel<SupplierListViewModel, SupplierFilterViewModel> pagedResultSupplierListViewModel = new PagedResultViewModel<SupplierListViewModel, SupplierFilterViewModel>
        {
            Items = supplierListViewModel,
            TotalPages = (int)Math.Ceiling(suppliers.totalCount / (double)pageSize),
            PageNum = pageNum,
            Filters = filters,
        };

        return pagedResultSupplierListViewModel;
    }

    public async Task<OperationResult?> CreateSupplier(CreateSupplierViewModel supplierCreate)
    {
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

        bool supplierExist = await _supplierRepository.AnySupplierAsync(supplierCreate.CNPJ, supplier.Id);

        if (supplierExist)
        {
            return OperationResult.Fail("Já existe um fornecedor cadastrado com o CNPJ fornecido");
        }

        _supplierRepository.AddSupplier(supplier);
        await _supplierRepository.SaveChangesSupplierAsync();

        return OperationResult.Ok();
    }

    public async Task<EditSupplierViewModel?> SetEditSupplierViewModel(int id)
    {
        Supplier? supplier = await _supplierRepository.GetSupplierByIdAsync(id);

        if (supplier == null)
        {
            return null;
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

    public async Task<OperationResult?> EditSupplier(EditSupplierViewModel supplierEdit)
    {
        Supplier? supplier = await _supplierRepository.GetSupplierByIdAsync(supplierEdit.Id);

        if (supplier == null)
        {
            return null;
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

    public async Task<OperationResult?> DeleteSupplier(int id)
    {
        Supplier? supplier = await _supplierRepository.GetSupplierByIdAsync(id);

        if (supplier == null)
        {
            return null;
        }

        bool existSupplierWithCollect = await _collectRepository.AnyCollectAsync("supplier", supplier.Id);

        if (existSupplierWithCollect)
        {
            return OperationResult.Fail("Não foi possível deletar, existe uma coleta vinculada a este fornecedor");
        }

        _supplierRepository.RemoveSupplier(supplier);
        await _supplierRepository.SaveChangesSupplierAsync();

        return OperationResult.Ok();
    }
}