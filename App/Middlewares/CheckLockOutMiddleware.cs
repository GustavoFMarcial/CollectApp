using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using CollectApp.Models;

namespace CollectApp.Middlewares;

public class CheckLockoutMiddleware
{
    private readonly RequestDelegate _next;

    public CheckLockoutMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, UserManager<ApplicationUser> userManager)
    {

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
