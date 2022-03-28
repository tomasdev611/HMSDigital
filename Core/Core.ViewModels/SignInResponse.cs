using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels
{
    public class SignInResponse
    {
        public string IdToken { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
