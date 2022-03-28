using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Notification.NotificationProcessor
{
    public interface ISmsService
    {
        Task<bool> SendSmsAsync(IEnumerable<string> recipients, string bodyContent);
    }
}
