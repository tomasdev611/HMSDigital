using GlobalToast;
using MobileApp.Assets.Constants;
using MobileApp.Interface;
using MobileApp.iOS.Service;
using Xamarin.Forms;

[assembly: Dependency(typeof(ToastMessageService))]
namespace MobileApp.iOS.Service
{
    class ToastMessageService : IToastMessage
    {
        public void ShowToast(string message, ToastMessageDuration duration = ToastMessageDuration.Short)
        {
            var toastDuration = ToastDuration.Regular;
            if (duration == ToastMessageDuration.Long)
            {
                toastDuration = ToastDuration.Long;
            }
            Toast.MakeToast(message)
                .SetDuration(toastDuration)
                .Show();
        }
    }
}