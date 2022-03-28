using NotificationSDK.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Notification.ViewModels
{
    public class NotificationRequest
    {
        public IEnumerable<string> Recipients { get; set; }

        public string Channel { get; set; }

        public string Content { get; set; }

        public string Subject { get; set; }

        public string Platform { get; set; }

        public IEnumerable<FileAttachment> Attachments { get; set; }
    }
}
