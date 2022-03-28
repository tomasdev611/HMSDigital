using System.Collections.Generic;
using System.Threading.Tasks;

namespace Notification.NotificationProcessor
{
    public interface IPushNotificationService
    {
        Task<bool> SendPushNotificationAsync(IEnumerable<string> recipients, string title, string bodyContent, string platform);
    }
}
