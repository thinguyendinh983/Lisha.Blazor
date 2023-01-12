﻿using Lisha.Blazor.Components.EntityTable;
using Lisha.Blazor.Infrastructure.ApiClient;
using Lisha.Blazor.Infrastructure.Auth;
using Lisha.Shared.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace Lisha.Blazor.Pages.Identity.Users
{
    public partial class Users
    {
        [CascadingParameter]
        protected Task<AuthenticationState> AuthState { get; set; } = default!;
        [Inject]
        protected IAuthorizationService AuthService { get; set; } = default!;

        [Inject]
        protected IUsersClient UsersClient { get; set; } = default!;

        protected EntityClientTableContext<UserDetailsDto, Guid, CreateUserRequest> Context { get; set; } = default!;

        private bool _canExportUsers;
        private bool _canViewRoles;

        // Fields for editform
        protected string Password { get; set; } = string.Empty;
        protected string ConfirmPassword { get; set; } = string.Empty;

        private bool _passwordVisibility;
        private InputType _passwordInput = InputType.Password;
        private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

        protected override async Task OnInitializedAsync()
        {
            var user = (await AuthState).User;
            _canExportUsers = await AuthService.HasPermissionAsync(user, AppAction.Export, AppResource.Users);
            _canViewRoles = await AuthService.HasPermissionAsync(user, AppAction.View, AppResource.UserRoles);

            Context = new(
                entityName: L["User"],
                entityNamePlural: L["Users"],
                entityResource: AppResource.Users,
                searchAction: AppAction.View,
                updateAction: string.Empty,
                deleteAction: string.Empty,
                fields: new()
                {
                new(user => user.FirstName, L["First Name"]),
                new(user => user.LastName, L["Last Name"]),
                new(user => user.UserName, L["User Name"]),
                new(user => user.Email, L["Email"]),
                new(user => user.PhoneNumber, L["Phone Number"]),
                new(user => user.EmailConfirmed, L["Email Confirmation"], Type: typeof(bool)),
                new(user => user.IsActive, L["Active"], Type: typeof(bool))
                },
                idFunc: user => Guid.Parse(user.Id),
                loadDataFunc: async () => (await UsersClient.GetListAsync()).ToList(),
                searchFunc: (searchString, user) =>
                    string.IsNullOrWhiteSpace(searchString)
                        || user.FirstName?.Contains(searchString, StringComparison.OrdinalIgnoreCase) == true
                        || user.LastName?.Contains(searchString, StringComparison.OrdinalIgnoreCase) == true
                        || user.Email?.Contains(searchString, StringComparison.OrdinalIgnoreCase) == true
                        || user.PhoneNumber?.Contains(searchString, StringComparison.OrdinalIgnoreCase) == true
                        || user.UserName?.Contains(searchString, StringComparison.OrdinalIgnoreCase) == true,
                createFunc: user => UsersClient.CreateAsync(user),
                hasExtraActionsFunc: () => true,
                exportAction: string.Empty);
        }

        private void ViewProfile(in Guid userId) =>
            Navigation.NavigateTo($"/users/{userId}/profile");

        private void ManageRoles(in Guid userId) =>
            Navigation.NavigateTo($"/users/{userId}/roles");

        private void TogglePasswordVisibility()
        {
            if (_passwordVisibility)
            {
                _passwordVisibility = false;
                _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
                _passwordInput = InputType.Password;
            }
            else
            {
                _passwordVisibility = true;
                _passwordInputIcon = Icons.Material.Filled.Visibility;
                _passwordInput = InputType.Text;
            }

            Context.AddEditModal.ForceRender();
        }
    }
}
