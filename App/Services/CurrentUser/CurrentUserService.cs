using System.Security.Claims;

namespace CollectApp.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string UserId =>
        _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new Exception("Usuário não logado");

    public ClaimsPrincipal User =>
        _httpContextAccessor?.HttpContext?.User ?? throw new Exception("Usuário não logado");
}