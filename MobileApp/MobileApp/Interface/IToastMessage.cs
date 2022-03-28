using MobileApp.Assets.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobileApp.Interface
{
    public interface IToastMessage
    {
        void ShowToast(string message, ToastMessageDuration duration = ToastMessageDuration.Short);
    }
}
