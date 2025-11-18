using Microsoft.AspNetCore.Identity;

namespace CollectApp.Models;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public UserStatus Status { get; set; } = UserStatus.Ativo;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}