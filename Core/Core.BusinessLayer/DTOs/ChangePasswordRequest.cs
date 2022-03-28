using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.BusinessLayer.DTOs
{
    public class ChangePasswordRequest
    {
        public string AccessToken { get; set; }

        public string OldPassword { get; set; }

        public string NewPassword { get; set; }
    }
}
