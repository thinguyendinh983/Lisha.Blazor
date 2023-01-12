using Lisha.Shared.Notifications;

namespace Lisha.Blazor.Infrastructure.Notifications
{
    public interface INotificationPublisher
    {
        Task PublishAsync(INotificationMessage notification);
    }
}
