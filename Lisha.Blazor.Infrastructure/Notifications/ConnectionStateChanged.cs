using Lisha.Shared.Notifications;

namespace Lisha.Blazor.Infrastructure.Notifications
{
    public record ConnectionStateChanged(ConnectionState State, string? Message) : INotificationMessage;
}
