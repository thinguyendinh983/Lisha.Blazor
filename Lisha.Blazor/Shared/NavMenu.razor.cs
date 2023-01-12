using Lisha.Blazor.Infrastructure.Auth;
using Lisha.Blazor.Infrastructure.Common;
using Lisha.Shared.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Lisha.Blazor.Shared
{
    public partial class NavMenu
    {
        [CascadingParameter]
        protected Task<AuthenticationState> AuthState { get; set; } = default!;
        [Inject]
        protected IAuthorizationService AuthService { get; set; } = default!;

        private string? _hangfireUrl;
        private bool _canViewHangfire;
        private bool _canViewDashboard;
        private bool _canViewRoles;
        private bool _canViewUsers;
        private bool CanViewAdministrationGroup => _canViewUsers || _canViewRoles;

        protected override async Task OnParametersSetAsync()
        {
            _hangfireUrl = Config[ConfigNames.ApiBaseUrl] + "jobs";
            var user = (await AuthState).User;
            _canViewHangfire = await AuthService.HasPermissionAsync(user, AppAction.View, AppResource.Hangfire);
            _canViewDashboard = await AuthService.HasPermissionAsync(user, AppAction.View, AppResource.Dashboard);
            _canViewRoles = await AuthService.HasPermissionAsync(user, AppAction.View, AppResource.Roles);
            _canViewUsers = await AuthService.HasPermissionAsync(user, AppAction.View, AppResource.Users);
        }
    }
}
