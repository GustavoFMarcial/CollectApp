using CollectApp.ViewModels;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace CollectAppTests.Builders;

public class CreateFilialViewModelBuilder
{
    private string _name = "Filial SP Centro";

    public CreateFilialViewModelBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public CreateFilialViewModel Build()
    {
        return new CreateFilialViewModel
        {
            Name = _name,
        };
    }
}