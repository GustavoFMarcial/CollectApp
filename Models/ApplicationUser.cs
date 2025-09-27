using Microsoft.AspNetCore.Identity;

namespace CollectApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}