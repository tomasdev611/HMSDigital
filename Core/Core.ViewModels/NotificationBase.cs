using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels
{
    public class NotificationBase
    {
        public string Email { get; set; }

        public long PhoneNumber { get; set; }

        public int CountryCode { get; set; }

        public IEnumerable<string> Channels { get; set; }
    }
}
