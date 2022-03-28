using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels
{
    public class ResetPasswordRequest
    {
        public string Email { get; set; }

        public string Otp { get; set; }

        public string Password { get; set; }
    }
}
