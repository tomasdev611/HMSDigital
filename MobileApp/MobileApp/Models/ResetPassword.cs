using System;
namespace MobileApp.Models
{
    public class ResetPassword
    {
        public string Email { get; set; }

        public string OTP { get; set; }

        public string Password { get; set; }
    }
}
