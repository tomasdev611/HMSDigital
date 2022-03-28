using System;
using System.Collections.Generic;
using System.Text;

namespace Notification.NotificationProcessor
{
    public class EmailConfig
    {
        public IEnumerable<string> AllowedList { get; set; }

        public bool IsGatedByAllowedList { get; set; }
    }
}
