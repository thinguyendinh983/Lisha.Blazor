﻿using Lisha.Blazor.Infrastructure.ApiClient;

namespace Lisha.Blazor.Infrastructure.Auth
{
    public interface IAuthenticationService
    {
        AuthProvider ProviderType { get; }

        void NavigateToExternalLogin(string returnUrl);

        Task<bool> LoginAsync(TokenRequest request);

        Task LogoutAsync();

        Task ReLoginAsync(string returnUrl);
    }
}
