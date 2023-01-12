using Lisha.Blazor.Infrastructure.ApiClient;
using Lisha.Blazor.Infrastructure.Notifications;
using Lisha.Blazor.Shared;
using Lisha.Shared.Notifications;
using MediatR.Courier;
using Microsoft.AspNetCore.Components;

namespace Lisha.Blazor.Pages.Personal
{
    public partial class Dashboard
    {
        public int UserCount { get; set; }
        [Parameter]
        public int RoleCount { get; set; }

        [Inject]
        private IDashboardClient DashboardClient { get; set; } = default!;
        [Inject]
        private ICourier Courier { get; set; } = default!;

        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            Courier.SubscribeWeak<NotificationWrapper<StatsChangedNotification>>(async _ =>
            {
                await LoadDataAsync();
                StateHasChanged();
            });

            await LoadDataAsync();

            _loaded = true;
        }

        private async Task LoadDataAsync()
        {
            if (await ApiHelper.ExecuteCallGuardedAsync(
                    () => DashboardClient.GetAsync(),
                    Snackbar)
                is StatsDto statsDto)
            {
                UserCount = statsDto.UserCount;
                RoleCount = statsDto.RoleCount;
            }
        }
    }
}
