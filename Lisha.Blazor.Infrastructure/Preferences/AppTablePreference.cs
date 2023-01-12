using Lisha.Shared.Notifications;

namespace Lisha.Blazor.Infrastructure.Preferences
{
    public class AppTablePreference : INotificationMessage
    {
        public bool IsDense { get; set; }
        public bool IsStriped { get; set; }
        public bool HasBorder { get; set; }
        public bool IsHoverable { get; set; } = true;
    }
}
