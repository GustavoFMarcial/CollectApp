using CollectApp.ViewModels;

namespace CollectAppTests.Builders;

public class CreateUserViewModelBuilder
{
    private string _fullName = "João da Silva";
    private string _username = "joao.silva";
    private string _password = "Senha@123";
    private string _confirmPassword = "Senha@123";
    private string _role = "Comrprador";

    public CreateUserViewModelBuilder WithFullName(string fullName)
    {
        _fullName = fullName;
        return this;
    }

    public CreateUserViewModelBuilder WithUsername(string username)
    {
        _username = username;
        return this;
    }

    public CreateUserViewModelBuilder WithPassword(string password)
    {
        _password = password;
        return this;
    }

    public CreateUserViewModelBuilder WithConfirmPassword(string confirmPassword)
    {
        _confirmPassword = confirmPassword;
        return this;
    }

    public CreateUserViewModelBuilder WithRole(string role)
    {
        _role = role;
        return this;
    }

    public CreateUserViewModel Build()
    {
        return new CreateUserViewModel
        {
            FullName = _fullName,
            Username = _username,
            Password = _password,
            ConfirmPassword = _confirmPassword,
            Role = _role
        };
    }
}