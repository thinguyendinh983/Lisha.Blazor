﻿using AutoMapper;
using Lisha.Blazor.Infrastructure.ApiClient;
using Lisha.Blazor.Infrastructure.Auth;
using Lisha.Blazor.Shared;
using Lisha.Shared.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace Lisha.Blazor.Pages.Identity.Roles
{
    public partial class RolePermissions
    {
        [Parameter]
        public string Id { get; set; } = default!; // from route
        [CascadingParameter]
        protected Task<AuthenticationState> AuthState { get; set; } = default!;
        [Inject]
        protected IAuthorizationService AuthService { get; set; } = default!;
        [Inject]
        protected IRolesClient RolesClient { get; set; } = default!;
        [Inject]
        protected IMapper Mapper { get; set; } = default!;

        private Dictionary<string, List<PermissionViewModel>> _groupedRoleClaims = default!;

        public string _title = string.Empty;
        public string _description = string.Empty;

        private string _searchString = string.Empty;

        private bool _canEditRoleClaims;
        private bool _canSearchRoleClaims;
        private bool _loaded;

        static RolePermissions()
        {
            //TypeAdapterConfig<AppPermission, PermissionViewModel>.NewConfig().MapToConstructor(true); Mapster
            //Mapper.Map<PermissionViewModel>(AppPermission);
        }

        protected override async Task OnInitializedAsync()
        {
            var state = await AuthState;
            _canEditRoleClaims = await AuthService.HasPermissionAsync(state.User, AppAction.Update, AppResource.RoleClaims);
            _canSearchRoleClaims = await AuthService.HasPermissionAsync(state.User, AppAction.View, AppResource.RoleClaims);

            if (await ApiHelper.ExecuteCallGuardedAsync(
                    () => RolesClient.GetByIdWithPermissionsAsync(Id), Snackbar)
                is RoleDto role && role.Permissions is not null)
            {
                _title = string.Format(L["{0} Permissions"], role.Name);
                _description = string.Format(L["Manage {0} Role Permissions"], role.Name);

                var permissions = AppPermissions.Admin;

                _groupedRoleClaims = permissions
                    .GroupBy(p => p.Resource)
                    .ToDictionary(g => g.Key, g => g.Select(p =>
                    {
                        var permission = Mapper.Map<PermissionViewModel>(p);
                        permission.Enabled = role.Permissions.Contains(permission.Name);
                        return permission;
                    }).ToList());
            }

            _loaded = true;
        }

        private Color GetGroupBadgeColor(int selected, int all)
        {
            if (selected == 0)
                return Color.Error;

            if (selected == all)
                return Color.Success;

            return Color.Info;
        }

        private async Task SaveAsync()
        {
            var allPermissions = _groupedRoleClaims.Values.SelectMany(a => a);
            var selectedPermissions = allPermissions.Where(a => a.Enabled);
            var request = new UpdateRolePermissionsRequest()
            {
                RoleId = Id,
                Permissions = selectedPermissions.Where(x => x.Enabled).Select(x => x.Name).ToList(),
            };

            if (await ApiHelper.ExecuteCallGuardedAsync(
                    () => RolesClient.UpdatePermissionsAsync(request.RoleId, request),
                    Snackbar,
                    successMessage: L["Updated Permissions."])
                is not null)
            {
                Navigation.NavigateTo("/roles");
            }
        }

        private bool Search(PermissionViewModel permission) =>
            string.IsNullOrWhiteSpace(_searchString)
                || permission.Name.Contains(_searchString, StringComparison.OrdinalIgnoreCase) is true
                || permission.Description.Contains(_searchString, StringComparison.OrdinalIgnoreCase) is true;
    }

    public record PermissionViewModel : AppPermission
    {
        public bool Enabled { get; set; }

        public PermissionViewModel(string Description, string Action, string Resource, bool IsBasic = false, bool IsRoot = false)
            : base(Description, Action, Resource, IsBasic, IsRoot)
        {
        }
    }
}
