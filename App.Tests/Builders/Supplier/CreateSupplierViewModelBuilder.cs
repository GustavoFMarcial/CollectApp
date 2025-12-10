using CollectApp.ViewModels;

namespace CollectApp.Tests.Builders;

public class CreateSupplierViewModelBuilder
{
    private string _name = "Fornecedor Padrão";
    private string _cnpj = "12.345.678/0001-90";
    private string _street = "Rua das Flores";
    private string _neighborhood = "Centro";
    private string _number = "123";
    private string _city = "São Paulo";
    private string _state = "SP";
    private string _zipCode = "01234-567";

    public CreateSupplierViewModelBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public CreateSupplierViewModelBuilder WithCNPJ(string cnpj)
    {
        _cnpj = cnpj;
        return this;
    }

    public CreateSupplierViewModelBuilder WithStreet(string street)
    {
        _street = street;
        return this;
    }

    public CreateSupplierViewModelBuilder WithNeighborhood(string neighborhood)
    {
        _neighborhood = neighborhood;
        return this;
    }

    public CreateSupplierViewModelBuilder WithNumber(string number)
    {
        _number = number;
        return this;
    }

    public CreateSupplierViewModelBuilder WithCity(string city)
    {
        _city = city;
        return this;
    }

    public CreateSupplierViewModelBuilder WithState(string state)
    {
        _state = state;
        return this;
    }

    public CreateSupplierViewModelBuilder WithZipCode(string zipCode)
    {
        _zipCode = zipCode;
        return this;
    }

    public CreateSupplierViewModel Build()
    {
        return new CreateSupplierViewModel
        {
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