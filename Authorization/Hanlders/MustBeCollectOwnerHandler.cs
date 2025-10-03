using System.Security.Claims;
using CollectApp.Authorization.Requirements;
using CollectApp.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace CollectApp.Authorization.Handlers
{
    public class MustBeCollectOwnerHandler : AuthorizationHandler<MustBeCollectOwnerRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MustBeCollectOwnerRequirement requirement)
        {
            string? currentUserId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(currentUserId))
            {
                return Task.CompletedTask;;
            }

            if (context.User.IsInRole("Admin"))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            string? collectUserId = GetCollectUserIdFromResource(context.Resource);

            if (currentUserId == collectUserId)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }

        private static string? GetCollectUserIdFromResource(object? resource)
        {
            if (resource is string id)
            {
                return id;
            }

            return null;
        }
    }
}