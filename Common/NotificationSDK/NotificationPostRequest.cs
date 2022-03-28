using NotificationSDK.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotificationSDK
{
    public class NotificationPostRequest
    {
        public IEnumerable<string> Channels { get; set; }

        public IEnumerable<string> Emails { get; set; }

        public IEnumerable<string> PhoneNumbers { get; set; }

        public IEnumerable<int> UserIds { get; set; }

        public string Subject { get; set; }

        public string PlainTextBody { get; set; }

        public string RichTextBody { get; set; }

        public IEnumerable<FileAttachment> Attachments { get; set; }

    }
}
