using Android.Widget;
using MobileApp.Assets.Constants;
using MobileApp.Droid.Service;
using MobileApp.Interface;
using Xamarin.Forms;

[assembly: Dependency(typeof(ToastMessageService))]
namespace MobileApp.Droid.Service
{
    class ToastMessageService : IToastMessage
    {
        public void ShowToast(string message, ToastMessageDuration duration = ToastMessageDuration.Short)
        {
            var toastLength = ToastLength.Short;
            if (duration == ToastMessageDuration.Long)
            {
                toastLength = ToastLength.Long;
            }
            Toast.MakeText(Android.App.Application.Context, message, toastLength).Show();
        }
    }
}