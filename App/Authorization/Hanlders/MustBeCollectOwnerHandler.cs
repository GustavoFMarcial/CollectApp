using System.Security.Claims;
using CollectApp.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace CollectApp.Authorization.Handlers;

public class MustBeCollectOwnerHandler : AuthorizationHandler<MustBeCollectOwnerRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MustBeCollectOwnerRequirement requirement)
    {
        string? currentUserId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(currentUserId))
        {
            return Task.CompletedTask; ;
        }

        if (context.User.IsInRole("Admin"))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        string collectUserId = "";
        if (context.Resource is string id)
        {
            collectUserId = id;
        }

        if (currentUserId == collectUserId)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}