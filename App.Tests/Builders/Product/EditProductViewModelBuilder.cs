using CollectApp.ViewModels;

namespace CollectAppTests.Builders;

public class EditProductViewModelBuilder
{
    private int _id = 1;
    private string _name = "Batata";

    public EditProductViewModelBuilder WithId(int id)
    {
        _id = id;
        return this;
    }
    public EditProductViewModelBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public EditProductViewModel Build()
    {
        return new EditProductViewModel
        {
            Id = _id,
            Name = _name,
        };
    }
}