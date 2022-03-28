
using System.Collections.Generic;

namespace HMSDigital.Core.ViewModels
{
    public class UserPasswordRequest : NotificationBase
    {
        public string Password { get; set; }

        public bool Permanent { get; set; }

    }
}
