using System.Security.Claims;
using CollectApp.Authorization.Requirements;
using CollectApp.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace CollectApp.Authorization.Handlers
{
    public class MustBeCollectOwnerHanlder : AuthorizationHandler<MustBeCollectOwnerRequirement>
    {
        private readonly ICollectRepository _collectRepository;

        public MustBeCollectOwnerHanlder(ICollectRepository collectRepository)
        {
            _collectRepository = collectRepository;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, MustBeCollectOwnerRequirement requirement)
        {
            string? currentUserId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(currentUserId))
            {
                return;
            }

            if (context.User.IsInRole("Admin"))
            {
                context.Succeed(requirement);
            }

            string? collectUserId = GetCollectUserIdFromResource(context.Resource);

            if (currentUserId == collectUserId)
            {
                context.Succeed(requirement);
            }

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