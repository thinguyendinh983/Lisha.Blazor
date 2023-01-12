using Lisha.Shared.Notifications;
using MediatR;

namespace Lisha.Blazor.Infrastructure.Notifications
{
    public class NotificationWrapper<TNotificationMessage> : INotification
    where TNotificationMessage : INotificationMessage
    {
        public NotificationWrapper(TNotificationMessage notification) => Notification = notification;

        public TNotificationMessage Notification { get; }
    }
}
