using Lisha.Blazor.Components.Common;
using Lisha.Blazor.Infrastructure.ApiClient;
using Lisha.Blazor.Shared;
using Microsoft.AspNetCore.Components;

namespace Lisha.Blazor.Pages.Authentication
{
    public partial class ForgotPassword
    {
        private readonly ForgotPasswordRequest _forgotPasswordRequest = new();
        private CustomValidation? _customValidation;
        private bool BusySubmitting { get; set; }

        [Inject]
        private IUsersClient UsersClient { get; set; } = default!;

        private async Task SubmitAsync()
        {
            BusySubmitting = true;

            await ApiHelper.ExecuteCallGuardedAsync(
                () => UsersClient.ForgotPasswordAsync(_forgotPasswordRequest),
                Snackbar,
                _customValidation);

            BusySubmitting = false;
        }
    }
}
