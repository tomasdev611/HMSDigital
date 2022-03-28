using Foundation;
using MobileApp.Interface;
using MobileApp.iOS.Service;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(SettingsHelper))]
namespace MobileApp.iOS.Service
{
    class SettingsHelper : ISettingsHelper
    {
        public void OpenAppSettings()
        {
            var url = new NSUrl($"app-settings:{NSBundle.MainBundle.BundleIdentifier}");
            UIApplication.SharedApplication.OpenUrl(url);
        }

        public void OpenLocationSetting()
        {
            var url = new NSUrl("App-Prefs:root=LOCATION_SERVICES");

            if (UIApplication.SharedApplication.CanOpenUrl(url))
            {
                UIApplication.SharedApplication.OpenUrl(url);
            }
        }
    }
}