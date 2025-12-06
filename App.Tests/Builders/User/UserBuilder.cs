using System.Data.Common;
using CollectApp.Models;

namespace CollectAppTests.Builders;

public class UserBuilder
{
    private string _id = "1";
    private string _fullName = "João Silva";
    private string _role = "Comprador";
    private UserStatus _status = UserStatus.Ativo;
    private DateTime _createdAt = new DateTime(2023, 6, 1);

    public UserBuilder WithId(string id)
    {
        _id = id;
        return this;
    }

    public UserBuilder WithFullName(string fullName)
    {
        _fullName = fullName;
        return this;
    }

    public UserBuilder WithRole(string role)
    {
        _role = role;
        return this;
    }

    public UserBuilder WithStatus(UserStatus status)
    {
        _status = status;
        return this;
    }

    public UserBuilder WithCreatedAt(DateTime createdAt)
    {
        _createdAt = createdAt;
        return this;
    }

    public ApplicationUser Build()
    {
        return new ApplicationUser
        {
            Id = _id,
            FullName = _fullName,
            Role = _role,
            Status = _status,
            CreatedAt = _createdAt, 
        };
    }
}