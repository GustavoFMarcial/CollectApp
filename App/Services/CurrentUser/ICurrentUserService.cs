using System.Security.Claims;

namespace CollectApp.Services;

public interface ICurrentUserService
{
    string UserId { get; }
    ClaimsPrincipal User { get; }
}