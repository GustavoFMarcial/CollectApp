using CollectApp.ViewModels;

namespace CollectApp.Tests.Builders;

public class EditUserViewModelBuilder
{
    private string _id = "1";
    private string _fullName = "João da Silva";
    private string _role = "Gestor";

    public EditUserViewModelBuilder WithId(string id)
    {
        _id = id;
        return this;
    }

    public EditUserViewModelBuilder WithFullName(string fullName)
    {
        _fullName = fullName;
        return this;
    }

    public EditUserViewModelBuilder WithRole(string role)
    {
        _role = role;
        return this;
    }

    public EditUserViewModel Build()
    {
        return new EditUserViewModel
        {
            Id = _id,
            FullName = _fullName,
            Role = _role
        };
    }
}