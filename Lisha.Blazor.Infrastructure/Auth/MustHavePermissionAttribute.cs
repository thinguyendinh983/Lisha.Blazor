using Microsoft.AspNetCore.Authorization;

namespace Lisha.Blazor.Infrastructure.Auth
{
    public class MustHavePermissionAttribute : AuthorizeAttribute
    {
        public MustHavePermissionAttribute(string action, string resource) =>
            Policy = AppPermission.NameFor(action, resource);
    }
}
