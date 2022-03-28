using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Notification.ViewModels
{
    public class Email
    {
        public string EmailAddress { get; set; }

        public string EmailType { get; set; }

        public bool IsPrimary { get; set; }

        public bool IsVerified { get; set; }

    }
}
