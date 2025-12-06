using CollectApp.Models;

namespace CollectAppTests.Builders;

public class SupplierBuilder
{
    private int _id = 1;
    private DateTime _createdAt = new DateTime(2023, 5, 10);
    private string _cNPJ = "12.345.678/0001-90";
    private string _name = "Fornecedor ABC Ltda";
    private string _street = "Rua das Flores";
    private string _neighborhood = "Centro";
    private string _number = "100";
    private string _city = "São Paulo";
    private string _state = "SP";
    private string _zipCode = "01234-567";

    public SupplierBuilder WithId(int id)
    {
        _id = id;
        return this;
    }

    public SupplierBuilder WithCreatedAt(DateTime createdAt)
    {
        _createdAt = createdAt;
        return this;
    }

    public SupplierBuilder WithCNPJ(string CNPJ)
    {
        _cNPJ = CNPJ;
        return this;
    }

    public SupplierBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public SupplierBuilder WithStreet(string street)
    {
        _street = street;
        return this;
    }

    public SupplierBuilder WithNeighborhood(string neighborhood)
    {
        _neighborhood = neighborhood;
        return this;
    }

    public SupplierBuilder WithNumber(string number)
    {
        _number = number;
        return this;
    }

    public SupplierBuilder WithCity(string city)
    {
        _city = city;
        return this;
    }

    public SupplierBuilder WithState(string state)
    {
        _state = state;
        return this;
    }

    public SupplierBuilder WithZipCode(string zipCode)
    {
        _zipCode = zipCode;
        return this;
    }

    public Supplier Build()
    {
        return new Supplier
        {
            Id = _id,
            CreatedAt = _createdAt,
            CNPJ = _cNPJ,
            Name = _name,
            Street = _street,
            Neighborhood = _neighborhood,
            Number = _number,
            City = _city,
            State = _state,
            ZipCode = _zipCode,
        };
    }
};