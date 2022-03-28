using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels
{
    public class ChangePasswordRequest
    {
        public string OldPassword { get; set; }

        public string NewPassword { get; set; }
    }
}
