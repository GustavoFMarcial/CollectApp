using CollectApp.ViewModels;

namespace CollectAppTests.Builders;

public class EditFilialViewModelBuilder
{
    private string _name = "Luz Industra";

    public EditFilialViewModelBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public EditFilialViewModel Build()
    {
        return new EditFilialViewModel
        {
            Name = _name,
        };
    }
}