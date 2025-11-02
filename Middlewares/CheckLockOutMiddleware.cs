using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using CollectApp.Models;

namespace CollectApp.Middlewares;

public class CheckLockoutMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceScopeFactory _scopeFactory;

    public CheckLockoutMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory)
    {
        _next = next;
        _scopeFactory = scopeFactory;
    }

    public async Task Invoke(HttpContext context)
    {
        using var scope = _scopeFactory.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        if (context.User?.Identity?.IsAuthenticated == true)
        {
            var user = await userManager.GetUserAsync(context.User);
            if (user != null && await userManager.IsLockedOutAsync(user))
            {
                await context.SignOutAsync(IdentityConstants.ApplicationScheme);
                return;
            }
        }

        await _next(context);
    }
}
