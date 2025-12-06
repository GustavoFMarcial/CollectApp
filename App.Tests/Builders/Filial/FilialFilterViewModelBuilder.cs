using CollectApp.ViewModels;

namespace CollectAppTests.Builders;

public class FilialFilterViewModelBuilder
{
    private int _id;
    private string _name = string.Empty;

    public FilialFilterViewModelBuilder WithId(int id)
    {
        _id = id;
        return this;
    }

    public FilialFilterViewModelBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public FilialFilterViewModel Build()
    {
        return new FilialFilterViewModel
        {
            Id = _id,
            Name = _name,
        };
    }
}