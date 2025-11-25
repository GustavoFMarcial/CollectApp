using CollectApp.Models;

namespace CollectAppTests.Builders;

public class ProductBuilder
{
    private int _id = 1;
    private DateTime _createdAt = new DateTime(2023, 3, 15);
    private string _name = "Papel Reciclável";

    public ProductBuilder WithId(int id)
    {
        _id = id;
        return this;
    }

    public ProductBuilder WithCreatedAt(DateTime createdAt)
    {
        _createdAt = createdAt;
        return this;
    }

    public ProductBuilder WithName(string name)
    {
        _name = name;
        return this;
    }
    
    public Product Build()
    {
        return new Product
        {
            Id = _id,
            CreatedAt = _createdAt,
            Name = _name,
        };
    }
};