using CollectApp.ViewModels;

namespace CollectAppTests.Builders;

public class ProductFilterViewModelBuilder
{
    private int _id = 1;
    private string _name = "Papel Reciclável";

    public ProductFilterViewModelBuilder WithId(int id)
    {
        _id = id;
        return this;
    }

    public ProductFilterViewModelBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public ProductFilterViewModel Build()
    {
        return new ProductFilterViewModel
        {
            Id = _id,
            Name = _name,
        };
    }
}