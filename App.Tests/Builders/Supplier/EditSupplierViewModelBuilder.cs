using CollectApp.ViewModels;

namespace CollectApp.Tests.Builders;

public class EditSupplierViewModelBuilder
{
    private int _id = 1;
    private string _name = "Fornecedor Padrão";
    private string _cnpj = "12.345.678/0001-90";
    private string _street = "Rua das Flores";
    private string _neighborhood = "Centro";
    private string _number = "123";
    private string _city = "São Paulo";
    private string _state = "SP";
    private string _zipCode = "01234-567";

    public EditSupplierViewModelBuilder WithId(int id)
    {
        _id = id;
        return this;
    }

    public EditSupplierViewModelBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public EditSupplierViewModelBuilder WithCNPJ(string cnpj)
    {
        _cnpj = cnpj;
        return this;
    }

    public EditSupplierViewModelBuilder WithStreet(string street)
    {
        _street = street;
        return this;
    }

    public EditSupplierViewModelBuilder WithNeighborhood(string neighborhood)
    {
        _neighborhood = neighborhood;
        return this;
    }

    public EditSupplierViewModelBuilder WithNumber(string number)
    {
        _number = number;
        return this;
    }

    public EditSupplierViewModelBuilder WithCity(string city)
    {
        _city = city;
        return this;
    }

    public EditSupplierViewModelBuilder WithState(string state)
    {
        _state = state;
        return this;
    }

    public EditSupplierViewModelBuilder WithZipCode(string zipCode)
    {
        _zipCode = zipCode;
        return this;
    }

    public EditSupplierViewModel Build()
    {
        return new EditSupplierViewModel
        {
            Id = _id,
            Name = _name,
            CNPJ = _cnpj,
            Street = _street,
            Neighborhood = _neighborhood,
            Number = _number,
            City = _city,
            State = _state,
            ZipCode = _zipCode
        };
    }
}