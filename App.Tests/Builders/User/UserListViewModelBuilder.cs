using CollectApp.Models;
using CollectApp.ViewModels;

namespace CollectAppTests.Builders;

public class UserListViewModelBuilder
{
    private string _id = string.Empty;
    private DateTime _createdAt;
    private string _fullName = string.Empty;
    private string _role = string.Empty;
    private UserStatus _status;

    public UserListViewModelBuilder FromUser(ApplicationUser u)
    {
        _id = u.Id;
        _createdAt = u.CreatedAt;
        _fullName = u.FullName;
        _role = u.Role;
        _status = u.Status;
        return this;
    }

    public UserListViewModel Build()
    {
        return new UserListViewModel
        {
            Id = _id,
            CreatedAt = _createdAt,
            FullName = _fullName,
            Role = _role,
            Status = _status,
        };
    }
}