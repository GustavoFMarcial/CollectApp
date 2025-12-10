using CollectApp.Models;
using CollectApp.ViewModels;

namespace CollectAppTests.Builders;

public class SupplierListViewModelBuilder
{
    private int _id;
    private string _name = string.Empty;
    private string _cNPJ = string.Empty;
    private string _address = string.Empty;

    public SupplierListViewModelBuilder FromSupplier(Supplier s)
    {
        _id = s.Id;
        _name = s.Name;
        _cNPJ = s.CNPJ;
        _address = $"Rua {s.Street}, Bairro {s.Neighborhood}, nº {s.Number}, {s.City}/{s.State} - CEP {s.ZipCode}";
        return this;
    }

    public SupplierListViewModel Build()
    {
        return new SupplierListViewModel
        {
            Id = _id,
            Name = _name,
            CNPJ = _cNPJ,
            Address = _address,
        };
    }
}