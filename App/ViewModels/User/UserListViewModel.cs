using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CollectApp.Models;

namespace CollectApp.ViewModels;

public class UserListViewModel
{
    public string Id { get; set; } = string.Empty;

    [DisplayName("Criação usuário")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime CreatedAt { get; set; }

    [DisplayName("Nome completo")]
    public string FullName { get; set; } = string.Empty;

    [DisplayName("Cargo")]
    public string Role { get; set; } = string.Empty;

    public UserStatus Status { get; set; }
}