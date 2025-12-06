using CollectApp.ViewModels;

namespace CollectAppTests.Builders;

public class CreateProductViewModelBuilder
{
    private string _name = "Tomate";

    public CreateProductViewModelBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public CreateProductViewModel Build()
    {
        return new CreateProductViewModel
        {
            Name = _name,
        };
    }
}