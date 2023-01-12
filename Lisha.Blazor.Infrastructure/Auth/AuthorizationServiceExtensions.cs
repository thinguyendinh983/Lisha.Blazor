using Microsoft.AspNetCore.Authorization;

namespace Lisha.Blazor.Infrastructure.Auth
{
    public static class AuthorizationServiceExtensions
    {
        public static async Task<bool> HasPermissionAsync(this IAuthorizationService service, ClaimsPrincipal user, string action, string resource) =>
            (await service.AuthorizeAsync(user, null, AppPermission.NameFor(action, resource))).Succeeded;
    }
}
