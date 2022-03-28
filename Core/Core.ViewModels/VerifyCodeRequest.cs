using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels
{
    public class VerifyCodeRequest : ConfirmationCodeRequest
    {
        public string Code { get; set; }
    }
}
