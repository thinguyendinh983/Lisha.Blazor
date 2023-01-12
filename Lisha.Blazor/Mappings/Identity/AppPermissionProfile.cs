using AutoMapper;
using Lisha.Blazor.Pages.Identity.Roles;
using Lisha.Shared.Authorization;

namespace Lisha.Blazor.Mappings.Identity
{
    public class AppPermissionProfile : Profile
    {
        public AppPermissionProfile()
        {
            CreateMap<AppPermission, PermissionViewModel>();
        }
    }
}
